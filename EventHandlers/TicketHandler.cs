using System;
using System.Collections.Generic;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace DarkBot.EventHandlers
{
	public class TicketHandler
    {
        [Obsolete]
        public static async void HandleTicketInteractions(ComponentInteractionCreateEventArgs e)
        {
            DiscordMember user = e.User as DiscordMember;
            DiscordGuild guild = e.Guild;

            const ulong categoryId = 1187032461914419293;

            var category = guild.GetChannel(categoryId) as DiscordChannel;
            if (category == null || category.Type != ChannelType.Category)
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent("Fehler beim Erstellen des Tickets: Eine Kategorie für Tickets konnte nicht gefunden werden.").AsEphemeral(true));
                return;
            }

            var overwrites = new List<DiscordOverwriteBuilder>
                {
                    new DiscordOverwriteBuilder().For(guild.EveryoneRole).Deny(Permissions.AccessChannels),
                    new DiscordOverwriteBuilder().For(user).Allow(Permissions.None).Allow(Permissions.AccessChannels),
                };

            DiscordChannel channel = await guild.CreateTextChannelAsync($"{e.User.Username}-Ticket", category, overwrites: overwrites, position: 0);


            await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent(($"Dein neues Ticket ({channel.Mention}) wurde erstellt!")).AsEphemeral(true));

            var closeButton = new DiscordButtonComponent(ButtonStyle.Secondary, "closeTicketButton", "🔒 Ticket schließen");

            await channel.SendMessageAsync($"||{user.Mention}||");

            var ticketMessage = new DiscordMessageBuilder()
            .AddEmbed(new DiscordEmbedBuilder()

            .WithColor(DiscordColor.Orange)
            .WithTitle("**__Ticketsystem__**")
            .WithThumbnail(guild.IconUrl)
            .WithTimestamp(DateTime.UtcNow)
            .WithDescription("**In Kürze wird sich jemand um dich kümmern!**\n\n" +
                             "Sollte dein Anliegen bereits erledigt sein dann drücke auf 🔒 um dein Ticket zu schließen!")
            )
            .AddComponents(closeButton);

            await channel.SendMessageAsync(ticketMessage);
        }

        public static async void HandleCloseTicket(ComponentInteractionCreateEventArgs e)
        {
            await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                    .WithContent((e.User.Mention)).AsEphemeral(true));


            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "🔒 Ticket geschlossen!",
                Description = $"Das Ticket wurde von {e.User.Mention} geschlossen!\n\n",
                Timestamp = DateTime.UtcNow
            };

            await e.Channel.SendMessageAsync(embedMessage);
        }
    }
}
