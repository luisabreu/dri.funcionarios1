using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using site.App_Start;

namespace site {
    public class MvcApplication : HttpApplication {
        protected void Application_Start() {
            AutofacConfig.RegisterForMvc();
            AutofacConfig.RegisterForWebApi(GlobalConfiguration.Configuration);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}