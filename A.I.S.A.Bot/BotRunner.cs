using A.I.S.A.Bot.Discord;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A.I.S.A.Bot
{
    public class BotRunner
    {
        private Task _deamonTask;
        private DiscordDæmon _discordDeamon;
        private ILogger _logger;

        public BotRunner()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder
                .AddConsole()
                .AddFilter(level => level >= LogLevel.Information)
            );
            var loggerFactory = serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();

            _logger = new Logger<BotRunner>(loggerFactory);
        }

        public async Task InitializeAsync()
        {
            // Init Discord Deamon
            _discordDeamon = new DiscordDæmon(_logger);

            if (!_discordDeamon.IsRunning)
            {
                try
                {
                    _deamonTask = _discordDeamon.InitializeAsync(CancellationToken.None)
                        .ContinueWith(OnDeamon_Exited);
                }
                catch (Exception e)
                {
                    _logger.Log(LogLevel.Error, e.Message);
                    _logger.Log(LogLevel.Warning, e.StackTrace);
                    throw;
                }
            }
        }

        private void OnDeamon_Exited(Task obj)
        {
            _logger.Log(LogLevel.Critical, "Dicord Deamon exited");
        }
    }
}