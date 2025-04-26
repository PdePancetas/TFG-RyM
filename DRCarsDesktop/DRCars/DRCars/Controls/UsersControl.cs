using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Models;
using DRCars.Utils;
using DRCars.Forms;

namespace DRCars.Controls
{
    public class UsersControl : UserControl
    {
        private RoundedPanel headerPanel;
        private DataGridView usersDataGridView;
        private RoundedButton addUserButton;
        private RoundedButton editUserButton;
        private RoundedButton deleteUserButton;
        private RoundedButton refreshButton;
        private RoundedTextBox searchTextBox;

        private ApiClient apiClient;
        private List<User> allUsers;
        private User selectedUser;

        public UsersControl()
        {
            apiClient = new ApiClient();
            InitializeComponent();
            LoadUsers();
        }

        private void InitializeComponent()
        {
            headerPanel = new RoundedPanel();
            usersDataGridView = new DataGridView();
            addUserButton = new RoundedButton();
            editUserButton = new RoundedButton();
            deleteUserButton = new RoundedButton();
            refreshButton = new RoundedButton();
            searchTextBox = new RoundedTextBox();

            // Header Panel
            headerPanel.BorderRadius = 15;
            headerPanel.BorderColor = Color.FromArgb(220, 220, 220);
            headerPanel.BorderSize = 1;
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 70;
            headerPanel.Padding = new Padding(15);
            headerPanel.Margin = new Padding(0, 0, 0, 20);

            // Search TextBox
            searchTextBox.PlaceholderText = "Buscar usuarios...";
            searchTextBox.Size = new Size(300, 40);
            searchTextBox.Location = new Point(15, 15);
            searchTextBox.BorderRadius = 10;
            searchTextBox.TextChanged += SearchTextBox_TextChanged;

            // Add User Button
            addUserButton.Text = "Añadir Usuario";
            addUserButton.Size = new Size(130, 40);
            addUserButton.Location = new Point(headerPanel.Width - 550, 15);
            addUserButton.BorderRadius = 10;
            addUserButton.BackColor = Color.FromArgb(0, 120, 215);
            addUserButton.ForeColor = Color.White;
            addUserButton.Click += AddUserButton_Click;

            // Edit User Button
            editUserButton.Text = "Editar";
            editUserButton.Size = new Size(130, 40);
            editUserButton.Location = new Point(headerPanel.Width - 410, 15);
            editUserButton.BorderRadius = 10;
            editUserButton.BackColor = Color.FromArgb(50, 50, 50);
            editUserButton.ForeColor = Color.White;
            editUserButton.Click += EditUserButton_Click;

            // Delete User Button
            deleteUserButton.Text = "Eliminar";
            deleteUserButton.Size = new Size(130, 40);
            deleteUserButton.Location = new Point(headerPanel.Width - 270, 15);
            deleteUserButton.BorderRadius = 10;
            deleteUserButton.BackColor = Color.FromArgb(200, 50, 50);
            deleteUserButton.ForeColor = Color.White;
            deleteUserButton.Click += DeleteUserButton_Click;

            // Refresh Button
            refreshButton.Text = "Actualizar";
            refreshButton.Size = new Size(130, 40);
            refreshButton.Location = new Point(headerPanel.Width - 130, 15);
            refreshButton.BorderRadius = 10;
            refreshButton.BackColor = Color.FromArgb(50, 50, 50);
            refreshButton.ForeColor = Color.White;
            refreshButton.Click += RefreshButton_Click;

            // Users DataGridView
            usersDataGridView.Dock = DockStyle.Fill;
            usersDataGridView.BackgroundColor = Color.White;
            usersDataGridView.BorderStyle = BorderStyle.None;
            usersDataGridView.RowHeadersVisible = false;
            usersDataGridView.AllowUserToAddRows = false;
            usersDataGridView.AllowUserToDeleteRows = false;
            usersDataGridView.AllowUserToResizeRows = false;
            usersDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            usersDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            usersDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            usersDataGridView.RowTemplate.Height = 40;
            usersDataGridView.Font = new Font("Segoe UI", 9F);
            usersDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 230);
            usersDataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            usersDataGridView.ColumnHeadersHeight = 50;
            usersDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            usersDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            usersDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            usersDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            usersDataGridView.EnableHeadersVisualStyles = false;
            usersDataGridView.CellClick += UsersDataGridView_CellClick;

            // Add columns to DataGridView
            usersDataGridView.Columns.Add("Id", "ID");
            usersDataGridView.Columns.Add("Name", "Nombre");
            usersDataGridView.Columns.Add("Email", "Email");
            usersDataGridView.Columns.Add("Phone", "Teléfono");
            usersDataGridView.Columns.Add("Role", "Rol");
            usersDataGridView.Columns.Add("Status", "Estado");
            usersDataGridView.Columns.Add("CreatedAt", "Fecha de Creación");
            usersDataGridView.Columns.Add("LastLogin", "Último Acceso");

            // Set column widths
            usersDataGridView.Columns["Id"].Width = 50;
            usersDataGridView.Columns["Name"].Width = 150;
            usersDataGridView.Columns["Email"].Width = 200;
            usersDataGridView.Columns["Phone"].Width = 120;
            usersDataGridView.Columns["Role"].Width = 100;
            usersDataGridView.Columns["Status"].Width = 80;
            usersDataGridView.Columns["CreatedAt"].Width = 150;
            usersDataGridView.Columns["LastLogin"].Width = 150;

            // Add controls to panels
            headerPanel.Controls.Add(searchTextBox);
            headerPanel.Controls.Add(addUserButton);
            headerPanel.Controls.Add(editUserButton);
            headerPanel.Controls.Add(deleteUserButton);
            headerPanel.Controls.Add(refreshButton);

            // Add panels to control
            this.Controls.Add(usersDataGridView);
            this.Controls.Add(headerPanel);

            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 245, 245);

            // Handle resize event to reposition buttons
            this.Resize += UsersControl_Resize;
        }

        private void UsersControl_Resize(object sender, EventArgs e)
        {
            // Reposition buttons when control is resized
            addUserButton.Location = new Point(headerPanel.Width - 550, 15);
            editUserButton.Location = new Point(headerPanel.Width - 410, 15);
            deleteUserButton.Location = new Point(headerPanel.Width - 270, 15);
            refreshButton.Location = new Point(headerPanel.Width - 130, 15);
        }

        private async void LoadUsers()
        {
            try
            {
                allUsers = await apiClient.GetUsersAsync();
                PopulateUsersDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateUsersDataGridView()
        {
            usersDataGridView.Rows.Clear();

            foreach (var user in allUsers)
            {
                string status = user.IsActive ? "Activo" : "Inactivo";
                string role = GetRoleText(user.Role);
                string lastLogin = user.LastLogin.HasValue ? user.LastLogin.Value.ToString("dd/MM/yyyy HH:mm") : "-";

                usersDataGridView.Rows.Add(
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Phone,
                    role,
                    status,
                    user.CreatedAt.ToString("dd/MM/yyyy"),
                    lastLogin
                );
            }
        }

        private string GetRoleText(UserRole role)
        {
            switch (role)
            {
                case UserRole.Admin:
                    return "Administrador";
                case UserRole.Manager:
                    return "Gerente";
                case UserRole.SalesAgent:
                    return "Agente";
                case UserRole.Viewer:
                    return "Visualizador";
                default:
                    return "Usuario";
            }
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchTextBox.Texts.ToLower();

            usersDataGridView.Rows.Clear();

            foreach (var user in allUsers)
            {
                if (string.IsNullOrEmpty(searchText) ||
                    user.Name.ToLower().Contains(searchText) ||
                    user.Email.ToLower().Contains(searchText) ||
                    user.Phone?.ToLower().Contains(searchText) == true ||
                    GetRoleText(user.Role).ToLower().Contains(searchText))
                {
                    string status = user.IsActive ? "Activo" : "Inactivo";
                    string role = GetRoleText(user.Role);
                    string lastLogin = user.LastLogin.HasValue ? user.LastLogin.Value.ToString("dd/MM/yyyy HH:mm") : "-";

                    usersDataGridView.Rows.Add(
                        user.Id,
                        user.Name,
                        user.Email,
                        user.Phone,
                        role,
                        status,
                        user.CreatedAt.ToString("dd/MM/yyyy"),
                        lastLogin
                    );
                }
            }
        }

        private void UsersDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int userId = Convert.ToInt32(usersDataGridView.Rows[e.RowIndex].Cells["Id"].Value);
                selectedUser = allUsers.Find(u => u.Id == userId);
            }
        }

        private void AddUserButton_Click(object sender, EventArgs e)
        {
            // Open add user form
            UserForm userForm = new UserForm();
            if (userForm.ShowDialog() == DialogResult.OK)
            {
                LoadUsers();
            }
        }

        private void EditUserButton_Click(object sender, EventArgs e)
        {
            if (selectedUser != null)
            {
                // Open edit user form
                UserForm userForm = new UserForm(selectedUser);
                if (userForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers();
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un usuario para editar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteUserButton_Click(object sender, EventArgs e)
        {
            if (selectedUser != null)
            {
                DialogResult result = MessageBox.Show($"¿Está seguro que desea eliminar al usuario {selectedUser.Name}?", "Eliminar Usuario",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Delete user logic would go here
                    // For now, just refresh the list
                    LoadUsers();
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un usuario para eliminar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }
    }
}
