﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.ServiceFabric.AspNetCore.Hosting;
using System.Collections.Immutable;
using System.Fabric;

namespace Sms
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var webHost = BuildWebHost(args);

            using (var fabricRuntime = FabricRuntime.Create())
            {
                fabricRuntime.RegisterStatefulServiceFactory("SmsType", () => new SmsService(webHost));

                webHost.Run();
            }
        }

        private static IWebHost BuildWebHost(string[] args)
        {
            var serviceDescription = new ServiceDescription()
            {
                ServiceType = typeof(SmsService),
                InterfaceTypes = ImmutableArray.Create(typeof(ISmsService))
            };

            var options = new ServiceFabricOptions()
            {
                EndpointName = "SmsTypeEndpoint",
                ServiceDescriptions = ImmutableArray.Create(serviceDescription)
            };

            return new WebHostBuilder().UseDefaultConfiguration(args)
                                       .UseStartup<Startup>()
                                       .UseServiceFabric(options)
                                       .Build();
        }
    }
}
