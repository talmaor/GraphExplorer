using System.Configuration;
using System.Web.Mvc;

namespace GraphExplorer.GraphExplorer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            if (ConfigurationManager.AppSettings["AuthenticationEnabled"] == "true")
            {
                filters.Add(new AuthorizeAttribute());
            }  
        }
    }
}
