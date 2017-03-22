using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace CityOfWindsor.Reports
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Enable cors
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "ServiceRequestApi",
                routeTemplate: "api/{controller}/{dept}/{repmethod}/{from}/{to}/{block}/{street}/{ward}",
                defaults: new
                {
                    dept = RouteParameter.Optional,
                    repmethod = RouteParameter.Optional,
                    from = RouteParameter.Optional,
                    to = RouteParameter.Optional,
                    block = RouteParameter.Optional,
                    street = RouteParameter.Optional,
                    ward = RouteParameter.Optional
                }
            );

            // Adding formatter for Json   
            config.Formatters.JsonFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("type", "json", new MediaTypeHeaderValue("application/json")));

            // Adding formatter for XML   
            config.Formatters.XmlFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("type", "xml", new MediaTypeHeaderValue("application/xml")));
        }
    }
}
