using System;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Controls;
using DRCars.Models;

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
        private Label phoneLabel;
        private RoundedTextBox phoneTextBox;
        private Label passwordLabel;
        private RoundedTextBox passwordTextBox;
        private Label confirmPasswordLabel;
        private RoundedTextBox confirmPasswordTextBox;
        private Label roleLabel;
        private ComboBox roleComboBox;
        private CheckBox isActiveCheckBox;
        private RoundedButton saveButton;
        private RoundedButton cancelButton;

        private User _user;
        private bool _isEditMode;

        public UserForm(User user = null)
        {
            _user = user;
            _isEditMode = user != null;
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
            phoneLabel = new Label();
            phoneTextBox = new RoundedTextBox();
            passwordLabel = new Label();
            passwordTextBox = new RoundedTextBox();
            confirmPasswordLabel = new Label();
            confirmPasswordTextBox = new RoundedTextBox();
            roleLabel = new Label();
            roleComboBox = new ComboBox();
            isActiveCheckBox = new CheckBox();
            saveButton = new RoundedButton();
            cancelButton = new RoundedButton();

            // Form
            this.Text = _isEditMode ? "Editar Usuario" : "Añadir Usuario";
            this.Size = new Size(600, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Main Panel
            mainPanel.BorderRadius = 15;
            mainPanel.BorderColor = Color.FromArgb(220, 220, 220);
            mainPanel.BorderSize = 1;
            mainPanel.Size = new Size(550, 600);
            mainPanel.Location = new Point((this.ClientSize.Width - 550) / 2, (this.ClientSize.Height - 600) / 2);
            mainPanel.BackColor = Color.White;
            mainPanel.Padding = new Padding(30);

            // Title Label
            titleLabel.AutoSize = false;
            titleLabel.Size = new Size(490, 40);
            titleLabel.Location = new Point(30, 20);
            titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Text = _isEditMode ? "Editar Usuario" : "Añadir Usuario";

            // Name Label
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(30, 80);
            nameLabel.Font = new Font("Segoe UI", 9F);
            nameLabel.Text = "Nombre";

            // Name TextBox
            nameTextBox.Size = new Size(490, 40);
            nameTextBox.Location = new Point(30, 105);
            nameTextBox.BorderRadius = 10;
            nameTextBox.PlaceholderText = "Nombre completo";

            // Email Label
            emailLabel.AutoSize = true;
            emailLabel.Location = new Point(30, 155);
            emailLabel.Font = new Font("Segoe UI", 9F);
            emailLabel.Text = "Email";

            // Email TextBox
            emailTextBox.Size = new Size(490, 40);
            emailTextBox.Location = new Point(30, 180);
            emailTextBox.BorderRadius = 10;
            emailTextBox.PlaceholderText = "Correo electrónico";

            // Phone Label
            phoneLabel.AutoSize = true;
            phoneLabel.Location = new Point(30, 230);
            phoneLabel.Font = new Font("Segoe UI", 9F);
            phoneLabel.Text = "Teléfono";

            // Phone TextBox
            phoneTextBox.Size = new Size(490, 40);
            phoneTextBox.Location = new Point(30, 255);
            phoneTextBox.BorderRadius = 10;
            phoneTextBox.PlaceholderText = "Número de teléfono";

            // Password Label
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(30, 305);
            passwordLabel.Font = new Font("Segoe UI", 9F);
            passwordLabel.Text = _isEditMode ? "Nueva Contraseña (dejar en blanco para mantener la actual)" : "Contraseña";

            // Password TextBox
            passwordTextBox.Size = new Size(490, 40);
            passwordTextBox.Location = new Point(30, 330);
            passwordTextBox.BorderRadius = 10;
            passwordTextBox.PlaceholderText = "Contraseña";
            passwordTextBox.PasswordChar = true;

            // Confirm Password Label
            confirmPasswordLabel.AutoSize = true;
            confirmPasswordLabel.Location = new Point(30, 380);
            confirmPasswordLabel.Font = new Font("Segoe UI", 9F);
            confirmPasswordLabel.Text = "Confirmar Contraseña";

            // Confirm Password TextBox
            confirmPasswordTextBox.Size = new Size(490, 40);
            confirmPasswordTextBox.Location = new Point(30, 405);
            confirmPasswordTextBox.BorderRadius = 10;
            confirmPasswordTextBox.PlaceholderText = "Confirmar contraseña";
            confirmPasswordTextBox.PasswordChar = true;

            // Role Label
            roleLabel.AutoSize = true;
            roleLabel.Location = new Point(30, 455);
            roleLabel.Font = new Font("Segoe UI", 9F);
            roleLabel.Text = "Rol";

            // Role ComboBox
            roleComboBox.Size = new Size(230, 40);
            roleComboBox.Location = new Point(30, 480);
            roleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            roleComboBox.Font = new Font("Segoe UI", 9F);
            roleComboBox.Items.AddRange(new object[] { "Administrador", "Gerente", "Agente de Ventas", "Visualizador" });

            // Is Active CheckBox
            isActiveCheckBox.AutoSize = true;
            isActiveCheckBox.Location = new Point(290, 480);
            isActiveCheckBox.Font = new Font("Segoe UI", 9F);
            isActiveCheckBox.Text = "Usuario Activo";
            isActiveCheckBox.Checked = true;

            // Save Button
            saveButton.Text = "Guardar";
            saveButton.Size = new Size(230, 45);
            saveButton.Location = new Point(30, 530);
            saveButton.BorderRadius = 10;
            saveButton.BackColor = Color.FromArgb(0, 120, 215);
            saveButton.ForeColor = Color.White;
            saveButton.Click += SaveButton_Click;

            // Cancel Button
            cancelButton.Text = "Cancelar";
            cancelButton.Size = new Size(230, 45);
            cancelButton.Location = new Point(290, 530);
            cancelButton.BorderRadius = 10;
            cancelButton.BackColor = Color.FromArgb(200, 50, 50);
            cancelButton.ForeColor = Color.White;
            cancelButton.Click += CancelButton_Click;

            // Add controls to panel
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(nameLabel);
            mainPanel.Controls.Add(nameTextBox);
            mainPanel.Controls.Add(emailLabel);
            mainPanel.Controls.Add(emailTextBox);
            mainPanel.Controls.Add(phoneLabel);
            mainPanel.Controls.Add(phoneTextBox);
            mainPanel.Controls.Add(passwordLabel);
            mainPanel.Controls.Add(passwordTextBox);
            mainPanel.Controls.Add(confirmPasswordLabel);
            mainPanel.Controls.Add(confirmPasswordTextBox);
            mainPanel.Controls.Add(roleLabel);
            mainPanel.Controls.Add(roleComboBox);
            mainPanel.Controls.Add(isActiveCheckBox);
            mainPanel.Controls.Add(saveButton);
            mainPanel.Controls.Add(cancelButton);

            // Add panel to form
            this.Controls.Add(mainPanel);
        }

        private void LoadUserData()
        {
            if (_isEditMode && _user != null)
            {
                nameTextBox.Texts = _user.Name;
                emailTextBox.Texts = _user.Email;
                phoneTextBox.Texts = _user.Phone;
                roleComboBox.SelectedIndex = (int)_user.Role;
                isActiveCheckBox.Checked = _user.IsActive;
            }
            else
            {
                // Set defaults for new user
                roleComboBox.SelectedIndex = 3; // Viewer by default
                isActiveCheckBox.Checked = true;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrEmpty(nameTextBox.Texts) || string.IsNullOrEmpty(emailTextBox.Texts))
                {
                    MessageBox.Show("Por favor, complete los campos obligatorios (Nombre y Email).", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate email format
                if (!IsValidEmail(emailTextBox.Texts))
                {
                    MessageBox.Show("Por favor, ingrese un email válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate password
                if (!_isEditMode || !string.IsNullOrEmpty(passwordTextBox.Texts))
                {
                    if (passwordTextBox.Texts != confirmPasswordTextBox.Texts)
                    {
                        MessageBox.Show("Las contraseñas no coinciden.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (passwordTextBox.Texts.Length < 6)
                    {
                        MessageBox.Show("La contraseña debe tener al menos 6 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Create or update user
                if (_isEditMode)
                {
                    // Update existing user
                    _user.Name = nameTextBox.Texts;
                    _user.Email = emailTextBox.Texts;
                    _user.Phone = phoneTextBox.Texts;
                    _user.Role = (UserRole)roleComboBox.SelectedIndex;
                    _user.IsActive = isActiveCheckBox.Checked;

                    // Update password if provided
                    if (!string.IsNullOrEmpty(passwordTextBox.Texts))
                    {
                        _user.Password = passwordTextBox.Texts; // In a real app, this would be hashed
                    }
                }
                else
                {
                    // Create new user
                    User newUser = new User
                    {
                        Name = nameTextBox.Texts,
                        Email = emailTextBox.Texts,
                        Phone = phoneTextBox.Texts,
                        Password = passwordTextBox.Texts, // In a real app, this would be hashed
                        Role = (UserRole)roleComboBox.SelectedIndex,
                        IsActive = isActiveCheckBox.Checked,
                        CreatedAt = DateTime.Now
                    };

                    // In a real app, you would add the user to the database
                    _user = newUser;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
