using System;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Controls;
using DRCars.Models;
using System.Net.Http;
using DRCars.Utils;

namespace DRCars.Forms
{
    public partial class LoginForm : Form
    {
        private RoundedPanel loginPanel;
        private Label titleLabel;
        private Label subtitleLabel;
        private Label emailLabel;
        private RoundedTextBox emailTextBox;
        private Label passwordLabel;
        private RoundedTextBox passwordTextBox;
        private RoundedButton loginButton;
        private RoundedButton exitButton;
        private CheckBox rememberCheckBox;
        private LinkLabel forgotPasswordLink;
        private Label statusLabel;
        private PictureBox logoBox;

        // Colores de Odoo
        private Color primaryColor = Color.FromArgb(0, 160, 157); // Verde Odoo
        private Color secondaryColor = Color.FromArgb(242, 242, 242); // Gris claro
        private Color textColor = Color.FromArgb(51, 51, 51); // Texto oscuro
        private Color accentColor = Color.FromArgb(108, 117, 125); // Gris para detalles

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            loginPanel = new RoundedPanel();
            titleLabel = new Label();
            subtitleLabel = new Label();
            emailLabel = new Label();
            emailTextBox = new RoundedTextBox();
            passwordLabel = new Label();
            passwordTextBox = new RoundedTextBox();
            loginButton = new RoundedButton();
            exitButton = new RoundedButton();
            rememberCheckBox = new CheckBox();
            forgotPasswordLink = new LinkLabel();
            statusLabel = new Label();
            logoBox = new PictureBox();

            // Form
            this.Text = "DRCars - Iniciar Sesión";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = secondaryColor;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Logo Box
            /*logoBox.Size = new Size(200, 80);
            logoBox.Location = new Point(350, 00);
            logoBox.BackColor = Color.Transparent;
            logoBox.SizeMode = PictureBoxSizeMode.Zoom;*/
            // Set a placeholder logo or load from resources

            // Login Panel
            loginPanel.BorderRadius = 8;
            loginPanel.BorderColor = Color.FromArgb(230, 230, 230);
            loginPanel.BorderSize = 1;
            loginPanel.Size = new Size(400, 450);
            loginPanel.Location = new Point((this.ClientSize.Width - 400) / 2, 40);
            loginPanel.BackColor = Color.White;
            loginPanel.Padding = new Padding(30);

            // Title Label
            titleLabel.AutoSize = false;
            titleLabel.Size = new Size(340, 40);
            titleLabel.Location = new Point(30, 30);
            titleLabel.Font = new Font("Segoe UI Semibold", 18F);
            titleLabel.ForeColor = textColor;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Text = "Bienvenido a DRCars";

            // Subtitle Label
            subtitleLabel.AutoSize = false;
            subtitleLabel.Size = new Size(340, 30);
            subtitleLabel.Location = new Point(30, 70);
            subtitleLabel.Font = new Font("Segoe UI", 10F);
            subtitleLabel.ForeColor = accentColor;
            subtitleLabel.TextAlign = ContentAlignment.MiddleCenter;
            subtitleLabel.Text = "Inicie sesión para continuar";

            // Email Label
            emailLabel.AutoSize = true;
            emailLabel.Location = new Point(30, 120);
            emailLabel.Font = new Font("Segoe UI", 10F);
            emailLabel.ForeColor = textColor;
            emailLabel.Text = "Email";

            // Email TextBox
            emailTextBox.Size = new Size(340, 40);
            emailTextBox.Location = new Point(30, 145);
            emailTextBox.BorderRadius = 4;
            emailTextBox.PlaceholderText = "tu@email.com";
            emailTextBox.BorderColor = Color.FromArgb(206, 212, 218);

            // Password Label
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(30, 200);
            passwordLabel.Font = new Font("Segoe UI", 10F);
            passwordLabel.ForeColor = textColor;
            passwordLabel.Text = "Contraseña";

            // Password TextBox
            passwordTextBox.Size = new Size(340, 40);
            passwordTextBox.Location = new Point(30, 225);
            passwordTextBox.BorderRadius = 4;
            passwordTextBox.PlaceholderText = "••••••••";
            passwordTextBox.PasswordChar = true;
            passwordTextBox.BorderColor = Color.FromArgb(206, 212, 218);

            // Remember CheckBox
            rememberCheckBox.AutoSize = true;
            rememberCheckBox.Location = new Point(30, 280);
            rememberCheckBox.Font = new Font("Segoe UI", 9F);
            rememberCheckBox.ForeColor = textColor;
            rememberCheckBox.Text = "Recordarme";

            // Forgot Password Link
            forgotPasswordLink.AutoSize = true;
            forgotPasswordLink.Location = new Point(230, 280);
            forgotPasswordLink.Font = new Font("Segoe UI", 9F);
            forgotPasswordLink.Text = "¿Olvidaste tu contraseña?";
            forgotPasswordLink.LinkColor = primaryColor;
            forgotPasswordLink.Click += ForgotPasswordLink_Click;

            // Login Button
            loginButton.Text = "Iniciar Sesión";
            loginButton.Size = new Size(340, 45);
            loginButton.Location = new Point(30, 320);
            loginButton.BorderRadius = 4;
            loginButton.BackColor = primaryColor;
            loginButton.ForeColor = Color.White;
            loginButton.Click += LoginButton_Click;

            // Exit Button
            exitButton.Text = "Salir";
            exitButton.Size = new Size(340, 45);
            exitButton.Location = new Point(/*((this.ClientSize.Width - 400) / 2)+30*/loginPanel.Location.X+loginButton.Location.X, loginPanel.Height + 50);
            exitButton.BorderRadius = 4;
            exitButton.BackColor = Color.FromArgb(224, 76, 84);
            exitButton.ForeColor = Color.White;
            exitButton.Click += ExitButton_Click;

            // Status Label
            statusLabel.AutoSize = false;
            statusLabel.Size = new Size(340, 30);
            statusLabel.Location = new Point(30, 380);
            statusLabel.Font = new Font("Segoe UI", 9F);
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            statusLabel.ForeColor = Color.FromArgb(220, 53, 69);
            statusLabel.Visible = false;

            // Add controls to panel
            loginPanel.Controls.Add(titleLabel);
            loginPanel.Controls.Add(subtitleLabel);
            loginPanel.Controls.Add(emailLabel);
            loginPanel.Controls.Add(emailTextBox);
            loginPanel.Controls.Add(passwordLabel);
            loginPanel.Controls.Add(passwordTextBox);
            loginPanel.Controls.Add(rememberCheckBox);
            loginPanel.Controls.Add(forgotPasswordLink);
            loginPanel.Controls.Add(loginButton);
            loginPanel.Controls.Add(statusLabel);

            // Add controls to form
            //this.Controls.Add(logoBox);
            this.Controls.Add(exitButton);
            this.Controls.Add(loginPanel);
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                string email = emailTextBox.Texts;
                string password = passwordTextBox.Texts;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    statusLabel.Text = "Por favor, ingrese email y contraseña.";
                    statusLabel.Visible = true;
                    return;
                }

                // Deshabilitar el botón de login mientras se procesa
                loginButton.Enabled = false;
                loginButton.Text = "Iniciando sesión...";
                statusLabel.Visible = false;

                // Crear instancia de ApiClient
                ApiClient apiClient = new ApiClient();

                // Intentar iniciar sesión
                var (success, userType, message) = await apiClient.LoginAsync(email, password);

                if (success)
                {
                    // Si el login fue exitoso, autenticar con Firebase
                    loginButton.Text = "Conectando con Firebase...";
                    Console.WriteLine("=== LOGIN API EXITOSO - INICIANDO FIREBASE ===");
                    Console.WriteLine($"Usuario autenticado: {email}");
                    Console.WriteLine($"Tipo de usuario: {userType}");

                    var firebaseAuth = new FirebaseAuthService();
                    Console.WriteLine("Instancia de FirebaseAuthService creada");

                    bool firebaseSuccess = await firebaseAuth.AuthenticateAsync();
                    Console.WriteLine($"Resultado autenticación Firebase: {firebaseSuccess}");

                    if (firebaseSuccess)
                    {
                        Console.WriteLine("✅ Firebase autenticado correctamente");
                        // Guardar la instancia de autenticación en AppConfig para uso global
                        AppConfig.SetFirebaseAuth(firebaseAuth);

                        // Crear un objeto User básico para pasar al MainForm
                        User user = new User
                        {
                            Email = email,
                            Role = UserRole.ADMIN,
                            Name = "Administrador",
                            IsActive = true
                        };

                        Console.WriteLine("Abriendo MainForm...");
                        Console.WriteLine($"🎯 Estado final de autenticación:");
                        Console.WriteLine($"  - API Login exitoso: {success}");
                        Console.WriteLine($"  - Firebase autenticado: {AppConfig.IsFirebaseAuthenticated()}");
                        Console.WriteLine($"  - Token disponible: {!string.IsNullOrEmpty(AppConfig.GetFirebaseAuthToken())}");
                        Console.WriteLine($"  - Usuario: "/*{user.Name}"*/+" ({user.Email})");
                        Console.WriteLine("=== INICIANDO APLICACIÓN PRINCIPAL ===");
                        // Abrir el formulario principal
                        MainForm mainForm = new MainForm(user);
                        this.Hide();
                        mainForm.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        Console.WriteLine("❌ Error en autenticación Firebase");
                        statusLabel.Text = "Error al conectar con Firebase. Intente nuevamente.";
                        statusLabel.Visible = true;
                    }
                }
                else
                {
                    // Mostrar el mensaje de error
                    statusLabel.Text = message;
                    statusLabel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Error al procesar la solicitud de inicio de sesión.";
                statusLabel.Visible = true;
                Console.WriteLine($"Login error: {ex.Message}");
            }
            finally
            {
                // Restaurar el botón de login
                loginButton.Enabled = true;
                loginButton.Text = "Iniciar Sesión";
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void ForgotPasswordLink_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Funcionalidad de recuperación de contraseña no implementada en esta versión de demostración.",
                "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
