﻿using DSharpPlus.CommandsNext.Attributes;
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
using System.Threading;

namespace DarkBot.Slash_Commands
{
    public class FunSL : ApplicationCommandModule
    {
        [SlashCommand("pingspam", "Pingt die Person nach beliebiger Anzahl")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task PingSpam(InteractionContext ctx,
                            [Option("user", "User, der gepingt werden soll", autocomplete: false)] DiscordUser user,
                            [Option("anzahl", "Anzahl, wie oft der User gepingt werden soll", autocomplete: false)] double anzahl)
        {
            await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                     .WithContent(($"PingSpam an {user.Mention} wird durchgeführt. Dies kann einige Zeit in Anspruch nehmen")).AsEphemeral(true));
            
            for (int i = 0; i < anzahl; i++)
            {
                await ctx.Channel.SendMessageAsync(user.Mention);
            }
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


        [SlashCommand("valorant", "Valorant Statistiken")]
        public async Task Valorant(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Hier werden bald deine Valorant Statistiken angezeigt"));

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "... coming soon"
            };

            // Riot API: RGAPI-3e72ae2c-69a7-4ab8-8d51-97ce23d5ee43
            await ctx.Channel.SendMessageAsync(embedMessage);
        }

        [SlashCommand("hund", "Random Hunde Bild")]
        public async Task Hund(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Hunde Bild"));

            var dog = "http://random.dog/" + await SearchHelper.GetResponseStringAsync("https://random.dog/woof").ConfigureAwait(false);
            var embed = new DiscordEmbedBuilder().WithImageUrl(dog).WithTitle("so ein Feini").WithUrl(dog);
            await ctx.Channel.SendMessageAsync(embed.Build());
        }
    }
}
