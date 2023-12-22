using DSharpPlus.SlashCommands;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;

namespace DarkBot.Slash_Commands
{
    public class BasicSL : ApplicationCommandModule
    {
        [SlashCommand("help", "Liste mit allen Befehlen")]
        public async Task HelpCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("**Hilfe**"));

            var funButton = new DiscordButtonComponent(ButtonStyle.Success, "funButton", "Fun");
            var gameButton = new DiscordButtonComponent(ButtonStyle.Success, "gameButton", "Games");
            var modButton = new DiscordButtonComponent(ButtonStyle.Success, "modButton", "Mod");

            var helpMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Aquamarine)
                .WithTitle("Command Menü")
                .WithDescription("Klicke auf einen Button um die Commands der jeweiligen Kategorien zu sehen")
                )
                .AddComponents(funButton, gameButton, modButton);

            await ctx.Channel.SendMessageAsync(helpMessage);
        }

        [SlashCommand("server", "Zeigt Informationen zum Server an")]
        [Aliases("info", "serverinfo")]
        public async Task ServerEmbed(InteractionContext ctx)
        {
            string serverDescription = $"🏷\n** Servername:** {ctx.Guild.Name}\n" +
                                        $"**Server ID:** {ctx.Guild.Id}\n" +
                                        $"**Erstellt am:** {ctx.Guild.CreationTimestamp:dd/M/yyyy}\n" +
                                        $"**Owner:** {ctx.Guild.Owner.Mention}\n\n" +
                                        $"**Users:** {ctx.Guild.MemberCount}\n" +
                                        $"**Channels:** {ctx.Guild.Channels.Count}\n" +
                                        $"**Rollen:** {ctx.Guild.Roles.Count}\n" +
                                        $"**Emojis: ** {ctx.Guild.Emojis.Count}";

            var serverInformation = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Title = "Server Informationen",
                Description = serverDescription
            };

            await ctx.Channel.SendMessageAsync(serverInformation.WithImageUrl(ctx.Guild.IconUrl));
        }

        [SlashCommand("avatar", "Zeigt die Avatar-URL eines Users an")]
        [Aliases("profilbild")]
        public async Task AvatarCommand(InteractionContext ctx, [Option("user", "Der User, dessen Avatar angezeigt werden soll")] DiscordUser user = null)
        {
            var targetUser = user ?? ctx.User;

            var avatarUrl = targetUser.AvatarUrl;

            var embed = new DiscordEmbedBuilder
            {
                Title = $"{targetUser.Username}'s Avatar",
                ImageUrl = avatarUrl,
                Color = DiscordColor.HotPink,
                Description = ctx.User.AvatarUrl,
            };

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed.Build()));
        }
    }
}