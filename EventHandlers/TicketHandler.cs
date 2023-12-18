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
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.EventArgs;

namespace DarkBot.EventHandlers
{
    public class TicketHandler
    {
		[Obsolete]
		public static async void HandleTicketButtons(ComponentInteractionCreateEventArgs e)
        {
            DiscordMember user = e.User as DiscordMember;
            DiscordGuild guild = e.Guild;

            var overwrites = new List<DiscordOverwriteBuilder>
                {
                    new DiscordOverwriteBuilder().For(guild.EveryoneRole).Deny(Permissions.AccessChannels),
                    new DiscordOverwriteBuilder().For(user).Allow(Permissions.None).Allow(Permissions.AccessChannels),
                };

            DiscordChannel channel = await guild.CreateTextChannelAsync($"{e.User.Username} Ticket", overwrites: overwrites);

            await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent(($"Dein neues Ticket ({channel.Mention}) wurde erstellt!")).AsEphemeral(true));

            var closeButton = new DiscordButtonComponent(ButtonStyle.Secondary, "closeButton", "🔒 Ticket schließen");

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

            // /rename Command
            // /adduser Command
            // /removeuser Command
            // /close Command

        }
    }
}
