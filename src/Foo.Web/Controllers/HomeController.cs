using System.Diagnostics;
using Foo.Web.Boots;
using Microsoft.AspNetCore.Mvc;

namespace Foo.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public class ErrorViewModel
        {
            public string RequestId { get; set; }

            public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        }
    }
}
