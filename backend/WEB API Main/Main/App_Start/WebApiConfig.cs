using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Cors;
using System.Web.Http.Routing;

namespace ReusableWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            //Enable Cors
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            //Global Authorization specified:
            //config.Filters.Add(new AuthorizeAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes(new CustomDirectRouteProvider());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.RoundtripKind;
            config.Formatters.JsonFormatter.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTimeOffset;
			config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
        }

        //so we can use BaseController:
        public class CustomDirectRouteProvider : DefaultDirectRouteProvider
        {
            protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
            {
                return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(inherit: true);
            }
        }
    }
}
