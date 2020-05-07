using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using GraphExplorer.Utilities;

namespace GraphExplorer.Controllers
{
    using GraphExplorer.Configuration;
    using System.Web.Mvc;

    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}