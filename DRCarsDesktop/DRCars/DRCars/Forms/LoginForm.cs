using System;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Controls;
using DRCars.Models;

namespace DRCars.Forms
{
    public partial class LoginForm : Form
    {
        private RoundedPanel loginPanel;
        private Label titleLabel;
        private Label emailLabel;
        private RoundedTextBox emailTextBox;
        private Label passwordLabel;
        private RoundedTextBox passwordTextBox;
        private RoundedButton loginButton;
        private CheckBox rememberCheckBox;
        private LinkLabel forgotPasswordLink;
        private Label statusLabel;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            loginPanel = new RoundedPanel();
            titleLabel = new Label();
            emailLabel = new Label();
            emailTextBox = new RoundedTextBox();
            passwordLabel = new Label();
            passwordTextBox = new RoundedTextBox();
            loginButton = new RoundedButton();
            rememberCheckBox = new CheckBox();
            forgotPasswordLink = new LinkLabel();
            statusLabel = new Label();

            // Form
            this.Text = "DRCars - Iniciar Sesión";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Login Panel
            loginPanel.BorderRadius = 15;
            loginPanel.BorderColor = Color.FromArgb(220, 220, 220);
            loginPanel.BorderSize = 1;
            loginPanel.Size = new Size(400, 400);
            loginPanel.Location = new Point((this.ClientSize.Width - 400) / 2, (this.ClientSize.Height - 400) / 2);
            loginPanel.BackColor = Color.White;
            loginPanel.Padding = new Padding(30);

            // Title Label
            titleLabel.AutoSize = false;
            titleLabel.Size = new Size(340, 40);
            titleLabel.Location = new Point(30, 30);
            titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Text = "Bienvenido a DRCars";

            // Email Label
            emailLabel.AutoSize = true;
            emailLabel.Location = new Point(30, 90);
            emailLabel.Font = new Font("Segoe UI", 9F);
            emailLabel.Text = "Email";

            // Email TextBox
            emailTextBox.Size = new Size(340, 40);
            emailTextBox.Location = new Point(30, 115);
            emailTextBox.BorderRadius = 10;
            emailTextBox.PlaceholderText = "tu@email.com";

            // Password Label
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(30, 170);
            passwordLabel.Font = new Font("Segoe UI", 9F);
            passwordLabel.Text = "Contraseña";

            // Password TextBox
            passwordTextBox.Size = new Size(340, 40);
            passwordTextBox.Location = new Point(30, 195);
            passwordTextBox.BorderRadius = 10;
            passwordTextBox.PlaceholderText = "••••••••";
            passwordTextBox.PasswordChar = true;

            // Remember CheckBox
            rememberCheckBox.AutoSize = true;
            rememberCheckBox.Location = new Point(30, 250);
            rememberCheckBox.Font = new Font("Segoe UI", 9F);
            rememberCheckBox.Text = "Recordarme";

            // Forgot Password Link
            forgotPasswordLink.AutoSize = true;
            forgotPasswordLink.Location = new Point(230, 250);
            forgotPasswordLink.Font = new Font("Segoe UI", 9F);
            forgotPasswordLink.Text = "¿Olvidaste tu contraseña?";
            forgotPasswordLink.LinkColor = Color.FromArgb(0, 120, 215);
            forgotPasswordLink.Click += ForgotPasswordLink_Click;

            // Login Button
            loginButton.Text = "Iniciar Sesión";
            loginButton.Size = new Size(340, 45);
            loginButton.Location = new Point(30, 290);
            loginButton.BorderRadius = 10;
            loginButton.BackColor = Color.Black;
            loginButton.ForeColor = Color.White;
            loginButton.Click += LoginButton_Click;

            // Status Label
            statusLabel.AutoSize = false;
            statusLabel.Size = new Size(340, 30);
            statusLabel.Location = new Point(30, 345);
            statusLabel.Font = new Font("Segoe UI", 9F);
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            statusLabel.ForeColor = Color.Red;
            statusLabel.Visible = false;

            // Add controls to panel
            loginPanel.Controls.Add(titleLabel);
            loginPanel.Controls.Add(emailLabel);
            loginPanel.Controls.Add(emailTextBox);
            loginPanel.Controls.Add(passwordLabel);
            loginPanel.Controls.Add(passwordTextBox);
            loginPanel.Controls.Add(rememberCheckBox);
            loginPanel.Controls.Add(forgotPasswordLink);
            loginPanel.Controls.Add(loginButton);
            loginPanel.Controls.Add(statusLabel);

            // Add panel to form
            this.Controls.Add(loginPanel);
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string email = emailTextBox.Texts;
            string password = passwordTextBox.Texts;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                statusLabel.Text = "Por favor, ingrese email y contraseña.";
                statusLabel.Visible = true;
                return;
            }

            // For demo purposes, allow any login
            // In a real application, you would validate against the API

            // Create a mock user for demo
            User user = new User
            {
                Id = 1,
                Name = "Usuario Demo",
                Email = email,
                Role = UserRole.Admin,
                IsActive = true,
                LastLogin = DateTime.Now
            };

            // Open main form and close login form
            MainForm mainForm = new MainForm(user);
            this.Hide();
            mainForm.ShowDialog();
            this.Close();
        }

        private void ForgotPasswordLink_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Funcionalidad de recuperación de contraseña no implementada en esta versión de demostración.",
                "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
