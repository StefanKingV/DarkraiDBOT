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

    }
}
