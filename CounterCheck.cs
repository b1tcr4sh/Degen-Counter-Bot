using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace ReplyBot.Commands {
    public class CounterCheck : BaseCommandModule {
        [Command("check")]
        [Aliases(new string[] { "count", "top", "leaderboard" })]
        [Description("Returns a list of the top 10 users on the leaderboard.")]
        public async Task CheckCounters(CommandContext context) {
            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();

            

            embedBuilder.Title = "Bitch Counters";
            foreach (User user in Bot.Users) {
                DiscordUser? DiscordUser = Bot.DiscordUsers.Find(x => {
                    return x.Username == user.Name;
                });

                if (user.BitchCount != 0 || DiscordUser.IsBot == false)
                    embedBuilder.AddField(user.Name, user.BitchCount.ToString(), true);
            }

            if (embedBuilder.Fields.Count == 0) {
                embedBuilder.Color = DiscordColor.Red;
                embedBuilder.WithFooter("Nobody has said any naughy words yet.");
            }
            DiscordEmbed embed = embedBuilder.Build();

            await context.Channel.SendMessageAsync(embed);           
        }
    }
}