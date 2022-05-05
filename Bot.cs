using System;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using ReplyBot.Commands;
using ReplyBot.Config;

namespace ReplyBot
{
    public class Bot {
        public static List<User> Users = new List<User>();
        public static List<DiscordUser> DiscordUsers = new List<DiscordUser>();
        private string[]? WordsList;
        public ConfigFile config { get; private set; }
        private DiscordClient? client;
        public static bool listReset = true;
        public static DateTime DateTimestamp;
        public async Task init() {
            ConfigManager manager = new ConfigManager("config.json");
            config = manager.config;

            WordsList = Program.LoadWords(config.Word_List_File_Name);

            client = new DiscordClient(new DiscordConfiguration() 
            {
                Token = config.Token, 
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            });

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

            DateTimestamp = DateTime.Now;

            await Task.Delay(-1);
        }
        public async Task CollectUsers(object sender, GuildDownloadCompletedEventArgs e) {
            if (client == null) return;

            await client.UpdateStatusAsync(new DiscordActivity("naughty words | b!", ActivityType.ListeningTo), UserStatus.Online);



            IReadOnlyDictionary<ulong, DiscordGuild> guilds = client.Guilds;

            foreach (KeyValuePair<ulong, DiscordGuild> guild in guilds) {
                foreach (DiscordMember user in await guild.Value.GetAllMembersAsync()) {
                    Users.Add(new User { Name = user.Username, BitchCount = 0});
                    DiscordUsers.Add(user);
                }
            }
        }
        private async Task CheckMessageContents(object sender, MessageCreateEventArgs e) {
            if (WordsList == null) {
                Console.WriteLine("Was unable to load words file."); 
                return;
            } 
            foreach (string? word in WordsList) {
                if (e.Message.Content.ToLower().Contains(word)) {
                    User? userToIterate = Users.Find((x) => {
                        if (x.Name.Equals(e.Author.Username)) {
                            return true;
                        } else return false;
                    });

                    if (userToIterate is not null) {
                        userToIterate.BitchCount += 1;
                    } else await e.Message.RespondAsync($"User {e.Author.Username} not found.");
                }   
            }
        }
    }
}