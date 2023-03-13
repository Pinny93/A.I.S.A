using A.I.S.A.Utils;
using A.I.S.A_.Core;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A.I.S.A.Bot.Discord
{
    internal class DiscordDæmon
    {
        // Add URL: https://discord.com/api/oauth2/authorize?client_id=1084433609076781116&permissions=8&scope=bot
        public const string TOKEN_FILE_PATH = "./Discord/discord.token";

        public static readonly string[] AISAPrefixes = new string[] { "AISA ", "§" };

        private ILogger _logger;
        private DiscordClient _discordClient;
        private AISAHandler _aisaHandler = new AISAHandler();

        public DiscordDæmon(ILogger logger)
        {
            _logger = logger;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.Log(LogLevel.Information, "Starting Dicord Deamon...");

                // Try to read token from Environment variable
                string token = Environment.GetEnvironmentVariable("BOT_TOKEN");

                // If no variable set, read Token from file
                if (String.IsNullOrEmpty(token))
                {
                    if (File.Exists(TOKEN_FILE_PATH))
                    {
                        token = await File.ReadAllTextAsync(TOKEN_FILE_PATH, cancellationToken);
                    }
                    else
                    {
                        File.Create(TOKEN_FILE_PATH);
                        throw new FileNotFoundException($"Token in File '{Path.Combine(Environment.CurrentDirectory, TOKEN_FILE_PATH)}' not found! ");
                    }
                }
                else
                {
                    await File.WriteAllTextAsync(TOKEN_FILE_PATH, token, cancellationToken);
                }

                _discordClient = new DiscordClient(new DiscordConfiguration()
                {
                    Token = token,
                    TokenType = TokenType.Bot,
                    Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents,
                });

                _discordClient.MessageCreated += OnDiscord_MessageCreated;

                await _discordClient.ConnectAsync();

                // Set Initial Bot Status
                //var game = new DiscordGame("Powershell");
                //game.State = "Spielt";

                //await discord.UpdateStatusAsync(game);

                this.IsRunning = true;

                await Task.Delay(-1, cancellationToken);
                this.IsRunning = false;
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Critical, $"Discord Dæmon exited: {e.Message}");
                _logger.Log(LogLevel.Error, $"Exception occured: {e.StackTrace}");

                throw;
            }
        }

        private async Task OnDiscord_MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (e.Author.Username == "A.I.S.A.") { return; }

            if (!e.Channel.IsPrivate && AISAPrefixes.All(curPrefix => !e.Message.Content.ToLower().StartsWith(curPrefix))) { return; }

            string message = e.Message.Content;
            string usedPrefix = AISAPrefixes.FirstOrDefault(curPrefix => message.StartsWith(curPrefix));
            string responseText = String.Empty;
            DiscordMessage response = null;

            TextWriter answerTextWriter = new StringWriter();
            answerTextWriter.Write($"```{message}" + Environment.NewLine);

            // Trim prefix from message
            if (!String.IsNullOrEmpty(usedPrefix))
            {
                message = message.Replace(usedPrefix, "");
            }

            // Message is added to response
            if (!e.Channel.IsPrivate) { await e.Message.DeleteAsync(); }

            // Check if it is permitted to execute commands in this channel
            string[] allowedChannels = new string[] { "bot-commands" };
            if (!e.Channel.IsPrivate && !allowedChannels.Contains(e.Channel.Name))
            {
                responseText += $"Command execution is not allowed in this channel.{Environment.NewLine}" +
                                $"Please use channel(s) '{allowedChannels.ToDelimiterSeparatedString()}' or send me a PM```";
                response = await e.Message.RespondAsync(responseText);
                return;
            }

            if (message.StartsWith("help"))
            {
                responseText += "A.I.S.A. Bot 1.0.0" + Environment.NewLine +
                                "Type --help for help.";

                await answerTextWriter.WriteAsync(responseText);
                return;
            }

            // ############ EXECUTE A.I.S.A ############
            _aisaHandler.Say(message, answerTextWriter);
            // #########################################

            await answerTextWriter.WriteAsync("```");

            await e.Message.RespondAsync(answerTextWriter.ToString());
        }

        public bool IsRunning { get; private set; }
    }
}