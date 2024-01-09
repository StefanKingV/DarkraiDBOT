using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.SlashCommands;
using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.Interactivity.Extensions;

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

        [SlashCommand("virus", "Sende der Person einen Virus")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Virus(InteractionContext ctx,
            [Option("user", "Ziel")] DiscordUser user)
        {
            await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                     .WithContent(("Der Virus wird installiert...")).AsEphemeral(true));

            await ctx.Channel.SendMessageAsync(user.Locale);
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

        [SlashCommand("Spritrechner", "Rechne den Sprit aus")]
        public async Task Spritrechner(InteractionContext ctx,
                                       [Option("Kilometer", "Gib die gefahrene Strecke in km ein")] double kilometres,
                                       [Option("Durchschnittsverbrauch", "Gib den Durschnittsverbrauch in l/100km ein")] double avgConsumption,
                                       [Option("Spritpreis", "Gib den Spritpreis pro Liter ein")] double fuelPrice,
                                       [Option("Mitfahrer", "Gib die Anzahl der Mitfahrer ein")] int passenger,
                                       [Option("Rückfahrt?", "Soll die Rückfahrt berücksichtigt werden?")] bool returnJourney = false,
                                       [Option("Mautgebühren", "Gib die Mautgebühren ein")] double maut = 0)
        {
            double price = ((kilometres * avgConsumption / 100) * fuelPrice);
            if (returnJourney == true)
            {
                price *= 2;
            }
            double pricePerPassenger = price * (passenger + 1) + (maut / passenger);
           
            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "**Spritrechner Auswertung**\n",
                Description = $"Kilometer insgesamt: {kilometres}\n" +
                              $"Durchschnittsverbrauch: {avgConsumption} Liter pro 100km\n" +
                              $"Spritpreis pro Liter: {fuelPrice}€\n" +
                              $"Anzahl der Mitfahrer: {passenger}\n" +
                              $"Rückfahrt berechnet?: {returnJourney}\n" +
                              $"Mautgebühren: {maut}€\n\n" +
                              $"Der **Gesamtpreis** beträgt **{price}** Euro.\n" +
                              $"Der Preis **pro Person** beträgt **{pricePerPassenger }** Euro.",
                Color = DiscordColor.Yellow
            };

            await ctx.Channel.SendMessageAsync(embedMessage);
        }
    }
}
