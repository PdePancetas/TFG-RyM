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

        public static string ApiBaseUrl => _configData?.ApiBaseUrl ?? "https://helped-bug-stirring.ngrok-free.app";
        public static string FirebaseUrl => _configData?.FirebaseUrl ?? "https://drcars-b7c5f-default-rtdb.firebaseio.com";
        public static string FirebaseStorageUrl => _configData?.FirebaseStorageUrl ?? "https://firebasestorage.googleapis.com/v0/b/drcars-b7c5f.appspot.com/o";
        public static string FirebaseApiKey => _configData?.FirebaseApiKey ?? "AIzaSyA9QwrGTolafzmvvVOTrivoSIww5W_3eBY";
        public static string AppName => _configData?.AppName ?? "DRCars";
        public static string AppVersion => _configData?.AppVersion ?? "1.0.0";
        public static string CompanyName => _configData?.CompanyName ?? "DR Cars Import";
        public static string LogoPath => _configData?.LogoPath ?? Path.Combine(Application.StartupPath, "Resources", "logo.png");
        public static string ThemeName => _configData?.ThemeName ?? "Default";
        public static bool EnableDarkMode => _configData?.EnableDarkMode ?? false;
        public static bool UseFirebase => _configData?.UseFirebase ?? true;
        public static bool EnableWebPCompression => _configData?.EnableWebPCompression ?? true;
        public static bool EnableSVGVectorization => _configData?.EnableSVGVectorization ?? true;

        // Firebase Authentication
        private static FirebaseAuthService _firebaseAuth;

        public static FirebaseAuthService FirebaseAuth => _firebaseAuth;

        public static void SetFirebaseAuth(FirebaseAuthService firebaseAuth)
        {
            _firebaseAuth = firebaseAuth;
        }

        public static string GetFirebaseAuthToken()
        {
            return _firebaseAuth?.CurrentToken;
        }

        public static bool IsFirebaseAuthenticated()
        {
            return _firebaseAuth?.IsAuthenticated ?? false;
        }

        // Colores de Odoo
        public static System.Drawing.Color PrimaryColor => System.Drawing.Color.FromArgb(0, 160, 157);
        public static System.Drawing.Color SecondaryColor => System.Drawing.Color.FromArgb(242, 242, 242);
        public static System.Drawing.Color TextColor => System.Drawing.Color.FromArgb(51, 51, 51);
        public static System.Drawing.Color AccentColor => System.Drawing.Color.FromArgb(108, 117, 125);

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
                    Console.WriteLine("✅ Configuración cargada desde archivo");
                }
                else
                {
                    _configData = new ConfigData();
                    SaveConfig();
                    Console.WriteLine("⚠️ Archivo de configuración no encontrado, creando uno nuevo");
                }

                // Log de configuración Firebase
                Console.WriteLine("=== CONFIGURACIÓN FIREBASE ===");
                Console.WriteLine($"Firebase URL: {FirebaseUrl}");
                Console.WriteLine($"Firebase Storage URL: {FirebaseStorageUrl}");
                Console.WriteLine($"Firebase API Key: {(FirebaseApiKey?.Length > 0 ? $"{FirebaseApiKey.Substring(0, 10)}..." : "NO CONFIGURADA")}");
                Console.WriteLine($"Usar Firebase: {UseFirebase}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading configuration: {ex.Message}");
                MessageBox.Show($"Error loading configuration: {ex.Message}", "Configuration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _configData = new ConfigData();
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
            public string FirebaseUrl { get; set; }
            public string FirebaseStorageUrl { get; set; }
            public string FirebaseApiKey { get; set; }
            public string AppName { get; set; }
            public string AppVersion { get; set; }
            public string CompanyName { get; set; }
            public string LogoPath { get; set; }
            public string ThemeName { get; set; }
            public bool EnableDarkMode { get; set; }
            public bool UseFirebase { get; set; }
            public bool EnableWebPCompression { get; set; }
            public bool EnableSVGVectorization { get; set; }

            public ConfigData()
            {
                ApiBaseUrl = "https://helped-bug-stirring.ngrok-free.app";
                FirebaseUrl = "https://drcars-b7c5f-default-rtdb.firebaseio.com";
                FirebaseStorageUrl = "https://firebasestorage.googleapis.com/v0/b/drcars-b7c5f.appspot.com/o";
                FirebaseApiKey = "AIzaSyA9QwrGTolafzmvvVOTrivoSIww5W_3eBY";
                AppName = "DRCars";
                AppVersion = "1.0.0";
                CompanyName = "DR Cars Import";
                LogoPath = Path.Combine(Application.StartupPath, "Resources", "logo.png");
                ThemeName = "Default";
                EnableDarkMode = false;
                UseFirebase = true;
                EnableWebPCompression = true;
                EnableSVGVectorization = true;
            }
        }
    }
}
