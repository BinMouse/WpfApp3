using System;
using System.IO;
using System.Text.Json;
using WpfApp3.Models;

namespace WpfApp3
{
    /// <summary>
    /// Отвечает за работу с настройками
    /// </summary>
    class SettingsManager
    {
        public static GameSettings settings { get; private set; }

        /// <summary>
        /// Загрузка настроек
        /// </summary>
        public static void LoadSettings()
        {
            if (!File.Exists("settings.json"))
                SetDefault();
            else
            {
                String json = File.ReadAllText("settings.json");
                settings = JsonSerializer.Deserialize<GameSettings>(json);
            }
        }

        /// <summary>
        /// Сохранение настроек
        /// </summary>
        public static void SaveSettings()
        {
            String json = JsonSerializer.Serialize(settings);
            File.WriteAllText("settings.json", json);
        }

        /// <summary>
        /// Выставляет настройки по умолчанию
        /// </summary>
        public static void SetDefault() 
        {
            settings = new GameSettings();
            SaveSettings();
        }
    }
}
