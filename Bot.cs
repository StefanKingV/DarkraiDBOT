using System;
using System.Threading.Tasks;
using DSharpPlus;
using DarkBot.Commands;
using DarkBot.Config;
using DarkBot.Slash_Commands;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using DSharpPlus.EventArgs;
using DarkBot.EventHandlers;

namespace DarkBot
{
	public class Bot
    {
        public static DiscordClient Client { get; private set; }
        public static CommandsNextExtension Commands { get; private set; }

		[Obsolete]
		static async Task Main(string[] e)
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
			Client.MessageCreated += MessageCreatedEventHandler;

            //6. Set up the Commands Configuration
            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { configJsonFile.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = true,
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.CommandErrored += CommandEventHandler;

            

            //// 7. Register your commands
            // Prefix Commands
            Commands.RegisterCommands<BasicCommands>();

            // Slash Commands
            RegisterSlashCommands(Client);

            // Set Bot Status

            //8. Connect to get the Bot online
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

		private static Task MessageCreatedEventHandler(DiscordClient sender, MessageCreateEventArgs e)
		{
            if (e.Author.Id == e.Message.Interaction.User.Id)
            {
                string userMessageContent = e.Message.Content;
            }
		}

		private static async Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            await Client.UpdateStatusAsync(new DiscordActivity()
            {
                ActivityType = ActivityType.Playing,
                Name = "Valorant"
            });
        }

        [Obsolete]
		private static async Task ButtonPressResponse(DiscordClient sender, ComponentInteractionCreateEventArgs e)
        {
            if (e.Interaction.Data.CustomId == "entryGiveawayButton")
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                .WithContent("Du bist dem **Gewinnspiel** erfolgreich beigetreten! Viel Glück:tada:").AsEphemeral(true));
            }
            else if (e.Interaction.Data.CustomId == "funButton")
            {
                string funCommandsList = "/pingspam" +
                                         "/poll" +
                                         "/giveaway" +
                                         "/avatar" +
                                         "/server";

                await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent(funCommandsList));
            }
            else if (e.Interaction.Data.CustomId == "gameButton")
            {
                string gameCommandsList = "/" +
                                          "/";

                await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent(gameCommandsList));
            }
            else if (e.Interaction.Data.CustomId == "modButton")
            {
                string modCommandsList = "/clear\n" +
                                         "/ban";

                await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent(modCommandsList));
            }
            else if (e.Interaction.Data.CustomId == "ticketSupportButton" 
                  || e.Interaction.Data.CustomId == "ticketUnbanButton" 
                  || e.Interaction.Data.CustomId == "ticketOwnerButton"
                  || e.Interaction.Data.CustomId == "ticketDonationButton"
                  || e.Interaction.Data.CustomId == "ticketDropdown")
            {
                TicketHandler.HandleTicketInteractions(e);
            }
            else if (e.Interaction.Data.CustomId == "closeTicketButton")
            {
                TicketHandler.HandleCloseTicket(e);
            }
        }                                            
                                                     
        private static Task CommandEventHandler(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            return null;
        }

        private static void RegisterSlashCommands(DiscordClient Client) 
        {
            var slashCommandsConfig = Client.UseSlashCommands();

            slashCommandsConfig.RegisterCommands<FunSL>(1076192773776081029); // GuildID
            slashCommandsConfig.RegisterCommands<ModSL>();
            slashCommandsConfig.RegisterCommands<BasicSL>();
            slashCommandsConfig.RegisterCommands<TicketSL>();
            slashCommandsConfig.RegisterCommands<GiveawaySL>();
            slashCommandsConfig.RegisterCommands<CalculatorSL>();
            slashCommandsConfig.RegisterCommands<ImageSL>();
            slashCommandsConfig.RegisterCommands<CasinoSL>();
        }
    }
}
