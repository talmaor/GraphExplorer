namespace GraphExplorer.Controllers
{
    using GraphExplorer.Configuration;
    using System.Web.Mvc;

    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.QueryString["tenantId"] != null)
            {
                var tenantId = Request.QueryString["tenantId"];
                DocDbSettings.DatabaseId = tenantId;
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}