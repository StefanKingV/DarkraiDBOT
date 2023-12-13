using DiscordBotTemplate.Commands;
using DiscordBotTemplate.Config;
using DiscordBotTemplate.Slash_Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.args;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;

namespace DiscordBotTemplate
{
    public sealed class Bot
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
            Client.ComponentInteractionCreated += ButtonPressResponse;
            Client.ComponentInteractionCreated += TicketButtonResponse;
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
            Commands.RegisterCommands<BasicCommands>();

            // Slash Commands
            slashCommandsConfig.RegisterCommands<FunSL>(1076192773776081029); // GuildID
            slashCommandsConfig.RegisterCommands<ModSL>(1076192773776081029);

            //8. Connect to get the Bot online
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static Task TicketButtonResponse(DiscordClient sender, ComponentInteractionCreateEventArgs args)
        {
            if (args.Id == "create_ticket")
            {
                // Hier kannst du die Logik für die Erstellung des Tickets und des Channels implementieren
                // Zum Beispiel einen neuen Channel mit dem Namen "ticket-<ticketnummer>" erstellen
                // und die Berechtigungen entsprechend setzen.
                var ticketNumber = Guid.NewGuid().ToString().Substring(0, 8); // Funktion zum Generieren einer Ticketnummer
                var channelName = $"ticket-{ticketNumber}";

                var channel = await args.Guild.CreateTextChannelAsync(channelName);

                // Setze die Berechtigungen für den erstellten Channel
                await channel.AddOverwriteAsync(args.User, Permissions.SendMessages | Permissions.ReadMessages);
                foreach (var role in args.Guild.Roles)
                {
                    if (role.Permissions.HasPermission(Permissions.Administrator))
                    {
                        await channel.AddOverwriteAsync(role, Permissions.SendMessages | Permissions.ReadMessages);
                    }
                }

                await args.Interaction.CreateFollowupMessageAsync("Ticket erstellt! Der neue Channel wurde erstellt: " + channel.Mention);
            }
        }

        private static Task OnClientReady(DiscordClient sender, ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }

        private static async Task ButtonPressResponse(DiscordClient sender, ComponentInteractionCreateEventArgs args)
        {
            if (args.Interaction.Data.CustomId == "1")
            {
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Button 1 wurde gedrückt"));
            }
            else if (args.Interaction.Data.CustomId == "2")
            {
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Button 2 wurde gedrückt"));
            }
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
    }
}
