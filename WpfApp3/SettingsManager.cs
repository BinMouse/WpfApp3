using System;
using System.IO;
using System.Text.Json;
using WpfApp3.Models;

namespace WpfApp3
{
    class SettingsManager
    {
        public static GameSettings settings { get; private set; }

        public static void LoadSettings()
        {
            if (!File.Exists("settings.json"))
                settings = new GameSettings(volume: 100);
            else 
            {
                String json = File.ReadAllText("settings.json");
                settings = JsonSerializer.Deserialize<GameSettings>(json);
            }
        }

        public static void SaveSettings()
        {
            String json = JsonSerializer.Serialize(settings);
            File.WriteAllText("settings.json", json);
        }

        public static void ApplySettings(GameSettings newSettings)
        {
            SettingsManager.settings = newSettings;
        }

        event EventHandler<GameSettings> onSettingsChanged;
    }
}
