using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace QNA.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                //  logger.Debug("Ratings Pro Identity Server: Main");
                IConfigurationBuilder builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfigurationRoot configuration = builder.Build();
                var Sentry = Convert.ToBoolean(configuration.GetSection("IsSentryLogging").Value);
              
                BuildWebHost(args, Sentry).Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "RP Identity Server stopped because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHost BuildWebHost(string[] args, bool isSentry)
        {
            if (isSentry)
            {
                return WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options => options.AddServerHeader = false)
               .UseStartup<Startup>()
               .UseStartup<Startup>()
               .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                })
                .UseSentry()
                //if (isSentry)
                //    webHost.UseSentry();
                //else
                //    webHost.UseNLog();
                .Build();
            }
            else
            {
                return WebHost.CreateDefaultBuilder(args)
               .UseKestrel(options => options.AddServerHeader = false)
              .UseStartup<Startup>()
              .UseStartup<Startup>()
              .ConfigureLogging(logging =>
              {
                  logging.ClearProviders();
                  logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
              })
               .UseNLog()
               .Build();
            }
            //return webHost;
        }
        //    public static IWebHost BuildWebHost(string[] args) =>
        //        WebHost.CreateDefaultBuilder(args)
        //            .UseKestrel(options => options.AddServerHeader = false)
        //            .UseStartup<Startup>()
        //            .ConfigureLogging(logging =>
        //            {
        //                logging.ClearProviders();
        //                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        //            })
        //            .UseNLog()  // setup NLog for Dependency injection
        //            .Build();
         }
    }
