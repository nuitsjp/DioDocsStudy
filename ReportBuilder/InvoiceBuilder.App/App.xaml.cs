using System.Configuration;
using System.Data.Common;
using System.Windows;
using InvoiceBuilder.ReportBuilder;
using InvoiceBuilder.Repository;
using InvoiceBuilder.Repository.Impl;
using InvoiceBuilder.Transaction;
using InvoiceBuilder.UseCase;
using InvoiceBuilder.UseCase.Impl;
using InvoiceBuilder.ViewModel;
using SimpleInjector;
using SimpleInjector.Extras.DynamicProxy;

namespace InvoiceBuilder.App
{
    /// <inheritdoc />
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App
    {
        private readonly Container _container = new Container();
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var settings = ConfigurationManager.ConnectionStrings["AdventureWorks2017"];
            var factory = DbProviderFactories.GetFactory(settings.ProviderName);

            var transactionContext = new TransactionContext();

            _container.InterceptWith(
                x => x.Namespace == typeof(ISalesOrderRepository).Namespace,
                new TransactionInterceptor(
                    transactionContext,
                    () =>
                    {
                        var connection = factory.CreateConnection();
                        connection.ConnectionString = settings.ConnectionString;
                        return connection;
                    }));
            _container.Register<ITransactionContext>(() => transactionContext);
            _container.Register<IReportBuilder>(
                () => new ReportBuilder.Impl.ReportBuilder(
                    InvoiceBuilder.App.Properties.Settings.Default.Endpoint));
            _container.Register<ISalesOrderRepository, SalesOrderRepository>();
            _container.Register<ISalesOrderDetailRepository, SalesOrderDetailRepository>();
            _container.Register<IBuildInvoice, BuildInvoice>();
            _container.Verify();

            ViewModelLocator.ViewModelProvider = new ViewModelProvider(_container);
        }

        private class ViewModelProvider : IViewModelProvider
        {
            private readonly Container _container;

            public ViewModelProvider(Container container)
            {
                _container = container;
            }

            public T Resolve<T>() where T : class
            {
                return _container.GetInstance<T>();
            }
        }
    }
}
