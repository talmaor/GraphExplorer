namespace GraphExplorer.Controllers
{
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