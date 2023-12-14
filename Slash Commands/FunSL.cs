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
using System.Security.Cryptography;
using System.Runtime.Remoting.Contexts;
using Newtonsoft.Json;
using Emzi0767;
using DSharpPlus.Interactivity.Extensions;

namespace DiscordBotTemplate.Slash_Commands
{
    public class FunSL : ApplicationCommandModule
    {
        [SlashCommand("test", "Das ist ein Test")]
        public async Task TestSlash(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()                                                                          .WithContent("Starte Slash Command...."));

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "Test"
            };

            await ctx.Channel.SendMessageAsync(embedMessage);
        }

        [SlashCommand("clear", "Lösche Nachrichten aus dem Chat")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Clear(InteractionContext ctx, [Option("Anzahl", "Anzahl der Nachrichten die gelöscht werden sollen", autocomplete: false)] double delNumber)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Die letzten {delNumber} Nachrichten wurden erfolgreich gelöscht!"));

            var channel = ctx.Channel;
            var items = await channel.GetMessagesAsync((int)(delNumber + 1));
            await channel.DeleteMessagesAsync(items);

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
                Color = DiscordColor.HotPink
            };

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed.Build()));
        }

        [SlashCommand("pingspam", "Pingt die Person nach beliebiger Anzahl")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task PingSpam(InteractionContext ctx,
                            [Option("anzahl", "Anzahl, wie oft der User gepingt werden soll", autocomplete: false)] double anzahl,
                            [Option("user", "User, der gepingt werden soll", autocomplete: false)] DiscordUser user)
        {
            for (int i = 0; i < anzahl; i++)
            {
                await ctx.Channel.SendMessageAsync(user.Mention);
            }
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

        [SlashCommand("poll", "Starte eine Abstimmung")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Poll(InteractionContext ctx,
                            [Option("polltitel", "Titel der Abstimmung", autocomplete: false)] string pollTitle,
                            [Option("pollzeit", "Länge der Abstimmung in Sekunden", autocomplete: false)] long pollTime,
                            [Option("option1", "Option 1", autocomplete: false)] string option1,
                            [Option("option2", "Option 2", autocomplete: false)] string option2,
                            [Option("option3", "Option 3", autocomplete: false)] string option3)
        {
            var interactivity = Bot.Client.GetInteractivity();
            DateTimeOffset endTime = DateTimeOffset.UtcNow.AddSeconds(pollTime);

            DiscordEmoji[] emojiOptions = { DiscordEmoji.FromName(Bot.Client, ":one:"),
                                    DiscordEmoji.FromName(Bot.Client, ":two:"),
                                    DiscordEmoji.FromName(Bot.Client, ":three:") };

            string optionsDescription = $"{emojiOptions[0]} | {option1} \n" +
                                        $"{emojiOptions[1]} | {option2} \n" +
                                        $"{emojiOptions[2]} | {option3} \n" +
                                        $"\nEnde der Abstimmung: <t:{endTime.ToUnixTimeSeconds()}:R>";

            var pollMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Title = pollTitle,
                Description = optionsDescription
            };

            var sendPoll = await ctx.Channel.SendMessageAsync(embed: pollMessage);

            foreach (var emoji in emojiOptions)
            {
                await sendPoll.CreateReactionAsync(emoji);
            }

            var pollReactions = await interactivity.CollectReactionsAsync(sendPoll, TimeSpan.FromSeconds(pollTime));

            int count1 = 0;
            int count2 = 0;
            int count3 = 0;

            foreach (var emoji in pollReactions)
            {
                if (emoji.Emoji == emojiOptions[0])
                {
                    count1++;
                }
                if (emoji.Emoji == emojiOptions[1])
                {
                    count2++;
                }
                if (emoji.Emoji == emojiOptions[2])
                {
                    count3++;
                }
            }

            int pollVotes = count1 + count2 + count3;

            string pollResultDescription = $"{emojiOptions[0]}: {count1} Votes \n" +
                                            $"{emojiOptions[1]}: {count2} Votes \n" +
                                            $"{emojiOptions[2]}: {count3} Votes \n" +
                                            $"\nIngesamte Votes: {pollVotes}";

            var pollResultEmbed = new DiscordEmbedBuilder
            {
                Title = "Ergebnis der Abstimmung",
                Description = pollResultDescription,
                Color = DiscordColor.Green
            };

            await ctx.Channel.SendMessageAsync(embed: pollResultEmbed);
        }

        [SlashCommand("button", "Erstelle einen Button im Chat")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Button(InteractionContext ctx)
        {
            DiscordButtonComponent button1 = new DiscordButtonComponent(ButtonStyle.Primary, "1", "Button 1");
            DiscordButtonComponent button2 = new DiscordButtonComponent(ButtonStyle.Primary, "2", "Button 2");

            var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Azure)
                .WithTitle("Nachricht mit Buttons")
                .WithDescription("Wähle einen Button")
                )
                .AddComponents(button1)
                .AddComponents(button2);

                await ctx.Channel.SendMessageAsync(message);
        }

        [SlashCommand("giveaway", "Starte ein Gewinnspiel")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Giveaway(InteractionContext ctx,
                            [Option("Preis", "Preis des Gewinnspiels", autocomplete: false)] string giveawayPrize,
                            [Option("Beschreibung", "Beschreibung", autocomplete: false)] string giveawayDescription,
                            [Option("Gewinner", "Anzahl der Gewinner", autocomplete: false)] int amountWinner,
                            [Option("Dauer", "Länge des Gewinnspiels in Sekunden", autocomplete: false)] long giveawayTime)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("**Gewinnspiel**"));

            DiscordButtonComponent entryButton = new DiscordButtonComponent(ButtonStyle.Primary, "EntryGiveaway", ":tada:");
            DateTimeOffset endTime = DateTimeOffset.UtcNow.AddSeconds(giveawayTime);
            int totalEntries = 1;
            string giveawayWinner = "Hans";

            var giveawayMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()


                .WithColor(DiscordColor.Sienna)
                .WithTitle(":gift:" + giveawayPrize)
                .WithDescription(giveawayDescription + 
                                $"\n" +
                                $"Gewinner: {amountWinner}\n" +
                                $"Teilnehmer: **{totalEntries}**\n" +
                                $"Endet: {endTime}\n" +
                                $"Gehosted von: {ctx.User.Mention}\n")
                )
                .AddComponents(entryButton);

            await ctx.Channel.SendMessageAsync(giveawayMessage);

            for (int i =  0; i <= amountWinner; i++)
            {
                
            }

            string giveawayResultDescription = $"Teilnehmer: {totalEntries}\n" +
                                               $"Preis: {giveawayPrize}\n" +
                                               $"\nGewinner: {giveawayWinner}";

            var giveawayResultEmbed = new DiscordEmbedBuilder
            {
                Title = "Gewinnspiel",
                Description = giveawayResultDescription,
                Color = DiscordColor.Green
            };

            await ctx.Channel.SendMessageAsync(embed: giveawayResultEmbed);
        }
    }
}
