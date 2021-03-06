﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Myers.WebSockDemo.Client.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Chat", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ChatRoom",
                url: "Chat/ChatRoom/{username}",
                defaults: new { controller = "Chat", action = "ChatRoom" }
            );
        }
    }
}