using System;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using ReplyBot.Commands;
using System.Collections.Generic;

namespace ReplyBot
{
    public class Bot {
        public static List<User> Users = new List<User>();
        private string[]? WordsList = Program.LoadWords();
        private DiscordClient client = new DiscordClient(new DiscordConfiguration() 
        {
            Token = "OTQ4MDY0NDIyOTUyMzA0Njkw.Yh2XzA.WDMMU-ukL_zQ-u7Oy6h8pvXJdHI", 
            MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged
        });
        public async Task init() {
            CommandsNextConfiguration commandsConfig = new CommandsNextConfiguration() {
                CaseSensitive = false,
                EnableDms = false,
                StringPrefixes = new string[] {"b!", "B!"}
            };

            CommandsNextExtension commands = client.UseCommandsNext(commandsConfig);
            commands.RegisterCommands<CounterCheck>();

            client.MessageCreated += CheckMessageContents;
            client.GuildDownloadCompleted += CollectUsers;

            await client.ConnectAsync();

            await Task.Delay(-1);
        }
        public async Task CollectUsers(object sender, GuildDownloadCompletedEventArgs e) {
            await client.UpdateStatusAsync(new DiscordActivity("naught words | b!", ActivityType.ListeningTo), UserStatus.Online);


            IReadOnlyDictionary<ulong, DiscordGuild> guilds = client.Guilds;

            foreach (KeyValuePair<ulong, DiscordGuild> guild in guilds) {
                foreach (DiscordMember user in await guild.Value.GetAllMembersAsync()) {
                    Users.Add(new User { Name = user.Username, BitchCount = 0});
                }
            }
        }
        private async Task CheckMessageContents(object sender, MessageCreateEventArgs e) {
            foreach (string? word in WordsList) {
                if (e.Message.Content.ToLower().Contains(word)) {
                    User? userToIterate = Users.Find((x) => {
                        if (x.Name.Equals(e.Author.Username)) {
                            return true;
                        } else return false;
                    });

                    if (userToIterate != null) {
                        userToIterate.BitchCount += 1;
                    } else await e.Message.RespondAsync($"User {e.Author.Username} not found.");
                }   
            }
        }
    }
}