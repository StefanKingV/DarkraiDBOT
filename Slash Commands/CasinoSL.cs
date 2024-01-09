using DSharpPlus.SlashCommands;
using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace DarkBot.Slash_Commands
{
	public class CasinoSL : ApplicationCommandModule
	{
		[SlashCommand("glücksrad", "Dreh am Glücksrad!")]
		public async Task Gluecksrad(InteractionContext ctx)
		{

			int prize = 0;
			for (int i = 0; i < 60; i++)
			{
				//int prizeArray[] = { 0, 0, 0, 1, 1, 2, 2, 5, 5, 10, 10, 10, 10, 25, 25, 25, 50, 50, 100, 250, 500, 1000 };
				//std::random_device rd;
				//std::mt19937 gen(rd());
				//std::uniform_int_distribution<> dist(0, prizes.size() - 1);
				//prize = [dist(gen)];

				//await ctx.Channel.SendMessageAsync(prize);
				await Task.Delay(1000);
				//await ctx.Channel.DeleteMessagesAsync(1);
			}

			if (prize > 0)
			{
				await ctx.Channel.SendMessageAsync($"Glückwunsch! Du hast {prize}€ vom Glücksrad gewonnen :D");
				//m_balance += prize;
			}
			else
			{
				await ctx.Channel.SendMessageAsync($"Schade! Du hast nichts gewonnen :C");
			}
		}

		[SlashCommand("blackjack", "Spiel eine Runde Blackjack!")]
		public async Task Blackjack(InteractionContext ctx,
									[Option("Euro", "Wie viel möchtest du setzen?")] double bettingAmount)
		{

		}

		[SlashCommand("poker", "Spiel eine Runde Poker!")]
		public async Task Poker(InteractionContext ctx,
									[Option("Euro", "Wie viel möchtest du setzen?")] double bettingAmount)
		{

		}

		[SlashCommand("roulette", "Spiel eine Runde Roulette!")]
		public async Task Roulette(InteractionContext ctx,
									[Option("Euro", "Wie viel möchtest du setzen?")] double bettingAmount)
		{

		}

		[SlashCommand("slots", "Spiel eine Runde Slots!")]
		public async Task Slots(InteractionContext ctx,
									[Option("Euro", "Wie viel möchtest du setzen?")] double bettingAmount)
		{

		}

		[SlashCommand("zahlenraten", "Spiel eine Runde Zahlenraten!")]
		public async Task Zahlenraten(InteractionContext ctx,
									[Option("Euro", "Wie viel möchtest du setzen?")] double bettingAmount)
		{
			Random r = new Random();
			int dice = r.Next(0, 100);
			int tries = 5;

			await ctx.Channel.SendMessageAsync("Spielregeln\n\n" +
											   "1. Du hast 5 Versuche, um eine zufällige Zahl zwischen 1 und 100 zu erraten.\n" +
											   "2. Der Gewinn ist das 10x fache von deinem Wetteinsatz €€€.");


			for (int i = 0; tries > 0; i++)
			{
				await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
					new DiscordInteractionResponseBuilder().WithContent("Errate die richtige Zahl zwischen 1 und 100"));
				int guess = await ctx.Channel.WaitForMessageAsync(
				if (guess <= 0 || guess > 100)
				{
					await ctx.Channel.SendMessageAsync("\nDie Zahl muss zwischen 1 und 100 sein\n" +
													   "Versuche es erneut");
					continue;
				}

				if (guess > dice)
				{
					await ctx.Channel.SendMessageAsync("\nDie gesuchte Zahl ist kleiner\n");
					tries--;
				}
				else if (guess < dice)
				{
					await ctx.Channel.SendMessageAsync("\nDie gesuchte Zahl ist größer\n");
					tries--;
				}
				else
					break;
				await ctx.Channel.SendMessageAsync($"Du hast {tries} Versuche übrig\n");
			}
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
