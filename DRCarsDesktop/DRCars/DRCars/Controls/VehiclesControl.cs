using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Models;
using DRCars.Utils;
using DRCars.Forms;

namespace DRCars.Controls
{
    public class VehiclesControl : UserControl
    {
        private RoundedPanel headerPanel;
        private RoundedPanel filtersPanel;
        private FlowLayoutPanel vehiclesPanel;
        private RoundedButton addVehicleButton;
        private RoundedButton refreshButton;
        private RoundedTextBox searchTextBox;
        private ComboBox brandComboBox;
        private ComboBox modelComboBox;
        private ComboBox yearComboBox;
        private ComboBox statusComboBox;
        private Label filterLabel;
        private Label brandLabel;
        private Label modelLabel;
        private Label yearLabel;
        private Label statusLabel;
        private RoundedButton applyFiltersButton;
        private RoundedButton clearFiltersButton;

        private ApiClient apiClient;
        private List<Vehicle> allVehicles;
        private List<Vehicle> filteredVehicles;

        public VehiclesControl()
        {
            apiClient = new ApiClient();
            InitializeComponent();
            // Eliminamos la carga automática: LoadVehicles();
        }

        public void LoadData()
        {
            LoadVehicles();
        }

        private void InitializeComponent()
        {
            headerPanel = new RoundedPanel();
            filtersPanel = new RoundedPanel();
            vehiclesPanel = new FlowLayoutPanel();
            addVehicleButton = new RoundedButton();
            refreshButton = new RoundedButton();
            searchTextBox = new RoundedTextBox();
            brandComboBox = new ComboBox();
            modelComboBox = new ComboBox();
            yearComboBox = new ComboBox();
            statusComboBox = new ComboBox();
            filterLabel = new Label();
            brandLabel = new Label();
            modelLabel = new Label();
            yearLabel = new Label();
            statusLabel = new Label();
            applyFiltersButton = new RoundedButton();
            clearFiltersButton = new RoundedButton();

            // Header Panel
            headerPanel.BorderRadius = 15;
            headerPanel.BorderColor = Color.FromArgb(220, 220, 220);
            headerPanel.BorderSize = 1;
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 70;
            headerPanel.Padding = new Padding(15);
            headerPanel.Margin = new Padding(0, 0, 0, 20);

            // Search TextBox
            searchTextBox.PlaceholderText = "Buscar por marca, modelo...";
            searchTextBox.Size = new Size(300, 40);
            searchTextBox.Location = new Point(15, 15);
            searchTextBox.BorderRadius = 10;
            searchTextBox.TextChanged += SearchTextBox_TextChanged;

            // Add Vehicle Button
            addVehicleButton.Text = "Añadir Vehículo";
            addVehicleButton.Size = new Size(150, 40);
            addVehicleButton.Location = new Point(headerPanel.Width - 330, 15);
            addVehicleButton.BorderRadius = 10;
            addVehicleButton.BackColor = Color.FromArgb(0, 120, 215);
            addVehicleButton.ForeColor = Color.White;
            addVehicleButton.Click += AddVehicleButton_Click;

            // Refresh Button
            refreshButton.Text = "Actualizar";
            refreshButton.Size = new Size(150, 40);
            refreshButton.Location = new Point(headerPanel.Width - 170, 15);
            refreshButton.BorderRadius = 10;
            refreshButton.BackColor = Color.FromArgb(50, 50, 50);
            refreshButton.ForeColor = Color.White;
            refreshButton.Click += RefreshButton_Click;

            // Filters Panel
            filtersPanel.BorderRadius = 15;
            filtersPanel.BorderColor = Color.FromArgb(220, 220, 220);
            filtersPanel.BorderSize = 1;
            filtersPanel.Dock = DockStyle.Top;
            filtersPanel.Height = 120;
            filtersPanel.Padding = new Padding(15);
            filtersPanel.Margin = new Padding(0, 0, 0, 20);

            // Filter Label
            filterLabel.AutoSize = true;
            filterLabel.Location = new Point(15, 15);
            filterLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            filterLabel.Text = "Filtros";

            // Brand Label
            brandLabel.AutoSize = true;
            brandLabel.Location = new Point(15, 45);
            brandLabel.Font = new Font("Segoe UI", 9F);
            brandLabel.Text = "Marca";

            // Brand ComboBox
            brandComboBox.Size = new Size(150, 30);
            brandComboBox.Location = new Point(15, 70);
            brandComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            brandComboBox.Font = new Font("Segoe UI", 9F);

            // Model Label
            modelLabel.AutoSize = true;
            modelLabel.Location = new Point(180, 45);
            modelLabel.Font = new Font("Segoe UI", 9F);
            modelLabel.Text = "Modelo";

            // Model ComboBox
            modelComboBox.Size = new Size(150, 30);
            modelComboBox.Location = new Point(180, 70);
            modelComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            modelComboBox.Font = new Font("Segoe UI", 9F);

            // Year Label
            yearLabel.AutoSize = true;
            yearLabel.Location = new Point(345, 45);
            yearLabel.Font = new Font("Segoe UI", 9F);
            yearLabel.Text = "Año";

            // Year ComboBox
            yearComboBox.Size = new Size(100, 30);
            yearComboBox.Location = new Point(345, 70);
            yearComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            yearComboBox.Font = new Font("Segoe UI", 9F);

            // Status Label
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(460, 45);
            statusLabel.Font = new Font("Segoe UI", 9F);
            statusLabel.Text = "Estado";

            // Status ComboBox
            statusComboBox.Size = new Size(150, 30);
            statusComboBox.Location = new Point(460, 70);
            statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            statusComboBox.Font = new Font("Segoe UI", 9F);
            statusComboBox.Items.Add("Todos");
            statusComboBox.Items.Add("En Stock");
            statusComboBox.Items.Add("En Garaje");
            statusComboBox.Items.Add("En Venta");
            statusComboBox.Items.Add("Vendido");
            statusComboBox.Items.Add("En Reparación");
            statusComboBox.SelectedIndex = 0;

            // Apply Filters Button
            applyFiltersButton.Text = "Aplicar Filtros";
            applyFiltersButton.Size = new Size(120, 35);
            applyFiltersButton.Location = new Point(625, 65);
            applyFiltersButton.BorderRadius = 10;
            applyFiltersButton.BackColor = Color.FromArgb(0, 120, 215);
            applyFiltersButton.ForeColor = Color.White;
            applyFiltersButton.Click += ApplyFiltersButton_Click;

            // Clear Filters Button
            clearFiltersButton.Text = "Limpiar";
            clearFiltersButton.Size = new Size(100, 35);
            clearFiltersButton.Location = new Point(755, 65);
            clearFiltersButton.BorderRadius = 10;
            clearFiltersButton.BackColor = Color.FromArgb(200, 50, 50);
            clearFiltersButton.ForeColor = Color.White;
            clearFiltersButton.Click += ClearFiltersButton_Click;

            // Vehicles Panel
            vehiclesPanel.Dock = DockStyle.Fill;
            vehiclesPanel.AutoScroll = true;
            vehiclesPanel.Padding = new Padding(5);

            // Add controls to panels
            headerPanel.Controls.Add(searchTextBox);
            headerPanel.Controls.Add(addVehicleButton);
            headerPanel.Controls.Add(refreshButton);

            filtersPanel.Controls.Add(filterLabel);
            filtersPanel.Controls.Add(brandLabel);
            filtersPanel.Controls.Add(brandComboBox);
            filtersPanel.Controls.Add(modelLabel);
            filtersPanel.Controls.Add(modelComboBox);
            filtersPanel.Controls.Add(yearLabel);
            filtersPanel.Controls.Add(yearComboBox);
            filtersPanel.Controls.Add(statusLabel);
            filtersPanel.Controls.Add(statusComboBox);
            filtersPanel.Controls.Add(applyFiltersButton);
            filtersPanel.Controls.Add(clearFiltersButton);

            // Add panels to control
            this.Controls.Add(vehiclesPanel);
            this.Controls.Add(filtersPanel);
            this.Controls.Add(headerPanel);

            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 245, 245);

            // Handle resize event to reposition buttons
            this.Resize += VehiclesControl_Resize;
        }

        private void VehiclesControl_Resize(object sender, EventArgs e)
        {
            // Reposition buttons when control is resized
            addVehicleButton.Location = new Point(headerPanel.Width - 330, 15);
            refreshButton.Location = new Point(headerPanel.Width - 170, 15);
        }

        private async void LoadVehicles()
        {
            try
            {
                allVehicles = await apiClient.GetVehiclesAsync();
                filteredVehicles = new List<Vehicle>(allVehicles);

                PopulateVehicleCards();
                PopulateFilterDropdowns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar vehículos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateVehicleCards()
        {
            vehiclesPanel.Controls.Clear();

            foreach (var vehicle in filteredVehicles)
            {
                VehicleCard card = new VehicleCard
                {
                    Vehicle = vehicle,
                    Margin = new Padding(5),
                    Size = new Size(300, 380)
                };

                card.ViewDetailsClicked += VehicleCard_ViewDetailsClicked;
                vehiclesPanel.Controls.Add(card);
            }
        }

        private void PopulateFilterDropdowns()
        {
            // Clear existing items
            brandComboBox.Items.Clear();
            modelComboBox.Items.Clear();
            yearComboBox.Items.Clear();

            // Add default "All" option
            brandComboBox.Items.Add("Todas");
            modelComboBox.Items.Add("Todos");
            yearComboBox.Items.Add("Todos");

            // Get unique brands, models, and years
            HashSet<string> brands = new HashSet<string>();
            HashSet<string> models = new HashSet<string>();
            HashSet<int> years = new HashSet<int>();

            foreach (var vehicle in allVehicles)
            {
                brands.Add(vehicle.Brand);
                models.Add(vehicle.Model);
                years.Add(vehicle.Year);
            }

            // Add to dropdowns
            foreach (var brand in brands)
            {
                brandComboBox.Items.Add(brand);
            }

            foreach (var model in models)
            {
                modelComboBox.Items.Add(model);
            }

            // Sort years in descending order
            List<int> sortedYears = new List<int>(years);
            sortedYears.Sort();
            sortedYears.Reverse();

            foreach (var year in sortedYears)
            {
                yearComboBox.Items.Add(year.ToString());
            }

            // Select default options
            brandComboBox.SelectedIndex = 0;
            modelComboBox.SelectedIndex = 0;
            yearComboBox.SelectedIndex = 0;
        }

        private void ApplyFilters()
        {
            filteredVehicles = new List<Vehicle>();

            string searchText = searchTextBox.Texts.ToLower();
            string selectedBrand = brandComboBox.SelectedItem?.ToString();
            string selectedModel = modelComboBox.SelectedItem?.ToString();
            string selectedYear = yearComboBox.SelectedItem?.ToString();
            string selectedStatus = statusComboBox.SelectedItem?.ToString();

            foreach (var vehicle in allVehicles)
            {
                bool matchesSearch = string.IsNullOrEmpty(searchText) ||
                                    vehicle.Brand.ToLower().Contains(searchText) ||
                                    vehicle.Model.ToLower().Contains(searchText);

                bool matchesBrand = selectedBrand == "Todas" || vehicle.Brand == selectedBrand;
                bool matchesModel = selectedModel == "Todos" || vehicle.Model == selectedModel;
                bool matchesYear = selectedYear == "Todos" || vehicle.Year.ToString() == selectedYear;

                bool matchesStatus = selectedStatus == "Todos" ||
                                    (selectedStatus == "En Stock" && vehicle.Status == VehicleStatus.InStock) ||
                                    (selectedStatus == "En Garaje" && vehicle.Status == VehicleStatus.InGarage) ||
                                    (selectedStatus == "En Venta" && vehicle.Status == VehicleStatus.ForSale) ||
                                    (selectedStatus == "Vendido" && vehicle.Status == VehicleStatus.Sold) ||
                                    (selectedStatus == "En Reparación" && vehicle.Status == VehicleStatus.InRepair);

                if (matchesSearch && matchesBrand && matchesModel && matchesYear && matchesStatus)
                {
                    filteredVehicles.Add(vehicle);
                }
            }

            PopulateVehicleCards();
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFiltersButton_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ClearFiltersButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Texts = "";
            brandComboBox.SelectedIndex = 0;
            modelComboBox.SelectedIndex = 0;
            yearComboBox.SelectedIndex = 0;
            statusComboBox.SelectedIndex = 0;

            filteredVehicles = new List<Vehicle>(allVehicles);
            PopulateVehicleCards();
        }

        private void AddVehicleButton_Click(object sender, EventArgs e)
        {
            // Open add vehicle form
            VehicleForm vehicleForm = new VehicleForm();
            if (vehicleForm.ShowDialog() == DialogResult.OK)
            {
                LoadVehicles();
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadVehicles();
        }

        private void VehicleCard_ViewDetailsClicked(object sender, Vehicle vehicle)
        {
            // Open vehicle details form
            VehicleForm vehicleForm = new VehicleForm(vehicle);
            if (vehicleForm.ShowDialog() == DialogResult.OK)
            {
                LoadVehicles();
            }
        }
    }
}
