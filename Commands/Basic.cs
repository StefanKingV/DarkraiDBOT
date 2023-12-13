﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBotTemplate.Commands
{
    public class Basic : BaseCommandModule
    {
        [Command("test")]
        public async Task Test(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Test");
        }
    }
}
