using DarkBot.Config;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Google.Apis.Services;
using System.Threading.Tasks;

namespace DarkBot.Slash_Commands
{
    public class ImageSL : ApplicationCommandModule
    {
        [SlashCommand("googlebild", "Google Bild Suche")]
        public async Task GoogleImageSearch(InteractionContext ctx,
                                            [Option("suche", "Nach welchem Bild möchtest du suchen ? ")] string search)
        {
            //var configJsonFile = new JSONReader();
            //await configJsonFile.ReadJSON();
            //
            //var customGoogleSearch = new CustomsearchService(new BaseClientService.Initializer()
            //{
            //    ApiKey = configJsonFile.googleapikey,
            //    ApplicationName = "Test"
            //});

            await ctx.CreateResponseAsync("Test");

        }

        [SlashCommand("hund", "Generiere ein zufälliges Bild von einem Hund")]
        public async Task Hund(InteractionContext ctx)
        {
            var dog = "http://random.dog/" + await SearchHelper.GetResponseStringAsync("https://random.dog/woof").ConfigureAwait(false);
            var embed = new DiscordEmbedBuilder().WithImageUrl(dog).WithTitle("so ein Feini").WithUrl(dog);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed.Build()));

        }
    }
}
