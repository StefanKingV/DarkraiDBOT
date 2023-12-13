using DiscordBotTemplate.Commands;
using DiscordBotTemplate.Config;
using DiscordBotTemplate.Slash_Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using System;
using System.Threading.Tasks;

namespace DiscordBotTemplate
{
    public sealed class Program
    {
        public static DiscordClient Client { get; private set; }
        public static CommandsNextExtension Commands { get; private set; }
        static async Task Main(string[] args)
        {
            //1. Get the details of your config.json file by deserialising it
            var configJsonFile = new JSONReader();
            await configJsonFile.ReadJSON();

            //2. Setting up the Bot Configuration
            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = configJsonFile.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            //3. Apply this config to our DiscordClient
            Client = new DiscordClient(discordConfig);

            //4. Set the default timeout for Commands that use interactivity
            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            //5. Set up the Task Handler Ready event
            Client.Ready += OnClientReady;
            //Client.MessageCreated += MessageCreatedHandler;
            Client.VoiceStateUpdated += VoiceChannelHandler;

            //6. Set up the Commands Configuration
            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { configJsonFile.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = true,
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            var slashCommandsConfig = Client.UseSlashCommands();

            Commands.CommandErrored += CommandEventHandler;

            //7. Register your commands

            // Prefix Commands
            Commands.RegisterCommands<Basic>();

            // Slash Commands
            slashCommandsConfig.RegisterCommands<FunSL>(1076192773776081029); // GuildID
            slashCommandsConfig.RegisterCommands<ModSL>(1076192773776081029);

            //8. Connect to get the Bot online
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static Task CommandEventHandler(CommandsNextExtension sender, CommandErrorEventArgs args)
        {
            return null;
        }

        private static async Task VoiceChannelHandler(DiscordClient sender, VoiceStateUpdateEventArgs args)
        {
            if (args.Before == null && args.Channel.Name == "Lobby")
            {
                await args.Channel.SendMessageAsync($"{args.User.Mention} hat den Voice Channel betreten");
            }      
        }

        private static async Task MessageCreatedHandler(DiscordClient sender, MessageCreateEventArgs args)
        {
            await args.Channel.SendMessageAsync("Trigger wurde ausgelöst");
        }

        private static Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
