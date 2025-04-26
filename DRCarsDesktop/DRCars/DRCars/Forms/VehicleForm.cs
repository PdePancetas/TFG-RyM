using System;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Controls;
using DRCars.Models;
using DRCars.Utils;

namespace DRCars.Forms
{
    public partial class VehicleForm : Form
    {
        private RoundedPanel mainPanel;
        private Label titleLabel;
        private Label brandLabel;
        private RoundedTextBox brandTextBox;
        private Label modelLabel;
        private RoundedTextBox modelTextBox;
        private Label yearLabel;
        private RoundedTextBox yearTextBox;
        private Label priceLabel;
        private RoundedTextBox priceTextBox;
        private Label categoryLabel;
        private ComboBox categoryComboBox;
        private Label fuelTypeLabel;
        private ComboBox fuelTypeComboBox;
        private Label transmissionLabel;
        private ComboBox transmissionComboBox;
        private Label kilometersLabel;
        private RoundedTextBox kilometersTextBox;
        private Label statusLabel;
        private ComboBox statusComboBox;
        private Label imageUrlLabel;
        private RoundedTextBox imageUrlTextBox;
        private RoundedButton saveButton;
        private RoundedButton cancelButton;

        private Vehicle _vehicle;
        private bool _isEditMode;
        private ApiClient apiClient;

        public VehicleForm(Vehicle vehicle = null)
        {
            _vehicle = vehicle;
            _isEditMode = vehicle != null;
            apiClient = new ApiClient();
            InitializeComponent();
            LoadVehicleData();
        }

        private void InitializeComponent()
        {
            mainPanel = new RoundedPanel();
            titleLabel = new Label();
            brandLabel = new Label();
            brandTextBox = new RoundedTextBox();
            modelLabel = new Label();
            modelTextBox = new RoundedTextBox();
            yearLabel = new Label();
            yearTextBox = new RoundedTextBox();
            priceLabel = new Label();
            priceTextBox = new RoundedTextBox();
            categoryLabel = new Label();
            categoryComboBox = new ComboBox();
            fuelTypeLabel = new Label();
            fuelTypeComboBox = new ComboBox();
            transmissionLabel = new Label();
            transmissionComboBox = new ComboBox();
            kilometersLabel = new Label();
            kilometersTextBox = new RoundedTextBox();
            statusLabel = new Label();
            statusComboBox = new ComboBox();
            imageUrlLabel = new Label();
            imageUrlTextBox = new RoundedTextBox();
            saveButton = new RoundedButton();
            cancelButton = new RoundedButton();

            // Form
            this.Text = _isEditMode ? "Editar Vehículo" : "Añadir Vehículo";
            this.Size = new Size(600, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Main Panel
            mainPanel.BorderRadius = 15;
            mainPanel.BorderColor = Color.FromArgb(220, 220, 220);
            mainPanel.BorderSize = 1;
            mainPanel.Size = new Size(550, 650);
            mainPanel.Location = new Point((this.ClientSize.Width - 550) / 2, (this.ClientSize.Height - 650) / 2);
            mainPanel.BackColor = Color.White;
            mainPanel.Padding = new Padding(30);

            // Title Label
            titleLabel.AutoSize = false;
            titleLabel.Size = new Size(490, 40);
            titleLabel.Location = new Point(30, 20);
            titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Text = _isEditMode ? "Editar Vehículo" : "Añadir Vehículo";

            // Brand Label
            brandLabel.AutoSize = true;
            brandLabel.Location = new Point(30, 80);
            brandLabel.Font = new Font("Segoe UI", 9F);
            brandLabel.Text = "Marca";

            // Brand TextBox
            brandTextBox.Size = new Size(230, 40);
            brandTextBox.Location = new Point(30, 105);
            brandTextBox.BorderRadius = 10;
            brandTextBox.PlaceholderText = "Marca del vehículo";

            // Model Label
            modelLabel.AutoSize = true;
            modelLabel.Location = new Point(290, 80);
            modelLabel.Font = new Font("Segoe UI", 9F);
            modelLabel.Text = "Modelo";

            // Model TextBox
            modelTextBox.Size = new Size(230, 40);
            modelTextBox.Location = new Point(290, 105);
            modelTextBox.BorderRadius = 10;
            modelTextBox.PlaceholderText = "Modelo del vehículo";

            // Year Label
            yearLabel.AutoSize = true;
            yearLabel.Location = new Point(30, 155);
            yearLabel.Font = new Font("Segoe UI", 9F);
            yearLabel.Text = "Año";

            // Year TextBox
            yearTextBox.Size = new Size(230, 40);
            yearTextBox.Location = new Point(30, 180);
            yearTextBox.BorderRadius = 10;
            yearTextBox.PlaceholderText = "Año del vehículo";

            // Price Label
            priceLabel.AutoSize = true;
            priceLabel.Location = new Point(290, 155);
            priceLabel.Font = new Font("Segoe UI", 9F);
            priceLabel.Text = "Precio (€)";

            // Price TextBox
            priceTextBox.Size = new Size(230, 40);
            priceTextBox.Location = new Point(290, 180);
            priceTextBox.BorderRadius = 10;
            priceTextBox.PlaceholderText = "Precio del vehículo";

            // Category Label
            categoryLabel.AutoSize = true;
            categoryLabel.Location = new Point(30, 230);
            categoryLabel.Font = new Font("Segoe UI", 9F);
            categoryLabel.Text = "Categoría";

            // Category ComboBox
            categoryComboBox.Size = new Size(230, 40);
            categoryComboBox.Location = new Point(30, 255);
            categoryComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            categoryComboBox.Font = new Font("Segoe UI", 9F);
            categoryComboBox.Items.AddRange(new object[] { "Lujo", "Estándar", "Deportivo", "SUV", "Compacto" });

            // Fuel Type Label
            fuelTypeLabel.AutoSize = true;
            fuelTypeLabel.Location = new Point(290, 230);
            fuelTypeLabel.Font = new Font("Segoe UI", 9F);
            fuelTypeLabel.Text = "Combustible";

            // Fuel Type ComboBox
            fuelTypeComboBox.Size = new Size(230, 40);
            fuelTypeComboBox.Location = new Point(290, 255);
            fuelTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            fuelTypeComboBox.Font = new Font("Segoe UI", 9F);
            fuelTypeComboBox.Items.AddRange(new object[] { "Gasolina", "Diésel", "Híbrido", "Eléctrico" });

            // Transmission Label
            transmissionLabel.AutoSize = true;
            transmissionLabel.Location = new Point(30, 305);
            transmissionLabel.Font = new Font("Segoe UI", 9F);
            transmissionLabel.Text = "Transmisión";

            // Transmission ComboBox
            transmissionComboBox.Size = new Size(230, 40);
            transmissionComboBox.Location = new Point(30, 330);
            transmissionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            transmissionComboBox.Font = new Font("Segoe UI", 9F);
            transmissionComboBox.Items.AddRange(new object[] { "Automática", "Manual" });

            // Kilometers Label
            kilometersLabel.AutoSize = true;
            kilometersLabel.Location = new Point(290, 305);
            kilometersLabel.Font = new Font("Segoe UI", 9F);
            kilometersLabel.Text = "Kilómetros";

            // Kilometers TextBox
            kilometersTextBox.Size = new Size(230, 40);
            kilometersTextBox.Location = new Point(290, 330);
            kilometersTextBox.BorderRadius = 10;
            kilometersTextBox.PlaceholderText = "Kilómetros del vehículo";

            // Status Label
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(30, 380);
            statusLabel.Font = new Font("Segoe UI", 9F);
            statusLabel.Text = "Estado";

            // Status ComboBox
            statusComboBox.Size = new Size(230, 40);
            statusComboBox.Location = new Point(30, 405);
            statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            statusComboBox.Font = new Font("Segoe UI", 9F);
            statusComboBox.Items.AddRange(new object[] { "En Stock", "En Garaje", "En Venta", "Vendido", "En Reparación" });

            // Image URL Label
            imageUrlLabel.AutoSize = true;
            imageUrlLabel.Location = new Point(30, 455);
            imageUrlLabel.Font = new Font("Segoe UI", 9F);
            imageUrlLabel.Text = "URL de la Imagen";

            // Image URL TextBox
            imageUrlTextBox.Size = new Size(490, 40);
            imageUrlTextBox.Location = new Point(30, 480);
            imageUrlTextBox.BorderRadius = 10;
            imageUrlTextBox.PlaceholderText = "URL de la imagen del vehículo";

            // Save Button
            saveButton.Text = "Guardar";
            saveButton.Size = new Size(230, 45);
            saveButton.Location = new Point(30, 550);
            saveButton.BorderRadius = 10;
            saveButton.BackColor = Color.FromArgb(0, 120, 215);
            saveButton.ForeColor = Color.White;
            saveButton.Click += SaveButton_Click;

            // Cancel Button
            cancelButton.Text = "Cancelar";
            cancelButton.Size = new Size(230, 45);
            cancelButton.Location = new Point(290, 550);
            cancelButton.BorderRadius = 10;
            cancelButton.BackColor = Color.FromArgb(200, 50, 50);
            cancelButton.ForeColor = Color.White;
            cancelButton.Click += CancelButton_Click;

            // Add controls to panel
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(brandLabel);
            mainPanel.Controls.Add(brandTextBox);
            mainPanel.Controls.Add(modelLabel);
            mainPanel.Controls.Add(modelTextBox);
            mainPanel.Controls.Add(yearLabel);
            mainPanel.Controls.Add(yearTextBox);
            mainPanel.Controls.Add(priceLabel);
            mainPanel.Controls.Add(priceTextBox);
            mainPanel.Controls.Add(categoryLabel);
            mainPanel.Controls.Add(categoryComboBox);
            mainPanel.Controls.Add(fuelTypeLabel);
            mainPanel.Controls.Add(fuelTypeComboBox);
            mainPanel.Controls.Add(transmissionLabel);
            mainPanel.Controls.Add(transmissionComboBox);
            mainPanel.Controls.Add(kilometersLabel);
            mainPanel.Controls.Add(kilometersTextBox);
            mainPanel.Controls.Add(statusLabel);
            mainPanel.Controls.Add(statusComboBox);
            mainPanel.Controls.Add(imageUrlLabel);
            mainPanel.Controls.Add(imageUrlTextBox);
            mainPanel.Controls.Add(saveButton);
            mainPanel.Controls.Add(cancelButton);

            // Add panel to form
            this.Controls.Add(mainPanel);
        }

        private void LoadVehicleData()
        {
            if (_isEditMode && _vehicle != null)
            {
                brandTextBox.Texts = _vehicle.Brand;
                modelTextBox.Texts = _vehicle.Model;
                yearTextBox.Texts = _vehicle.Year.ToString();
                priceTextBox.Texts = _vehicle.Price.ToString();
                categoryComboBox.SelectedItem = _vehicle.Category;
                fuelTypeComboBox.SelectedItem = _vehicle.FuelType;
                transmissionComboBox.SelectedItem = _vehicle.Transmission;
                kilometersTextBox.Texts = _vehicle.Kilometers.ToString();
                statusComboBox.SelectedIndex = (int)_vehicle.Status;
                imageUrlTextBox.Texts = _vehicle.ImageUrl;
            }
            else
            {
                // Set defaults for new vehicle
                categoryComboBox.SelectedIndex = 0;
                fuelTypeComboBox.SelectedIndex = 0;
                transmissionComboBox.SelectedIndex = 0;
                statusComboBox.SelectedIndex = 0;
            }
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrEmpty(brandTextBox.Texts) ||
                    string.IsNullOrEmpty(modelTextBox.Texts) ||
                    string.IsNullOrEmpty(yearTextBox.Texts) ||
                    string.IsNullOrEmpty(priceTextBox.Texts) ||
                    string.IsNullOrEmpty(kilometersTextBox.Texts))
                {
                    MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Parse numeric values
                if (!int.TryParse(yearTextBox.Texts, out int year) ||
                    !decimal.TryParse(priceTextBox.Texts, out decimal price) ||
                    !int.TryParse(kilometersTextBox.Texts, out int kilometers))
                {
                    MessageBox.Show("Por favor, ingrese valores numéricos válidos para Año, Precio y Kilómetros.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create or update vehicle
                if (_isEditMode)
                {
                    // Update existing vehicle
                    _vehicle.Brand = brandTextBox.Texts;
                    _vehicle.Model = modelTextBox.Texts;
                    _vehicle.Year = year;
                    _vehicle.Price = price;
                    _vehicle.Category = categoryComboBox.SelectedItem.ToString();
                    _vehicle.FuelType = fuelTypeComboBox.SelectedItem.ToString();
                    _vehicle.Transmission = transmissionComboBox.SelectedItem.ToString();
                    _vehicle.Kilometers = kilometers;
                    _vehicle.Status = (VehicleStatus)statusComboBox.SelectedIndex;
                    _vehicle.ImageUrl = imageUrlTextBox.Texts;
                    _vehicle.UpdatedAt = DateTime.Now;

                    await apiClient.UpdateVehicleAsync(_vehicle);
                }
                else
                {
                    // Create new vehicle
                    Vehicle newVehicle = new Vehicle
                    {
                        Brand = brandTextBox.Texts,
                        Model = modelTextBox.Texts,
                        Year = year,
                        Price = price,
                        Category = categoryComboBox.SelectedItem.ToString(),
                        FuelType = fuelTypeComboBox.SelectedItem.ToString(),
                        Transmission = transmissionComboBox.SelectedItem.ToString(),
                        Kilometers = kilometers,
                        Status = (VehicleStatus)statusComboBox.SelectedIndex,
                        ImageUrl = imageUrlTextBox.Texts,
                        CreatedAt = DateTime.Now
                    };

                    await apiClient.AddVehicleAsync(newVehicle);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el vehículo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
