using System;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Models;

namespace DRCars.Controls
{
    public class VehicleCard : UserControl
    {
        private Vehicle _vehicle;
        private RoundedPanel mainPanel;
        private PictureBox vehicleImage;
        private Label brandModelLabel;
        private Label yearLabel;
        private Label priceLabel;
        private Label detailsLabel;
        private Label statusLabel;
        private RoundedButton viewDetailsButton;

        // Colores de Odoo
        private Color primaryColor = Color.FromArgb(0, 160, 157); // Verde Odoo
        private Color secondaryColor = Color.FromArgb(242, 242, 242); // Gris claro
        private Color textColor = Color.FromArgb(51, 51, 51); // Texto oscuro
        private Color accentColor = Color.FromArgb(108, 117, 125); // Gris para detalles

        public event EventHandler<Vehicle> ViewDetailsClicked;

        public Vehicle Vehicle
        {
            get { return _vehicle; }
            set
            {
                _vehicle = value;
                UpdateCardInfo();
            }
        }

        public VehicleCard()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            mainPanel = new RoundedPanel();
            vehicleImage = new PictureBox();
            brandModelLabel = new Label();
            yearLabel = new Label();
            priceLabel = new Label();
            detailsLabel = new Label();
            statusLabel = new Label();
            viewDetailsButton = new RoundedButton();

            // Main Panel
            mainPanel.BorderRadius = 8;
            mainPanel.BorderColor = Color.FromArgb(230, 230, 230);
            mainPanel.BorderSize = 1;
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.White;
            mainPanel.Padding = new Padding(0);

            // Vehicle Image
            vehicleImage.Size = new Size(300, 180);
            vehicleImage.Location = new Point(0, 0);
            vehicleImage.SizeMode = PictureBoxSizeMode.CenterImage;
            vehicleImage.BackColor = Color.FromArgb(245, 245, 245);
            vehicleImage.Dock = DockStyle.Top;

            // Brand Model Label
            brandModelLabel.AutoSize = false;
            brandModelLabel.Size = new Size(280, 30);
            brandModelLabel.Location = new Point(15, 195);
            brandModelLabel.Font = new Font("Segoe UI Semibold", 12F);
            brandModelLabel.ForeColor = textColor;
            brandModelLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Year Label
            yearLabel.AutoSize = false;
            yearLabel.Size = new Size(280, 20);
            yearLabel.Location = new Point(15, 225);
            yearLabel.Font = new Font("Segoe UI", 9F);
            yearLabel.ForeColor = accentColor;
            yearLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Price Label
            priceLabel.AutoSize = false;
            priceLabel.Size = new Size(280, 30);
            priceLabel.Location = new Point(15, 245);
            priceLabel.Font = new Font("Segoe UI Semibold", 12F);
            priceLabel.ForeColor = primaryColor;
            priceLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Details Label
            detailsLabel.AutoSize = false;
            detailsLabel.Size = new Size(280, 40);
            detailsLabel.Location = new Point(15, 275);
            detailsLabel.Font = new Font("Segoe UI", 9F);
            detailsLabel.ForeColor = accentColor;
            detailsLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Status Label
            statusLabel.AutoSize = false;
            statusLabel.Size = new Size(100, 25);
            statusLabel.Location = new Point(15, 320);
            statusLabel.Font = new Font("Segoe UI", 9F);
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            statusLabel.BackColor = Color.FromArgb(240, 240, 240);
            statusLabel.BorderStyle = BorderStyle.None;

            // View Details Button
            viewDetailsButton.Text = "Ver Detalles";
            viewDetailsButton.Size = new Size(150, 35);
            viewDetailsButton.Location = new Point(135, 320);
            viewDetailsButton.BorderRadius = 4;
            viewDetailsButton.BackColor = primaryColor;
            viewDetailsButton.ForeColor = Color.White;
            viewDetailsButton.Click += ViewDetailsButton_Click;

            // Add controls to panel
            mainPanel.Controls.Add(vehicleImage);
            mainPanel.Controls.Add(brandModelLabel);
            mainPanel.Controls.Add(yearLabel);
            mainPanel.Controls.Add(priceLabel);
            mainPanel.Controls.Add(detailsLabel);
            mainPanel.Controls.Add(statusLabel);
            mainPanel.Controls.Add(viewDetailsButton);

            // Add panel to control
            this.Controls.Add(mainPanel);
            this.Size = new Size(300, 380);
        }

        private void UpdateCardInfo()
        {
            if (_vehicle != null)
            {
                brandModelLabel.Text = $"{_vehicle.Brand} {_vehicle.Model}";
                yearLabel.Text = $"{_vehicle.Year}";
                priceLabel.Text = $"{_vehicle.Price:N0} €";
                detailsLabel.Text = $"{_vehicle.FuelType} • {_vehicle.Transmission} • {_vehicle.Kilometers:N0} km";

                // Set status label
                statusLabel.Text = GetStatusText(_vehicle.Status);
                statusLabel.BackColor = GetStatusColor(_vehicle.Status);

                // Load image if available
                if (!string.IsNullOrEmpty(_vehicle.ImageUrl))
                {
                    try
                    {
                        // In a real app, you would load the image from the URL
                        // For demo purposes, use a placeholder
                        vehicleImage.Image = null;
                    }
                    catch
                    {
                        // If image loading fails, use a placeholder
                        vehicleImage.Image = null;
                    }
                }
                else
                {
                    vehicleImage.Image = null;
                }
            }
        }

        private string GetStatusText(VehicleStatus status)
        {
            switch (status)
            {
                case VehicleStatus.InStock:
                    return "En Stock";
                case VehicleStatus.InGarage:
                    return "En Garaje";
                case VehicleStatus.ForSale:
                    return "En Venta";
                case VehicleStatus.Sold:
                    return "Vendido";
                case VehicleStatus.InRepair:
                    return "En Reparación";
                default:
                    return "Desconocido";
            }
        }

        private Color GetStatusColor(VehicleStatus status)
        {
            switch (status)
            {
                case VehicleStatus.InStock:
                    return Color.FromArgb(209, 231, 221); // Verde claro
                case VehicleStatus.InGarage:
                    return Color.FromArgb(207, 226, 255); // Azul claro
                case VehicleStatus.ForSale:
                    return Color.FromArgb(255, 243, 205); // Amarillo claro
                case VehicleStatus.Sold:
                    return Color.FromArgb(248, 215, 218); // Rojo claro
                case VehicleStatus.InRepair:
                    return Color.FromArgb(255, 228, 208); // Naranja claro
                default:
                    return Color.FromArgb(240, 240, 240); // Gris claro
            }
        }

        private void ViewDetailsButton_Click(object sender, EventArgs e)
        {
            if (_vehicle != null && ViewDetailsClicked != null)
            {
                ViewDetailsClicked(this, _vehicle);
            }
        }
    }
}
