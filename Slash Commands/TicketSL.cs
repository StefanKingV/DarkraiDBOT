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
            ulong ticketCategoryId = 010101010101; // ID der erlaubten Kategorie

            if (ctx.Channel.Parent.Id != ticketCategoryId)
            {
                await ctx.Channel.SendMessageAsync("Dieser Befehl kann nur in einem bestimmten Kanal ausgeführt werden.");
                return;
            }

            await ctx.Channel.AddOverwriteAsync((DiscordMember)user, Permissions.SendMessages);

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "User hinzugefügt!",
                Description = $"{user.Mention} wurde von {ctx.User.Mention} zum Channel {ctx.Channel.Mention} hinzugefügt!\n",
                Timestamp = DateTime.UtcNow
            };
            await ctx.Channel.SendMessageAsync(embedMessage);
        }
    }
}