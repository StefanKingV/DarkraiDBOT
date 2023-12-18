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

namespace DarkBot.Slash_Commands
{
    public class ModSL : ApplicationCommandModule
    {
        [SlashCommand("clear", "Lösche Nachrichten aus dem Chat")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Clear(InteractionContext ctx, [Option("Anzahl", "Anzahl der Nachrichten die gelöscht werden sollen", autocomplete: false)] double delNumber)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Die letzten {delNumber} Nachrichten wurden erfolgreich gelöscht!"));

            var channel = ctx.Channel;
            var items = await channel.GetMessagesAsync((int)(delNumber + 1));
            await channel.DeleteMessagesAsync(items);

        }

        [SlashCommand("ban", "Verbanne einen User vom Discord")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Ban(InteractionContext ctx,
                             [Option("User", "Der User der gebannt werden soll")] DiscordUser user,
                             [Option("Grund", "Der Grund für den Bann")] string reason = null)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                
                var member = (DiscordMember)user;
                await ctx.Guild.BanMemberAsync(member, 0, reason);

                var banMessage = new DiscordEmbedBuilder()
                {
                    Title = $"{member.Username} wurde vom Server gebannt + {member.AvatarUrl}",
                    Description = $"Discord Name: **{member.Mention}**\n" +
                                  $"Discord ID: {ctx.Member.Id}\n\n" +
                                  $"Grund: **{reason}**\n" +
                                  $"Verantwortlicher Moderator: {ctx.User.Mention}",
                    Color = DiscordColor.Red,
                    Timestamp = DateTime.UtcNow
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(banMessage));
            }
            else
            {
                var nonAdminMessage = new DiscordEmbedBuilder()
                {
                    Title = "Keinen Zugriff",
                    Description = "Du hast nicht die nötigen Rechte, um einen Spieler zu bannen",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(nonAdminMessage));
            }

        }

    }
}
