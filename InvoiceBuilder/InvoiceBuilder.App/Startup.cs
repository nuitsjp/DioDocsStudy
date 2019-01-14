using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using InvoiceBuilder.Report;
using InvoiceBuilder.Report.Impl;
using InvoiceBuilder.Repository;
using InvoiceBuilder.Repository.Impl;
using InvoiceBuilder.Transaction;
using InvoiceBuilder.UseCase;
using InvoiceBuilder.UseCase.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Asn1.Cms;
using SimpleInjector;
using SimpleInjector.Extras.DynamicProxy;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

namespace InvoiceBuilder.App
{
    public class Startup : IConnectionFactory, ITemplateService
    {
        private readonly Container _container = new Container();

        private readonly string _connectionString;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _connectionString = Configuration.GetConnectionString("DioDocs");
            C1.Web.Mvc.LicenseManager.Key = "c1v01xIfEjMOSxoEtxofDq8Kzw7HErsKww7PDrGjCkcO+xKbDqsSsacSvwpvClcKVf0t7wr/DpMOQxJzDv8SwxYnDusOhw4TEgMSRw7TEpsOCwqnEmcWhxIrFnsKnwpvCu8WSxJNGw7nEpXPCjMWGwqXDqcKLwq/CicKHxIPDs8OldsKmwpN5xKPEvMKwxI9JxIvEvcOjwoPDq8SOSWTFksSKwq7ElMKxwpDCh1XEsH9Ww6XFpcOUxKvCk8KNe8O3w7PCgcOLxKLFo8SDw7jCsMWfxKlQwoTFosOrw4fFgsSOScO2wqXDuw==";//これはトライアル版のライセンスです。
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            IntegrateSimpleInjector(services);
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(_container));
            services.AddSingleton<IViewComponentActivator>(
                new SimpleInjectorViewComponentActivator(_container));

            services.EnableSimpleInjectorCrossWiring(_container);
            services.UseSimpleInjectorAspNetRequestScoping(_container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            InitializeContainer(app);
            _container.Verify();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            // Add application presentation components:
            _container.RegisterMvcControllers(app);
            _container.RegisterMvcViewComponents(app);

            // Add application services. For instance:
            _container.Register<IBuildInvoice, BuildInvoice>(Lifestyle.Scoped);
            _container.Register<ISalesOrderRepository, SalesOrderRepository>(Lifestyle.Scoped);
            _container.Register<IInvoiceRepository, InvoiceRepository>(Lifestyle.Scoped);
            _container.Register<IReportService, ReportService>(Lifestyle.Scoped);
            _container.Register<ITemplateService>(() => this, Lifestyle.Scoped);

            _container.Register<ITransactionContext, TransactionContext>(Lifestyle.Scoped);
            _container.InterceptWith<TransactionInterceptor>(
                x => x.Namespace == typeof(ISalesOrderRepository).Namespace);
            _container.Register<IConnectionFactory>(() => this);

            // Allow Simple Injector to resolve services from ASP.NET Core.
            _container.AutoCrossWireAspNetComponents(app);
        }

        public IDbConnection Create()
        {
            return new SqlConnection(_connectionString);
        }

        private readonly HttpClient _httpClient = new HttpClient();

        public byte[] Get()
        {
            var task = _httpClient.GetByteArrayAsync(Configuration.GetValue<string>("TemplateUrl"));
            task.Wait();
            return task.Result;
        }
    }
}
