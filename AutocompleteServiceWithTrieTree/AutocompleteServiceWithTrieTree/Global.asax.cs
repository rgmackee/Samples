using AutoComplete;
using LightInject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace AutocompleteServiceWithTrieTree
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            ConfigureContainer();
        }

        private void ConfigureContainer()
        {
            using (var container = new ServiceContainer())
            {
                container.RegisterApiControllers();
                //register other services
                var fileName = ConfigurationManager.AppSettings["dataFileName"];
                var filePath = Server.MapPath($"~/{fileName}");
                var names = File.ReadAllLines(filePath);
                container.Register<IClient>(factory => new Client(names), new PerContainerLifetime());
                container.EnableWebApi(GlobalConfiguration.Configuration);
            }
        }
    }
}
