using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Uploader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine($"Starting up... utc={DateTime.UtcNow}");
                CreateHostBuilder(args).Build().Run();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Startup failed. Exception message:\n" + ex.Message);
            }
            finally
            {
                Console.WriteLine($"Shutting down..., utc={DateTime.UtcNow}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
