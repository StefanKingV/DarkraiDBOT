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
    public class TestSL : ApplicationCommandModule
    {
        [SlashCommand("test", "Das ist ein Test")]
        public async Task TestSlash(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Starte Slash Command...."));

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "Test"
            };

            await ctx.Channel.SendMessageAsync(embedMessage);
        }

        [SlashCommand("button", "Erstelle einen Button im Chat")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator, true)]
        public async Task Button(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("**Button**"));

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

    }
}
