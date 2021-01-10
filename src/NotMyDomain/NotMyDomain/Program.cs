using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotMyDomain.Interface;
using NotMyDomain.Models;
using System.Threading.Tasks;

namespace NotMyDomain
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            var builder = new HostBuilder();

            builder.ConfigureAppConfiguration((context, configuration) =>
            {
                configuration.AddJsonFile("appsettings.json", optional: false);
            });

            builder.ConfigureServices((context, services) =>
            {
                services.AddHostedService<ConsoleHostedService>();

                services.AddTransient<IConsoleOptionSelector<Application>, ApplicationOptionSelector>();
                services.AddTransient<IConsoleOptionSelector<Account>, AccountOptionSelector>();
            });

            builder.ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.AddEventLog(settings =>
                {
                    settings.SourceName = "NotMyDomain";
                    settings.Filter = (s, level) => level >= LogLevel.Error;
                });
            });

            return builder.RunConsoleAsync();
        }
    }
}