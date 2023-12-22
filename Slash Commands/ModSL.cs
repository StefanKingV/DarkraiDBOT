using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Threading.Tasks;

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

        [SlashCommand("ban", "Banne einen User vom Discord")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Ban(InteractionContext ctx,
                             [Option("User", "Der User der gebannt werden soll")] DiscordUser user,
                             [Option("Grund", "Der Grund für den Bann")] string reason = null,
                             [Option("AnzahlTage", "Lösche alle Nachrichten, die innerhalb der letzten ... Tage vom User geschrieben wurden")] double deleteDays = 0)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {

                var member = (DiscordMember)user;
                await ctx.Guild.BanMemberAsync(member, (int)deleteDays, reason);

                var banMessage = new DiscordEmbedBuilder()
                {
                    Title = $"{member.Mention} wurde vom Server gebannt",
                    Description = $"Discord Name: **{member.Username}**\n" +
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

        [SlashCommand("unban", "Entbanne einen Spieler vom Discord")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Unban(InteractionContext ctx,
                             [Option("User", "Der User der entbannt werden soll")] DiscordUser user,
                             [Option("Grund", "Der Grund für den Unban")] string reason = null)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {

                var member = (DiscordMember)user;
                await ctx.Guild.UnbanMemberAsync(member, reason);

                var banMessage = new DiscordEmbedBuilder()
                {
                    Title = $"{member.Mention} wurde vom Server entbannt",
                    Description = $"Discord Name: **{member.Username}**\n" +
                                  $"Discord ID: {ctx.Member.Id}\n\n" +
                                  $"Grund: **{reason}**\n" +
                                  $"Verantwortlicher Moderator: {ctx.User.Mention}",
                    Color = DiscordColor.Green
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(banMessage));
            }
            else
            {
                var nonAdminMessage = new DiscordEmbedBuilder()
                {
                    Title = "Keinen Zugriff",
                    Description = "Du hast nicht die nötigen Rechte, um einen Spieler zu entbannen",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(nonAdminMessage));
            }

        }
    }
}
