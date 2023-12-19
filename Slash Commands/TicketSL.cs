using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus;
using System.Threading;
using DSharpPlus.Interactivity.Extensions;

namespace DarkBot.Slash_Commands
{
    public class TicketSL : ApplicationCommandModule
    {
        [SlashCommand("Ticketsystem", "Erschaffe das Ticketsystem mit Buttons oder Dropdown Menu :)")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Ticketsystem(InteractionContext ctx,
								[Choice("Button", 0)]
								[Choice("Dropdown Menu", 1)]
								[Option("system", "Buttons oder Dropdown")] long systemChoice = 1)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Ticketsystem wird geladen..."));
            var items = await ctx.Channel.GetMessagesAsync(1);
            await ctx.Channel.DeleteMessagesAsync(items);

            if (systemChoice == 0)
            {
                var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Goldenrod)
                .WithTitle("**Ticketsystem**")
                .WithDescription("Klicke auf einen Button, um ein Ticket der jeweiligen Kategorie zu erstellen")
                )
                .AddComponents(new DiscordComponent[]
                {
                    new DiscordButtonComponent(ButtonStyle.Success, "ticketSupportButton", "Support"),
                    new DiscordButtonComponent(ButtonStyle.Danger, "ticketUnbanButton", "Entbannung"),
                    new DiscordButtonComponent(ButtonStyle.Primary, "ticketOwnerButton", "Inhaber")
                });

                await ctx.Channel.SendMessageAsync(message);
            }

            else if (systemChoice == 1)
            {
                var options = new List<DiscordSelectComponentOption>()
                {
                    new DiscordSelectComponentOption(
                        "Support Ticket",
                        "label_with_desc_emoji",
                        "Öffne ein Ticket, für Fragen, Probleme, etc.!",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Bot.Client, ":envelope:"))),

                    new DiscordSelectComponentOption(
                        "Entbannungs Ticket",
                        "label_no_desc")
                    //new DiscordSelectComponentOption(
                    //    "Entbannungs Ticket",
                    //    "label_with_desc_emoji",
                    //    "Öffne ein Ticket, um über eine Entbannung zu diskutieren!",
                    //    emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Bot.Client, ":tickets:"))),
                    //
                    //new DiscordSelectComponentOption(
                    //    "Inhaber Ticket",
                    //    "label_with_desc_emoji",
                    //    "Öffne ein Ticket, um mit dem Inhaber zu sprechen!",
                    //    emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Bot.Client, ":man_construction_worker:")))
                };

                var ticketdropdown = new DiscordSelectComponent("ticketdropdown", "Wähle eine passende Kategorie aus", options, false, 1, 1);

                var message = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                    
                    .WithColor(DiscordColor.Goldenrod)
                    .WithTitle("**Ticketsystem**")
                    .WithDescription("Öffne das Dropdown Menü und wähle eine passende Kategorie aus, um ein Ticket deiner Wahl zu erstellen")
                    )
                    .AddComponents(ticketdropdown);

                await ctx.Channel.SendMessageAsync(message);
            }
            else
            {
                Console.WriteLine($"Error in File TicketSL - {systemChoice} not set");
            }
        }

        [SlashCommand("ticketadd", "Füge einen User zum Ticket hinzu")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Add(InteractionContext ctx,
                             [Option("User", "Der User, der zum Ticket hinzugefügt werden soll")] DiscordUser user)
        {
            //ulong ticketCategoryId = 010101010101; // ID der erlaubten Kategorie

            //if (ctx.Channel.Parent.Id != ticketCategoryId)
            //{
            //    await ctx.Channel.SendMessageAsync("Dieser Befehl kann nur in einem bestimmten Kanal ausgeführt werden.");
            //    return;
            //}

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "User hinzugefügt!",
                Description = $"{user.Mention} wurde von {ctx.User.Mention} zum Ticket {ctx.Channel.Mention} hinzugefügt!\n",
                Timestamp = DateTime.UtcNow
            };
            await ctx.CreateResponseAsync(embedMessage);
           
            await ctx.Channel.AddOverwriteAsync((DiscordMember)user, Permissions.AccessChannels);
        }

        [SlashCommand("ticketremove", "Entferne einen User vom Ticket")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Remove(InteractionContext ctx,
                             [Option("User", "Der User, der von diesem Ticket entfernt werden soll")] DiscordUser user)
        {
            //ulong ticketCategoryId = 010101010101; // ID der erlaubten Kategorie

            //if (ctx.Channel.Parent.Id != ticketCategoryId)
            //{
            //    await ctx.Channel.SendMessageAsync("Dieser Befehl kann nur in einem bestimmten Kanal ausgeführt werden.");
            //    return;
            //}

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "User entfernt!",
                Description = $"{user.Mention} wurde von {ctx.User.Mention} aus diesem Ticket entfernt!\n",
                Timestamp = DateTime.UtcNow
            };
            await ctx.CreateResponseAsync(embedMessage);

            await ctx.Channel.AddOverwriteAsync((DiscordMember)user, Permissions.None);
        }

        [SlashCommand("ticketrename", "Ändere den Namen vom Ticket")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Rename(InteractionContext ctx,
                             [Option("Name", "Gib dem Ticket einen neuen Namen")] string newChannelName)
        {
            var oldChannelName = ctx.Channel.Mention;

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "Ticket umbenannt!",
                Description = $"Das Ticket {ctx.Channel.Mention} wurde von {ctx.User.Mention} umbenannt!\n\n" +
                              $"Das Ticket heißt nun \"{newChannelName}\"",
                Timestamp = DateTime.UtcNow
            };
            await ctx.CreateResponseAsync(embedMessage);

            await ctx.Channel.ModifyAsync(properties => properties.Name = newChannelName);
        }

        [SlashCommand("ticketclose", "Schließe ein Ticket")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Close(InteractionContext ctx)
        {
            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "🔒 Ticket geschlossen!",
                Description = $"Das Ticket wurde von {ctx.User.Mention} geschlossen!\n",
                Timestamp = DateTime.UtcNow
            };
            await ctx.CreateResponseAsync(embedMessage);
        }
    }
}