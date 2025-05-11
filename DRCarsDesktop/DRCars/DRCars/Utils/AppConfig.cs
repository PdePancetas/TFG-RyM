using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DRCars.Utils
{
    public static class AppConfig
    {
        private static string _configFilePath = Path.Combine(Application.StartupPath, "config.json");
        private static ConfigData _configData;

        public static string ApiBaseUrl => _configData?.ApiBaseUrl ?? "https://api.drcars.example.com";
        public static string AppName => _configData?.AppName ?? "DRCars";
        public static string AppVersion => _configData?.AppVersion ?? "1.0.0";
        public static string CompanyName => _configData?.CompanyName ?? "DR Cars Import";
        public static string LogoPath => _configData?.LogoPath ?? Path.Combine(Application.StartupPath, "Resources", "logo.png");
        public static string ThemeName => _configData?.ThemeName ?? "Default";
        public static bool EnableDarkMode => _configData?.EnableDarkMode ?? false;

        // Colores de Odoo
        public static System.Drawing.Color PrimaryColor => System.Drawing.Color.FromArgb(0, 160, 157); // Verde Odoo
        public static System.Drawing.Color SecondaryColor => System.Drawing.Color.FromArgb(242, 242, 242); // Gris claro
        public static System.Drawing.Color TextColor => System.Drawing.Color.FromArgb(51, 51, 51); // Texto oscuro
        public static System.Drawing.Color AccentColor => System.Drawing.Color.FromArgb(108, 117, 125); // Gris para detalles

        // Propiedad para acceder a la configuración
        public static ConfigData Settings => _configData;

        static AppConfig()
        {
            LoadConfig();
        }

        public static void LoadConfig()
        {
            try
            {
                if (File.Exists(_configFilePath))
                {
                    string json = File.ReadAllText(_configFilePath);
                    _configData = JsonConvert.DeserializeObject<ConfigData>(json);
                }
                else
                {
                    // Create default config
                    _configData = new ConfigData
                    {
                        ApiBaseUrl = "https://api.drcars.example.com",
                        AppName = "DRCars",
                        AppVersion = "1.0.0",
                        CompanyName = "DR Cars Import",
                        LogoPath = Path.Combine(Application.StartupPath, "Resources", "logo.png"),
                        ThemeName = "Default",
                        EnableDarkMode = false
                    };

                    SaveConfig();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading configuration: {ex.Message}", "Configuration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Use default values
                _configData = new ConfigData
                {
                    ApiBaseUrl = "https://api.drcars.example.com",
                    AppName = "DRCars",
                    AppVersion = "1.0.0",
                    CompanyName = "DR Cars Import",
                    LogoPath = Path.Combine(Application.StartupPath, "Resources", "logo.png"),
                    ThemeName = "Default",
                    EnableDarkMode = false
                };
            }
        }

        public static void SaveConfig()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_configData, Formatting.Indented);
                File.WriteAllText(_configFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving configuration: {ex.Message}", "Configuration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void SaveSettings()
        {
            SaveConfig();
        }

        public class ConfigData
        {
            public string ApiBaseUrl { get; set; }
            public string AppName { get; set; }
            public string AppVersion { get; set; }
            public string CompanyName { get; set; }
            public string LogoPath { get; set; }
            public string ThemeName { get; set; }
            public bool EnableDarkMode { get; set; }
        }
    }
}
