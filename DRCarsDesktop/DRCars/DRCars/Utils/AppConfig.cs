using System;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace DRCars.Utils
{
    public static class AppConfig
    {
        private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        private static AppSettings _settings;

        public static string ApiBaseUrl => Settings.ApiBaseUrl;

        public static AppSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    LoadSettings();
                }
                return _settings;
            }
        }

        private static void LoadSettings()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    string json = File.ReadAllText(ConfigFilePath);
                    _settings = JsonConvert.DeserializeObject<AppSettings>(json);
                }
                else
                {
                    _settings = new AppSettings
                    {
                        ApiBaseUrl = "https://helped-bug-stirring.ngrok-free.app"
                    };
                    SaveSettings();
                }
            }
            catch (Exception ex)
            {
                // Fallback to default settings
                _settings = new AppSettings
                {
                    ApiBaseUrl = "https://helped-bug-stirring.ngrok-free.app"
                };
            }
        }

        public static void SaveSettings()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_settings, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(ConfigFilePath, json);
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }
    }

    public class AppSettings
    {
        public string ApiBaseUrl { get; set; }
    }
}