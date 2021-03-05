using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using Microsoft.AspNetCore.Builder;
using System.Xml.Serialization;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Assignment3
{
    public class Startup
    {
        private static void HandleMapTest1(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 1");
            });
        }

        private static void HandleMapTest2(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 2");
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Map("/map1", HandleMapTest1);

            app.Map("/map2", HandleMapTest2);

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello from non-Map delegate. <p>");
            });
        }
    }

    public class Program
    {

        private static async Task Main(string[] args)
        {
         
            await CreateWebHostBuilder(args).Build().RunAsync();
            Console.ReadKey();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .ConfigureKestrel((context, options)=>
            {
                options.Limits.MaxConcurrentConnections = 150;
                options.Limits.MaxConcurrentUpgradedConnections = 150;
                options.Limits.MaxRequestBodySize = int.MaxValue;
                options.Limits.MinRequestBodyDataRate = new MinDataRate(100, TimeSpan.FromSeconds(15));
                options.Limits.MinResponseDataRate = new MinDataRate(100, TimeSpan.FromSeconds(15));
                options.Listen(IPAddress.Loopback, 5000);
                // Set properties and call methods on serverOptions
                
            });

       
        private static byte[] SerializeResponse(string content)
        {
            return Encoding.UTF8.GetBytes(content);
        }
    }
}
