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
        [SlashCommand("Ticket", "Ticketsystem")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Ticket(InteractionContext ctx,
                            [Option("Preis", "Preis des Gewinnspiels", autocomplete: false)] string giveawayPrize,
                            [Option("Beschreibung", "Beschreibung", autocomplete: false)] string giveawayDescription,
                            [Option("Gewinner", "Anzahl der Gewinner", autocomplete: false)] double amountWinner,
                            [Option("Dauer", "Länge des Gewinnspiels in Sekunden", autocomplete: false)] long giveawayTime)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("_Neues Gewinnspiel gestartet..._"));

            var entryButton = new DiscordButtonComponent(ButtonStyle.Primary, "entryGiveawayButton", "\uD83C\uDF89");

            DateTimeOffset endTime = DateTimeOffset.UtcNow.AddSeconds(giveawayTime);
            int totalEntries = 1;
            var interactivity = Bot.Client.GetInteractivity();
            string giveawayWinner = ctx.User.Mention;

            var giveawayMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()


                .WithColor(DiscordColor.Rose)
                .WithTitle("**" + giveawayPrize + "** :gift:")
                .WithDescription(giveawayDescription +
                                $"\n\n" +
                                $":tada: Gewinner: {amountWinner}\n" +
                                $":man_standing: Teilnehmer: **{totalEntries}**\n" +
                                $"\nGewinnspiel Ende: <t:{endTime.ToUnixTimeSeconds()}:R>\n" +
                                $"Gehosted von: {ctx.User.Mention}\n")
                )
                .AddComponents(entryButton);

            var sendGiveaway = await ctx.Channel.SendMessageAsync(giveawayMessage);

            for (int i = 0; i <= amountWinner; i++)
            {

            }

            await interactivity.WaitForButtonAsync(sendGiveaway, TimeSpan.FromSeconds(giveawayTime));

            string giveawayResultDescription = $":man_standing: Teilnehmer: {totalEntries}\n" +
                                               $":tada: Preis: **{giveawayPrize}**\n" +
                                               $"\n:crown: **Gewinner:** {giveawayWinner}";

            var giveawayResultEmbed = new DiscordEmbedBuilder
            {
                Title = "Gewinnspiel Ende",
                Description = giveawayResultDescription,
                Color = DiscordColor.Green
            };

            await ctx.Channel.SendMessageAsync(embed: giveawayResultEmbed);
        }
    }
}