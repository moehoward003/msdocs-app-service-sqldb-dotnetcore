using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace DotNetCoreSqlDb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    //logging.AddConsole();
                    //logging.AddAzureWebAppDiagnostics();
                    //logging.AddApplicationInsights();

                    logging.AddApplicationInsights(configureTelemetryConfiguration: (config) => {
                            var instrumentationKey = string.Format("InstrumentationKey={0}", context.Configuration.GetValue<string>("ApplicationInsights:InstrumentationKey"));
                            config.ConnectionString = instrumentationKey;
                        },
                            configureApplicationInsightsLoggerOptions: (options) => { 
                            }
                    );

                    // Capture all log-level entries from Startup
                    logging.AddFilter<ApplicationInsightsLoggerProvider>(typeof(Startup).FullName, LogLevel.Trace);

                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
