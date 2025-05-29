using System;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Controls;
using DRCars.Models;
using DRCars.Utils;

namespace DRCars.Forms
{
    public partial class UserForm : Form
    {
        private RoundedPanel mainPanel;
        private Label titleLabel;
        private Label nameLabel;
        private RoundedTextBox nameTextBox;
        private Label emailLabel;
        private RoundedTextBox emailTextBox;
        private Label passwordLabel;
        private RoundedTextBox passwordTextBox;
        private Label roleLabel;
        private ComboBox roleComboBox;
        private CheckBox isActiveCheckBox;
        private RoundedButton saveButton;
        private RoundedButton cancelButton;
        private Label statusLabel;

        private User _user;
        private ApiClient apiClient;

        // Colores de Odoo
        private Color primaryColor = Color.FromArgb(0, 160, 157); // Verde Odoo
        private Color secondaryColor = Color.FromArgb(242, 242, 242); // Gris claro
        private Color textColor = Color.FromArgb(51, 51, 51); // Texto oscuro
        private Color accentColor = Color.FromArgb(108, 117, 125); // Gris para detalles

        public UserForm(User user = null)
        {
            _user = user;
            apiClient = new ApiClient();
            InitializeComponent();
            LoadUserData();
        }

        private void InitializeComponent()
        {
            mainPanel = new RoundedPanel();
            titleLabel = new Label();
            nameLabel = new Label();
            nameTextBox = new RoundedTextBox();
            emailLabel = new Label();
            emailTextBox = new RoundedTextBox();
            passwordLabel = new Label();
            passwordTextBox = new RoundedTextBox();
            roleLabel = new Label();
            roleComboBox = new ComboBox();
            isActiveCheckBox = new CheckBox();
            saveButton = new RoundedButton();
            cancelButton = new RoundedButton();
            statusLabel = new Label();

            // Form
            this.Text = _user == null ? "Añadir Usuario" : "Editar Usuario";
            this.Size = new Size(500, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = secondaryColor;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Main Panel
            mainPanel.BorderRadius = 8;
            mainPanel.BorderColor = Color.FromArgb(230, 230, 230);
            mainPanel.BorderSize = 1;
            mainPanel.Size = new Size(460, 480);
            mainPanel.Location = new Point(20, 20);
            mainPanel.BackColor = Color.White;
            mainPanel.Padding = new Padding(30);

            // Title Label
            titleLabel.AutoSize = false;
            titleLabel.Size = new Size(400, 40);
            titleLabel.Location = new Point(30, 20);
            titleLabel.Font = new Font("Segoe UI Semibold", 16F);
            titleLabel.ForeColor = textColor;
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            titleLabel.Text = _user == null ? "Añadir Nuevo Usuario" : "Editar Usuario";

            // Name Label
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(30, 80);
            nameLabel.Font = new Font("Segoe UI", 10F);
            nameLabel.ForeColor = textColor;
            nameLabel.Text = "Nombre";

            // Name TextBox
            nameTextBox.Size = new Size(400, 40);
            nameTextBox.Location = new Point(30, 105);
            nameTextBox.BorderRadius = 4;
            nameTextBox.PlaceholderText = "Nombre completo";
            nameTextBox.BorderColor = Color.FromArgb(206, 212, 218);

            // Email Label
            emailLabel.AutoSize = true;
            emailLabel.Location = new Point(30, 155);
            emailLabel.Font = new Font("Segoe UI", 10F);
            emailLabel.ForeColor = textColor;
            emailLabel.Text = "Email";

            // Email TextBox
            emailTextBox.Size = new Size(400, 40);
            emailTextBox.Location = new Point(30, 180);
            emailTextBox.BorderRadius = 4;
            emailTextBox.PlaceholderText = "email@ejemplo.com";
            emailTextBox.BorderColor = Color.FromArgb(206, 212, 218);

            // Password Label
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(30, 230);
            passwordLabel.Font = new Font("Segoe UI", 10F);
            passwordLabel.ForeColor = textColor;
            passwordLabel.Text = "Contraseña";

            // Password TextBox
            passwordTextBox.Size = new Size(400, 40);
            passwordTextBox.Location = new Point(30, 255);
            passwordTextBox.BorderRadius = 4;
            passwordTextBox.PlaceholderText = "Contraseña";
            passwordTextBox.PasswordChar = true;
            passwordTextBox.BorderColor = Color.FromArgb(206, 212, 218);

            // Role Label
            roleLabel.AutoSize = true;
            roleLabel.Location = new Point(30, 305);
            roleLabel.Font = new Font("Segoe UI", 10F);
            roleLabel.ForeColor = textColor;
            roleLabel.Text = "Rol";

            // Role ComboBox
            roleComboBox.Size = new Size(400, 40);
            roleComboBox.Location = new Point(30, 330);
            roleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            roleComboBox.Font = new Font("Segoe UI", 10F);
            roleComboBox.FlatStyle = FlatStyle.Flat;
            roleComboBox.Items.Add("Administrador");
            roleComboBox.Items.Add("Gerente");
            roleComboBox.Items.Add("Agente de Ventas");
            roleComboBox.Items.Add("Visualizador");
            roleComboBox.Items.Add("Usuario");
            roleComboBox.SelectedIndex = 0;

            // Is Active CheckBox
            isActiveCheckBox.AutoSize = true;
            isActiveCheckBox.Location = new Point(30, 380);
            isActiveCheckBox.Font = new Font("Segoe UI", 10F);
            isActiveCheckBox.ForeColor = textColor;
            isActiveCheckBox.Text = "Usuario Activo";
            isActiveCheckBox.Checked = true;

            // Save Button
            saveButton.Text = "Guardar";
            saveButton.Size = new Size(180, 45);
            saveButton.Location = new Point(30, 420);
            saveButton.BorderRadius = 4;
            saveButton.BackColor = primaryColor;
            saveButton.ForeColor = Color.White;
            saveButton.Click += SaveButton_Click;

            // Cancel Button
            cancelButton.Text = "Cancelar";
            cancelButton.Size = new Size(180, 45);
            cancelButton.Location = new Point(250, 420);
            cancelButton.BorderRadius = 4;
            cancelButton.BackColor = Color.FromArgb(108, 117, 125);
            cancelButton.ForeColor = Color.White;
            cancelButton.Click += CancelButton_Click;

            // Status Label
            statusLabel.AutoSize = false;
            statusLabel.Size = new Size(400, 30);
            statusLabel.Location = new Point(30, 380);
            statusLabel.Font = new Font("Segoe UI", 9F);
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            statusLabel.ForeColor = Color.FromArgb(220, 53, 69);
            statusLabel.Visible = false;

            // Add controls to panel
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(nameLabel);
            mainPanel.Controls.Add(nameTextBox);
            mainPanel.Controls.Add(emailLabel);
            mainPanel.Controls.Add(emailTextBox);
            mainPanel.Controls.Add(passwordLabel);
            mainPanel.Controls.Add(passwordTextBox);
            mainPanel.Controls.Add(roleLabel);
            mainPanel.Controls.Add(roleComboBox);
            mainPanel.Controls.Add(isActiveCheckBox);
            mainPanel.Controls.Add(saveButton);
            mainPanel.Controls.Add(cancelButton);
            mainPanel.Controls.Add(statusLabel);

            // Add panel to form
            this.Controls.Add(mainPanel);
        }

        private void LoadUserData()
        {
            if (_user != null)
            {
                nameTextBox.Texts = _user.Name;
                emailTextBox.Texts = _user.Email;
                passwordTextBox.Texts = "••••••••"; // Placeholder for security

                switch (_user.Role)
                {
                    case UserRole.ADMIN:
                        roleComboBox.SelectedIndex = 0;
                        break;
                    case UserRole.MANAGER:
                        roleComboBox.SelectedIndex = 1;
                        break;
                    case UserRole.SALESAGENT:
                        roleComboBox.SelectedIndex = 2;
                        break;
                    case UserRole.VIEWER:
                        roleComboBox.SelectedIndex = 3;
                        break;
                    case UserRole.USER:
                        roleComboBox.SelectedIndex = 4;
                        break;
                }

                isActiveCheckBox.Checked = _user.IsActive;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(nameTextBox.Texts) || string.IsNullOrEmpty(emailTextBox.Texts))
            {
                statusLabel.Text = "Por favor, complete todos los campos obligatorios.";
                statusLabel.Visible = true;
                return;
            }

            // Create or update user
            if (_user == null)
            {
                _user = new User();
            }

            _user.Name = nameTextBox.Texts;
            _user.Email = emailTextBox.Texts;

            // Only update password if it's not the placeholder
            if (passwordTextBox.Texts != "••••••••")
            {
                _user.Password = passwordTextBox.Texts;
            }

            // Set role based on selection
            switch (roleComboBox.SelectedIndex)
            {
                case 0:
                    _user.Role = UserRole.ADMIN;
                    break;
                case 1:
                    _user.Role = UserRole.MANAGER;
                    break;
                case 2:
                    _user.Role = UserRole.SALESAGENT;
                    break;
                case 3:
                    _user.Role = UserRole.VIEWER;
                    break;
                case 4:
                    _user.Role = UserRole.USER;
                    break;
            }

            _user.IsActive = isActiveCheckBox.Checked;

            // In a real app, you would save to API
            // For demo, just close with OK result
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
