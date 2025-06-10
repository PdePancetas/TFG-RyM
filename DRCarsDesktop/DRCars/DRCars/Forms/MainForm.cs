﻿using System;
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
        private Panel menuItemsPanel;
        private Label userNameLabel;
        private Label userRoleLabel;
        private PictureBox logoBox;
        private Panel activeIndicator;

        private DashboardControl dashboardControl;
        private VehiclesControl vehiclesControl;
        private SalesControl salesControl;
        private UsersControl usersControl;
        private SettingsControl settingsControl;

        private User currentUser;
        private Control activeControl;
        private string currentSection = "Dashboard";

        // Diccionario para almacenar referencias a los paneles del menú
        private Dictionary<string, Panel> menuPanels = new Dictionary<string, Panel>();

        // Colores de Odoo
        private Color primaryColor = Color.FromArgb(0, 160, 157); // Verde Odoo
        private Color secondaryColor = Color.FromArgb(242, 242, 242); // Gris claro
        private Color textColor = Color.FromArgb(51, 51, 51); // Texto oscuro
        private Color accentColor = Color.FromArgb(108, 117, 125); // Gris para detalles
        private Color whiteColor = Color.White;

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
            this.menuItemsPanel = new Panel();
            this.userNameLabel = new Label();
            this.userRoleLabel = new Label();
            this.logoBox = new PictureBox();
            this.activeIndicator = new Panel();

            // Form
            this.Text = "DRCars - Sistema de Gestión";
            this.Size = new Size(1280, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1100, 700);
            this.BackColor = secondaryColor;

            // Header Panel
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.Height = 60;
            this.headerPanel.BackColor = whiteColor;
            this.headerPanel.Padding = new Padding(20, 0, 20, 0);

            // Title Label
            this.titleLabel.AutoSize = false;
            this.titleLabel.Dock = DockStyle.Fill;
            this.titleLabel.Font = new Font("Segoe UI Semibold", 16F);
            this.titleLabel.ForeColor = textColor;
            this.titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.titleLabel.Text = "Dashboard";

            // Sidebar Panel
            this.sidebarPanel.Dock = DockStyle.Left;
            this.sidebarPanel.Width = 240;
            this.sidebarPanel.BackColor = whiteColor;
            this.sidebarPanel.Padding = new Padding(0, 20, 0, 20);

            // Logo Box
            this.logoBox.Size = new Size(200, 60);
            this.logoBox.Location = new Point(20, 20);
            this.logoBox.BackColor = Color.Transparent;
            this.logoBox.SizeMode = PictureBoxSizeMode.Zoom;
            // Set a placeholder logo or load from resources

            // Menu Items Panel
            this.menuItemsPanel.Location = new Point(0, 100);
            this.menuItemsPanel.Size = new Size(240, 400);
            this.menuItemsPanel.BackColor = Color.Transparent;

            // User Name Label
            this.userNameLabel.AutoSize = false;
            this.userNameLabel.Size = new Size(240, 30);
            this.userNameLabel.Location = new Point(0, this.sidebarPanel.Height - 80);
            this.userNameLabel.Font = new Font("Segoe UI Semibold", 12F);
            this.userNameLabel.ForeColor = textColor;
            this.userNameLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.userNameLabel.Text = currentUser?.Name ?? "Usuario";

            // User Role Label
            this.userRoleLabel.AutoSize = false;
            this.userRoleLabel.Size = new Size(240, 20);
            this.userRoleLabel.Location = new Point(0, this.sidebarPanel.Height - 50);
            this.userRoleLabel.Font = new Font("Segoe UI", 9F);
            this.userRoleLabel.ForeColor = accentColor;
            this.userRoleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.userRoleLabel.Text = GetRoleText(currentUser.Role);

            // Active Indicator
            this.activeIndicator.Size = new Size(4, 40);
            this.activeIndicator.BackColor = primaryColor;
            this.activeIndicator.Visible = true;

            // Content Panel
            this.contentPanel.Dock = DockStyle.Fill;
            this.contentPanel.BackColor = secondaryColor;
            this.contentPanel.Padding = new Padding(20);

            // Add controls to panels
            this.headerPanel.Controls.Add(this.titleLabel);

            this.sidebarPanel.Controls.Add(this.logoBox);
            this.sidebarPanel.Controls.Add(this.menuItemsPanel);
            this.sidebarPanel.Controls.Add(this.userNameLabel);
            this.sidebarPanel.Controls.Add(this.userRoleLabel);
            this.sidebarPanel.Controls.Add(this.activeIndicator);

            // Add panels to form
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.sidebarPanel);
            this.Controls.Add(this.headerPanel);

            // Create menu items
            CreateMenuItem("Dashboard", "dashboard", DashboardButton_Click);
            CreateMenuItem("Vehículos", "vehicles", VehiclesButton_Click);
            CreateMenuItem("Tramitación", "sales", SalesButton_Click);
            CreateMenuItem("Usuarios", "users", UsersButton_Click);
            CreateMenuItem("Configuración", "settings", SettingsButton_Click);
            CreateMenuItem("Cerrar Sesión", "logout", LogoutButton_Click, true);
        }

        private void CreateMenuItem(string text, string iconName, EventHandler clickEvent, bool isLogout = false)
        {
            int index = menuItemsPanel.Controls.Count;
            int yPos = index * 50;

            Panel itemPanel = new Panel
            {
                Size = new Size(240, 40),
                Location = new Point(0, yPos),
                Cursor = Cursors.Hand,
                Tag = text,
                Name = text + "Panel" // Añadir un nombre único para facilitar la identificación
            };

            if (isLogout)
            {
                // Position logout at bottom
                itemPanel.Location = new Point(0, 350);
            }

            Label iconLabel = new Label
            {
                AutoSize = false,
                Size = new Size(40, 40),
                Location = new Point(20, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Symbol", 14F)
            };

            // Simple icon representation using symbols
            switch (iconName)
            {
                case "dashboard": iconLabel.Text = "📊"; break;
                case "vehicles": iconLabel.Text = "🚗"; break;
                case "sales": iconLabel.Text = "💰"; break;
                case "users": iconLabel.Text = "👥"; break;
                case "settings": iconLabel.Text = "⚙️"; break;
                case "logout": iconLabel.Text = "🚪"; break;
                default: iconLabel.Text = "📄"; break;
            }

            Label textLabel = new Label
            {
                AutoSize = false,
                Size = new Size(180, 40),
                Location = new Point(60, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 11F),
                ForeColor = isLogout ? Color.FromArgb(220, 53, 69) : textColor,
                Text = text
            };

            // Hacer que todo el panel sea clickeable
            itemPanel.Click += clickEvent;
            iconLabel.Click += clickEvent;
            textLabel.Click += clickEvent;

            itemPanel.Controls.Add(iconLabel);
            itemPanel.Controls.Add(textLabel);

            menuItemsPanel.Controls.Add(itemPanel);

            // Guardar referencia al panel en el diccionario
            menuPanels[text] = itemPanel;
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
                case UserRole.ADMIN:
                    return "Administrador";
                case UserRole.USER:
                    return "Usuario";
                default:
                    return "Usuario";
            }
        }

        private void ShowDashboard()
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(dashboardControl);
            titleLabel.Text = "Dashboard";
            currentSection = "Dashboard";
            SetActiveButton(currentSection);
            activeControl = dashboardControl;
            dashboardControl.LoadData();
        }

        private void ShowVehicles()
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(vehiclesControl);
            titleLabel.Text = "Gestión de Vehículos";
            currentSection = "Vehículos";
            SetActiveButton(currentSection);
            activeControl = vehiclesControl;
            // Cargamos los datos de vehículos cuando se muestra esta sección
            vehiclesControl.LoadData();
        }

        private void ShowSales()
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(salesControl);
            titleLabel.Text = "Pipeline comercial";
            currentSection = "Tramitación";
            SetActiveButton(currentSection);
            activeControl = salesControl;
            // Cargamos los datos de ventas cuando se muestra esta sección
            salesControl.LoadAllData();
        }

        private void ShowUsers()
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(usersControl);
            titleLabel.Text = "Gestión de Usuarios";
            currentSection = "Usuarios";
            SetActiveButton(currentSection);
            activeControl = usersControl;
            // Cargamos los datos de usuarios cuando se muestra esta sección
            usersControl.LoadData();
        }

        private void ShowSettings()
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(settingsControl);
            titleLabel.Text = "Configuración";
            currentSection = "Configuración";
            SetActiveButton(currentSection);
            activeControl = settingsControl;
            // Cargamos los datos de configuración cuando se muestra esta sección
            settingsControl.LoadData();
        }

        private void SetActiveButton(string buttonText)
        {
            // Restablecer todos los paneles a su estado normal
            foreach (var kvp in menuPanels)
            {
                kvp.Value.BackColor = Color.Transparent;
            }

            // Si encontramos el panel correspondiente al botón activo, lo resaltamos
            if (menuPanels.ContainsKey(buttonText))
            {
                Panel activePanel = menuPanels[buttonText];
                activePanel.BackColor = Color.FromArgb(240, 240, 240);

                // Posicionar el indicador activo
                /*activeIndicator.Location = new Point(0, activePanel.Location.Y);
                activeIndicator.Height = activePanel.Height;
                activeIndicator.Visible = true;
                activeIndicator.BringToFront();*/

                Point absoluteLocation = activePanel.Parent.PointToScreen(activePanel.Location);
                Point relativeLocation = activeIndicator.Parent.PointToClient(absoluteLocation);

                // Posicionar el indicador en la ubicación correcta
                activeIndicator.Location = new Point(0, relativeLocation.Y);
                activeIndicator.Height = activePanel.Height;
                activeIndicator.Visible = true;
                activeIndicator.BringToFront();

            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // Asegurarse de que el indicador esté correctamente posicionado al inicio
            SetActiveButton(currentSection);
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
                // Clear Firebase authentication
                AppConfig.FirebaseAuth?.ClearToken();

                AppConfig.DeleteConfig();
                LoginForm loginForm = new LoginForm();
                this.Hide();
                loginForm.ShowDialog();
                this.Close();
            }
        }
    }
}
