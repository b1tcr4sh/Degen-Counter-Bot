using System;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReplyBot
{
    public static class Program {
        public static async Task Main(string[] args) {
            Bot bot = new Bot();

            await bot.init();
        }
        public static string[]? LoadWords() {
            string rawJson = File.ReadAllText("./words.json");

            string[]? words = JsonSerializer.Deserialize<string[]>(rawJson);
            return words;
        }
    }
}