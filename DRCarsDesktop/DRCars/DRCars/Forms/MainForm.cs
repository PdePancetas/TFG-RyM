using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Controls;
using DRCars.Models;
using DRCars.Utils;

namespace DRCars.Forms
{
    public partial class MainForm : Form
    {
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Panel headerPanel;
        private Label titleLabel;
        private RoundedButton dashboardButton;
        private RoundedButton vehiclesButton;
        private RoundedButton salesButton;
        private RoundedButton usersButton;
        private RoundedButton settingsButton;
        private RoundedButton logoutButton;
        private Label userNameLabel;
        private Label userRoleLabel;
        private PictureBox logoBox;

        private DashboardControl dashboardControl;
        private VehiclesControl vehiclesControl;
        private SalesControl salesControl;
        private UsersControl usersControl;
        private SettingsControl settingsControl;

        private User currentUser;

        public MainForm(User user)
        {
            currentUser = user;
            InitializeComponent();
            InitializeControls();
            ShowDashboard();
        }

        private void InitializeComponent()
        {
            this.sidebarPanel = new Panel();
            this.contentPanel = new Panel();
            this.headerPanel = new Panel();
            this.titleLabel = new Label();
            this.dashboardButton = new RoundedButton();
            this.vehiclesButton = new RoundedButton();
            this.salesButton = new RoundedButton();
            this.usersButton = new RoundedButton();
            this.settingsButton = new RoundedButton();
            this.logoutButton = new RoundedButton();
            this.userNameLabel = new Label();
            this.userRoleLabel = new Label();
            this.logoBox = new PictureBox();

            // Form
            this.Text = "DRCars - Sistema de Gestión";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 700);

            // Header Panel
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.Height = 60;
            this.headerPanel.BackColor = Color.White;
            this.headerPanel.BorderStyle = BorderStyle.FixedSingle;

            // Title Label
            this.titleLabel.AutoSize = false;
            this.titleLabel.Dock = DockStyle.Fill;
            this.titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.titleLabel.Text = "Dashboard";

            // Sidebar Panel
            this.sidebarPanel.Dock = DockStyle.Left;
            this.sidebarPanel.Width = 220;
            this.sidebarPanel.BackColor = Color.FromArgb(30, 30, 30);
            this.sidebarPanel.Padding = new Padding(10);

            // Logo Box
            this.logoBox.Size = new Size(200, 80);
            this.logoBox.Location = new Point(10, 20);
            this.logoBox.BackColor = Color.Transparent;
            this.logoBox.SizeMode = PictureBoxSizeMode.Zoom;
            // Set a placeholder logo or load from resources

            // User Name Label
            this.userNameLabel.AutoSize = false;
            this.userNameLabel.Size = new Size(200, 30);
            this.userNameLabel.Location = new Point(10, 110);
            this.userNameLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.userNameLabel.ForeColor = Color.White;
            this.userNameLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.userNameLabel.Text = currentUser?.Name ?? "Usuario";

            // User Role Label
            this.userRoleLabel.AutoSize = false;
            this.userRoleLabel.Size = new Size(200, 20);
            this.userRoleLabel.Location = new Point(10, 140);
            this.userRoleLabel.Font = new Font("Segoe UI", 9F);
            this.userRoleLabel.ForeColor = Color.Silver;
            this.userRoleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.userRoleLabel.Text = GetRoleText(currentUser?.Role ?? UserRole.Viewer);

            // Dashboard Button
            this.dashboardButton.Text = "Dashboard";
            this.dashboardButton.Size = new Size(200, 45);
            this.dashboardButton.Location = new Point(10, 180);
            this.dashboardButton.BackColor = Color.FromArgb(50, 50, 50);
            this.dashboardButton.ForeColor = Color.White;
            this.dashboardButton.BorderRadius = 10;
            this.dashboardButton.Click += DashboardButton_Click;

            // Vehicles Button
            this.vehiclesButton.Text = "Vehículos";
            this.vehiclesButton.Size = new Size(200, 45);
            this.vehiclesButton.Location = new Point(10, 235);
            this.vehiclesButton.BackColor = Color.FromArgb(50, 50, 50);
            this.vehiclesButton.ForeColor = Color.White;
            this.vehiclesButton.BorderRadius = 10;
            this.vehiclesButton.Click += VehiclesButton_Click;

            // Sales Button
            this.salesButton.Text = "Ventas";
            this.salesButton.Size = new Size(200, 45);
            this.salesButton.Location = new Point(10, 290);
            this.salesButton.BackColor = Color.FromArgb(50, 50, 50);
            this.salesButton.ForeColor = Color.White;
            this.salesButton.BorderRadius = 10;
            this.salesButton.Click += SalesButton_Click;

            // Users Button
            this.usersButton.Text = "Usuarios";
            this.usersButton.Size = new Size(200, 45);
            this.usersButton.Location = new Point(10, 345);
            this.usersButton.BackColor = Color.FromArgb(50, 50, 50);
            this.usersButton.ForeColor = Color.White;
            this.usersButton.BorderRadius = 10;
            this.usersButton.Click += UsersButton_Click;

            // Settings Button
            this.settingsButton.Text = "Configuración";
            this.settingsButton.Size = new Size(200, 45);
            this.settingsButton.Location = new Point(10, 400);
            this.settingsButton.BackColor = Color.FromArgb(50, 50, 50);
            this.settingsButton.ForeColor = Color.White;
            this.settingsButton.BorderRadius = 10;
            this.settingsButton.Click += SettingsButton_Click;

            // Logout Button
            this.logoutButton.Text = "Cerrar Sesión";
            this.logoutButton.Size = new Size(200, 45);
            this.logoutButton.Location = new Point(10, 600);
            this.logoutButton.BackColor = Color.FromArgb(200, 50, 50);
            this.logoutButton.ForeColor = Color.White;
            this.logoutButton.BorderRadius = 10;
            this.logoutButton.Click += LogoutButton_Click;

            // Content Panel
            this.contentPanel.Dock = DockStyle.Fill;
            this.contentPanel.BackColor = Color.FromArgb(245, 245, 245);
            this.contentPanel.Padding = new Padding(20);

            // Add controls to panels
            this.headerPanel.Controls.Add(this.titleLabel);

            this.sidebarPanel.Controls.Add(this.logoBox);
            this.sidebarPanel.Controls.Add(this.userNameLabel);
            this.sidebarPanel.Controls.Add(this.userRoleLabel);
            this.sidebarPanel.Controls.Add(this.dashboardButton);
            this.sidebarPanel.Controls.Add(this.vehiclesButton);
            this.sidebarPanel.Controls.Add(this.salesButton);
            this.sidebarPanel.Controls.Add(this.usersButton);
            this.sidebarPanel.Controls.Add(this.settingsButton);
            this.sidebarPanel.Controls.Add(this.logoutButton);

            // Add panels to form
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.sidebarPanel);
            this.Controls.Add(this.headerPanel);
        }

        private void InitializeControls()
        {
            // Initialize user controls
            dashboardControl = new DashboardControl();
            vehiclesControl = new VehiclesControl();
            salesControl = new SalesControl();
            usersControl = new UsersControl();
            settingsControl = new SettingsControl();

            // Set dock style for all controls
            dashboardControl.Dock = DockStyle.Fill;
            vehiclesControl.Dock = DockStyle.Fill;
            salesControl.Dock = DockStyle.Fill;
            usersControl.Dock = DockStyle.Fill;
            settingsControl.Dock = DockStyle.Fill;
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
                    return "Agente de Ventas";
                case UserRole.Viewer:
                    return "Visualizador";
                default:
                    return "Usuario";
            }
        }

        private void ShowDashboard()
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(dashboardControl);
            titleLabel.Text = "Dashboard";
            SetActiveButton(dashboardButton);
        }

        private void ShowVehicles()
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(vehiclesControl);
            titleLabel.Text = "Gestión de Vehículos";
            SetActiveButton(vehiclesButton);
        }

        private void ShowSales()
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(salesControl);
            titleLabel.Text = "Gestión de Ventas";
            SetActiveButton(salesButton);
        }

        private void ShowUsers()
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(usersControl);
            titleLabel.Text = "Gestión de Usuarios";
            SetActiveButton(usersButton);
        }

        private void ShowSettings()
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(settingsControl);
            titleLabel.Text = "Configuración";
            SetActiveButton(settingsButton);
        }

        private void SetActiveButton(RoundedButton activeButton)
        {
            // Reset all buttons
            dashboardButton.BackColor = Color.FromArgb(50, 50, 50);
            vehiclesButton.BackColor = Color.FromArgb(50, 50, 50);
            salesButton.BackColor = Color.FromArgb(50, 50, 50);
            usersButton.BackColor = Color.FromArgb(50, 50, 50);
            settingsButton.BackColor = Color.FromArgb(50, 50, 50);

            // Set active button
            activeButton.BackColor = Color.FromArgb(0, 120, 215);
        }

        private void DashboardButton_Click(object sender, EventArgs e)
        {
            ShowDashboard();
        }

        private void VehiclesButton_Click(object sender, EventArgs e)
        {
            ShowVehicles();
        }

        private void SalesButton_Click(object sender, EventArgs e)
        {
            ShowSales();
        }

        private void UsersButton_Click(object sender, EventArgs e)
        {
            ShowUsers();
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cerrar sesión?", "Cerrar Sesión",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
