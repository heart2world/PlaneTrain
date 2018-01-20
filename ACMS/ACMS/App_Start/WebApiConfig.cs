using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;

namespace ACMS
{
    public static class WebApiConfig
    {
        public class SessionRouteHandler : HttpControllerHandler, IRequiresSessionState
        {
            public SessionRouteHandler(RouteData routeData)
                : base(routeData)
            {

            }
        }

        public class SessionControllerRouteHandler : HttpControllerRouteHandler
        {
            protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
            {

                return new SessionRouteHandler(requestContext.RouteData);
            }
        }


        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            RouteTable.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            ).RouteHandler = new SessionControllerRouteHandler();
        }
    }
}
