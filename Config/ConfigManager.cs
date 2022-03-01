using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReplyBot.Config {
    public class ConfigManager {
        public ConfigFile config { get; private set; }
        public ConfigManager(string @path) {
            string rawText = File.ReadAllText(path);

            config = JsonSerializer.Deserialize<ConfigFile>(rawText);
        }
    }
}