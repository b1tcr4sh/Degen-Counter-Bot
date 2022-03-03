using System;
using System.Threading.Tasks;
using System.Linq;
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

            List<User> users = Bot.Users;
            List<User> orderedUsers = users.OrderBy(element => element.BitchCount).ToList<User>();
            orderedUsers.Reverse();

            embedBuilder.Title = "Bitch Counters";
            foreach (User user in orderedUsers) {
                DiscordUser? DiscordUser = Bot.DiscordUsers.Find(x => {
                    return x.Username == user.Name;
                });

                if (user.BitchCount != 0 && DiscordUser.IsBot == false && embedBuilder.Fields.Count() <= 10)
                    
                    embedBuilder.AddField(user.Name, user.BitchCount.ToString());
            }

            if (Bot.listReset) {
                embedBuilder.AddField("Notice:", "The bot has restarted recently, and the list is reset.");
            }

            if (embedBuilder.Fields.Count == 0) {
                embedBuilder.Color = DiscordColor.Red;
                embedBuilder.AddField("Nobody has said any naughy words yet.", String.Empty);
            }
            DiscordEmbed embed = embedBuilder.Build();

            await context.Channel.SendMessageAsync(embed);  
            Bot.listReset = false;         
        }
    }
}