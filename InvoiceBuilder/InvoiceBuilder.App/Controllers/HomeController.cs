using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceBuilder.App.Models;
using InvoiceBuilder.UseCase;

namespace InvoiceBuilder.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBuildInvoice _buildInvoice;

        public HomeController(IBuildInvoice buildInvoice)
        {
            _buildInvoice = buildInvoice;
        }

        public IActionResult Index()
        {
            return View(_buildInvoice.GetSalesOrders());
        }

        public IActionResult BuildReport([FromQuery]int salesOrderId)
        {
            var report = _buildInvoice.Build(salesOrderId);
            return File(report, MediaTypeNames.Application.Pdf, "Invoice.pdf");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
