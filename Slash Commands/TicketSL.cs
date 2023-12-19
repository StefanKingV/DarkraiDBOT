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
        [SlashCommand("Ticket", "Erschaffe das Ticketsystem mit Buttons :)")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Ticket(InteractionContext ctx)                
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Ticketsystem wird geladen..."));
            var items = await ctx.Channel.GetMessagesAsync(1);
            await ctx.Channel.DeleteMessagesAsync(items);

            var ticketSupportButton = new DiscordButtonComponent(ButtonStyle.Success, "ticketSupportButton", "Support");
            var ticketUnbanButton = new DiscordButtonComponent(ButtonStyle.Danger, "ticketUnbanButton", "Entbannung");
            var ticketOwnerButton = new DiscordButtonComponent(ButtonStyle.Primary, "ticketOwnerButton", "Inhaber");

            var options = new List<DiscordSelectComponentOption>()
            {
                new DiscordSelectComponentOption(
                    "Support Ticket",
                    "label_with_desc_emoji",
                    "Öffne ein Ticket, für Fragen, Probleme, etc.!",
                    emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Bot.Client, ":envelope:"))),

                new DiscordSelectComponentOption(
        "Label, no description",
        "label_no_desc"),
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

            var builder = new DiscordMessageBuilder()
                .WithContent("Ticket System")
                .AddComponents(ticketdropdown);

            await builder.SendAsync(ctx.Channel); // Replace with any method of getting a channel. //


            var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Goldenrod)
                .WithTitle("**Ticketsystem**")
                .WithDescription("Klicke auf einen Button, um ein Ticket zu erstellen")
                )
                .AddComponents(ticketSupportButton)
                .AddComponents(ticketUnbanButton)
                .AddComponents(ticketOwnerButton);

            await ctx.Channel.SendMessageAsync(message);
        }

        [SlashCommand("add", "Füge einen User zum Kanal hinzu")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Add(InteractionContext ctx,
                             [Option("User", "Der User, der zum Channel hinzugefügt werden soll")] DiscordUser user)
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
                Description = $"{user.Mention} wurde von {ctx.User.Mention} zum Channel {ctx.Channel.Mention} hinzugefügt!\n",
                Timestamp = DateTime.UtcNow
            };
            await ctx.CreateResponseAsync(embedMessage);
           
            await ctx.Channel.AddOverwriteAsync((DiscordMember)user, Permissions.AccessChannels);
        }
    }
}