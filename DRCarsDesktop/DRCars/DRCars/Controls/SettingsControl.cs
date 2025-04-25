using System;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Utils;

namespace DRCars.Controls
{
    public class SettingsControl : UserControl
    {
        private RoundedPanel apiSettingsPanel;
        private RoundedPanel appSettingsPanel;
        private Label apiUrlLabel;
        private RoundedTextBox apiUrlTextBox;
        private RoundedButton saveApiSettingsButton;
        private RoundedButton testConnectionButton;
        private Label appSettingsLabel;
        private Label apiSettingsLabel;
        private CheckBox darkModeCheckBox;
        private Label themeLabel;
        private RoundedButton saveAppSettingsButton;

        public SettingsControl()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void InitializeComponent()
        {
            apiSettingsPanel = new RoundedPanel();
            appSettingsPanel = new RoundedPanel();
            apiUrlLabel = new Label();
            apiUrlTextBox = new RoundedTextBox();
            saveApiSettingsButton = new RoundedButton();
            testConnectionButton = new RoundedButton();
            appSettingsLabel = new Label();
            apiSettingsLabel = new Label();
            darkModeCheckBox = new CheckBox();
            themeLabel = new Label();
            saveAppSettingsButton = new RoundedButton();

            // API Settings Panel
            apiSettingsPanel.BorderRadius = 15;
            apiSettingsPanel.BorderColor = Color.FromArgb(220, 220, 220);
            apiSettingsPanel.BorderSize = 1;
            apiSettingsPanel.Dock = DockStyle.Top;
            apiSettingsPanel.Height = 200;
            apiSettingsPanel.Padding = new Padding(20);
            apiSettingsPanel.Margin = new Padding(0, 0, 0, 20);

            // API Settings Label
            apiSettingsLabel.AutoSize = true;
            apiSettingsLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            apiSettingsLabel.Location = new Point(20, 20);
            apiSettingsLabel.Text = "Configuración de API";

            // API URL Label
            apiUrlLabel.AutoSize = true;
            apiUrlLabel.Font = new Font("Segoe UI", 9F);
            apiUrlLabel.Location = new Point(20, 60);
            apiUrlLabel.Text = "URL de la API";

            // API URL TextBox
            apiUrlTextBox.Size = new Size(400, 40);
            apiUrlTextBox.Location = new Point(20, 85);
            apiUrlTextBox.BorderRadius = 10;
            apiUrlTextBox.PlaceholderText = "Ingrese la URL de la API";

            // Save API Settings Button
            saveApiSettingsButton.Text = "Guardar";
            saveApiSettingsButton.Size = new Size(120, 40);
            saveApiSettingsButton.Location = new Point(20, 140);
            saveApiSettingsButton.BorderRadius = 10;
            saveApiSettingsButton.BackColor = Color.FromArgb(0, 120, 215);
            saveApiSettingsButton.ForeColor = Color.White;
            saveApiSettingsButton.Click += SaveApiSettingsButton_Click;

            // Test Connection Button
            testConnectionButton.Text = "Probar Conexión";
            testConnectionButton.Size = new Size(150, 40);
            testConnectionButton.Location = new Point(150, 140);
            testConnectionButton.BorderRadius = 10;
            testConnectionButton.BackColor = Color.FromArgb(50, 50, 50);
            testConnectionButton.ForeColor = Color.White;
            testConnectionButton.Click += TestConnectionButton_Click;

            // App Settings Panel
            appSettingsPanel.BorderRadius = 15;
            appSettingsPanel.BorderColor = Color.FromArgb(220, 220, 220);
            appSettingsPanel.BorderSize = 1;
            appSettingsPanel.Dock = DockStyle.Top;
            appSettingsPanel.Height = 200;
            appSettingsPanel.Padding = new Padding(20);
            appSettingsPanel.Margin = new Padding(0, 0, 0, 20);

            // App Settings Label
            appSettingsLabel.AutoSize = true;
            appSettingsLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            appSettingsLabel.Location = new Point(20, 20);
            appSettingsLabel.Text = "Configuración de la Aplicación";

            // Theme Label
            themeLabel.AutoSize = true;
            themeLabel.Font = new Font("Segoe UI", 9F);
            themeLabel.Location = new Point(20, 60);
            themeLabel.Text = "Tema";

            // Dark Mode CheckBox
            darkModeCheckBox.AutoSize = true;
            darkModeCheckBox.Font = new Font("Segoe UI", 9F);
            darkModeCheckBox.Location = new Point(20, 85);
            darkModeCheckBox.Text = "Modo Oscuro";

            // Save App Settings Button
            saveAppSettingsButton.Text = "Guardar";
            saveAppSettingsButton.Size = new Size(120, 40);
            saveAppSettingsButton.Location = new Point(20, 140);
            saveAppSettingsButton.BorderRadius = 10;
            saveAppSettingsButton.BackColor = Color.FromArgb(0, 120, 215);
            saveAppSettingsButton.ForeColor = Color.White;
            saveAppSettingsButton.Click += SaveAppSettingsButton_Click;

            // Add controls to panels
            apiSettingsPanel.Controls.Add(apiSettingsLabel);
            apiSettingsPanel.Controls.Add(apiUrlLabel);
            apiSettingsPanel.Controls.Add(apiUrlTextBox);
            apiSettingsPanel.Controls.Add(saveApiSettingsButton);
            apiSettingsPanel.Controls.Add(testConnectionButton);

            appSettingsPanel.Controls.Add(appSettingsLabel);
            appSettingsPanel.Controls.Add(themeLabel);
            appSettingsPanel.Controls.Add(darkModeCheckBox);
            appSettingsPanel.Controls.Add(saveAppSettingsButton);

            // Add panels to control
            this.Controls.Add(appSettingsPanel);
            this.Controls.Add(apiSettingsPanel);

            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 245, 245);
        }

        private void LoadSettings()
        {
            // Load API URL from settings
            apiUrlTextBox.Texts = AppConfig.ApiBaseUrl;

            // Load other settings (placeholder for future implementation)
            darkModeCheckBox.Checked = false;
        }

        private void SaveApiSettingsButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Save API URL to settings
                AppConfig.Settings.ApiBaseUrl = apiUrlTextBox.Texts;
                AppConfig.SaveSettings();

                MessageBox.Show("Configuración de API guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la configuración: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void TestConnectionButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Test connection to API
                testConnectionButton.Enabled = false;
                testConnectionButton.Text = "Conectando...";

                // Create a new ApiClient with the current URL
                ApiClient apiClient = new ApiClient();

                // Try to get users to test connection
                await apiClient.GetUsersAsync();

                MessageBox.Show("Conexión exitosa a la API.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                testConnectionButton.Enabled = true;
                testConnectionButton.Text = "Probar Conexión";
            }
        }

        private void SaveAppSettingsButton_Click(object sender, EventArgs e)
        {
            // Save app settings (placeholder for future implementation)
            MessageBox.Show("Configuración de la aplicación guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
