using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HRMWebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            //config.MapHttpAttributeRoutes();
            config.EnableCors();

            config.MessageHandlers.Add(new HRMWebApp.KPI.Core.Security.TokenValidationHandler());

            config.Routes.MapHttpRoute(
                name: "WithActionApi",
                routeTemplate: "Api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            
        }
    }
}
