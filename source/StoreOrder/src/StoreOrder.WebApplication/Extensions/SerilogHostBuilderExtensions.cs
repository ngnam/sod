using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;

namespace StoreOrder.WebApplication.Extensions
{
    public static class SerilogHostBuilderExtensions
    {
        public static IHostBuilder UseSerilog(this IHostBuilder builder,
            Serilog.ILogger logger = null, bool dispose = false)
        {
            builder.ConfigureServices((context, collection) =>
                collection.AddSingleton<ILoggerFactory>(services => new SerilogLoggerFactory(logger, dispose)));
            return builder;
        }
    }
}
