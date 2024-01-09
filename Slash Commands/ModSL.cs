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
                    Description = "Du hast nicht die nötigen Rechte, um einen User zu Bannen",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(nonAdminMessage));
            }
        }

        [SlashCommand("unban", "Entbanne einen User vom Discord")]
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
                    Description = "Du hast nicht die nötigen Rechte, um einen User zu Entbannen",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(nonAdminMessage));
            }
        }

        [SlashCommand("timeout", "Schicke einen User schlafen")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Timeout(InteractionContext ctx,
                             [Option("User", "Der User der schlafen geschickt werden soll")] DiscordUser user,
                             [Option("Länge", "Die Länge vom Winterschlaf")] int time,
                             [Option("Grund", "Der Grund für den Winterschlaf")] string reason = null)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {

                var member = (DiscordMember)user;
                await ctx.Guild.GetBansAsync();

                var banMessage = new DiscordEmbedBuilder()
                {
                    Title = $"{member.Mention} wurde Schlafen geschickt",
                    Description = $"Discord Name: **{member.Username}**\n" +
                                  $"Discord ID: {ctx.Member.Id}\n\n" +
                                  $"Länge: {time}\n" +
                                  $"Grund: **{reason}**\n" +
                                  $"Verantwortlicher Moderator: {ctx.User.Mention}",
                    Color = DiscordColor.Blurple,
                    Timestamp = DateTime.UtcNow
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(banMessage));
            }
            else
            {
                var nonAdminMessage = new DiscordEmbedBuilder()
                {
                    Title = "Keinen Zugriff",
                    Description = "Du hast nicht die nötigen Rechte, um einen User zu Timeouten",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(nonAdminMessage));
            }
        }

        [SlashCommand("Test123", "Schicke einen User schlafen")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Test123(InteractionContext ctx,
                             [Option("User", "Der User der schlafen geschickt werden soll")] DiscordUser user,
                             [Option("Länge", "Die Länge vom Winterschlaf")] int time,
                             [Option("Grund", "Der Grund für den Winterschlaf")] string reason = null)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {

                var member = (DiscordMember)user;
                await ctx.Guild.GetBansAsync();
                await ctx.Guild.GetAllMembersAsync();
                await ctx.Guild.ListActiveThreadsAsync();
                await ctx.Guild.GetChannelsAsync();

                var banMessage = new DiscordEmbedBuilder()
                {
                    Title = $"{member.Mention} d d d",
                    Description = $"d d: **{member.Username}**\n" +
                                  $"d d: {ctx.Member.Id}\n\n" +
                                  $"d: {time}\n" +
                                  $"d: **{reason}**\n" +
                                  $"d d: {ctx.User.Mention}",
                    Color = DiscordColor.Black,
                    Timestamp = DateTime.UtcNow
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(banMessage));
            }
            else
            {
                var nonAdminMessage = new DiscordEmbedBuilder()
                {
                    Title = "Keinen Zugriff",
                    Description = "Du hast nicht die nötigen Rechte, um einen User zu Timeouten",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(nonAdminMessage));
            }
        }
    }
}
