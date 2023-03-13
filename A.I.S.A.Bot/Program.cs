using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace A.I.S.A.Bot
{
    internal class Program
    {
        internal static async Task Main(string[] args)
        {
            Console.WriteLine("Starting A.I.S.A. Dicord Dæmon ...");
            var builder = WebHost.CreateDefaultBuilder();

            IWebHost? app =
                builder
                    .UseStartup<Startup>()
                    .Build();

            // Run TermuxInstance
            await new BotRunner().InitializeAsync();

            await app.RunAsync();
        }
    }

    internal class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            //app.UseRouting();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}