using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace test3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // BuildWebHost(args).Run();
            var host = BuildWebHost(args);
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext =
                services.GetRequiredService<EcommerceContext>();
                dbContext.Database.Migrate();
                dbContext.EnsureSeeded();
            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args)
            {
            var port = Environment.GetEnvironmentVariable("PORT");
            var webHost = WebHost.CreateDefaultBuilder(args);
            if (!string.IsNullOrEmpty(port))
            {
                webHost.UseUrls($"http://*:{port}");
            }
            return webHost
                .UseStartup<Startup>()
                .Build();
            }
    }
}
