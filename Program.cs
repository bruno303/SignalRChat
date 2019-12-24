using System;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SignalRChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigureApplication();
            CreateHostBuilder(args).Build().Run();
        }

        private static IConfigurationRoot ReadConfig()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .SetBasePath(System.AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, false)
                .Build();

            return configurationRoot;
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(configureOptions => 
                    {
                        IConfigurationRoot configuration = ReadConfig();
                        int portHttp = Convert.ToInt32(configuration.GetSection("WebConfig:PortHttp").Value);
                        int portHttps = Convert.ToInt32(configuration.GetSection("WebConfig:PortHttps").Value);
                        bool useHttps = Convert.ToBoolean(configuration.GetSection("WebConfig:UseHttps").Value);

                        configureOptions.Listen(System.Net.IPAddress.Any, portHttp);

                        if (useHttps) 
                        {
                            configureOptions.Listen(System.Net.IPAddress.Any, portHttps, config => 
                            {
                                config.UseHttps();
                            });
                        }
                    });
                    webBuilder.UseStartup<Startup>();
                });

        private static void ConfigureApplication()
        {
            CultureInfo cultureInfoPtBr = new CultureInfo("pt-BR", false);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfoPtBr;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfoPtBr;
        }
    }
}
