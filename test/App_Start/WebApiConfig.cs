using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Converters;

namespace test
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Конфигурация и службы веб-API

            // Маршруты веб-API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "FilterApi",
                routeTemplate: "api/{controller}/{search}/{datefrom}/{dateto}/{result}",
                defaults: new { search = RouteParameter.Optional, datefrom = RouteParameter.Optional, dateto = RouteParameter.Optional, result = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            var dateConverter = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
            };

            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(dateConverter);

        }
    }
}
