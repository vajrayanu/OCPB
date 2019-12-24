using System.Web.Http;
using System.Web.Http.Cors;
namespace OCPB
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // var enableCorsAttr = new EnableCorsAttribute(origins: "*", headers: "*", methods: "*") { SupportsCredentials = true };
            // config.EnableCors(enableCorsAttr); 
            // config.MapHttpAttributeRoutes();
            // //config.Routes.MapHttpRoute(
            // //    name: "DefaultApi",
            // //    routeTemplate: "api/{controller}/{action}/{id}",
            // //    defaults: new { controller = "Papi", action = "Get", id = RouteParameter.Optional }
            // //);
            // config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional, action = "Get" }
            //);
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { action = "Get", id = RouteParameter.Optional }
            );
        }
    }

}