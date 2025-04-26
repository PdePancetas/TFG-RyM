using System;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Models;

namespace DRCars.Controls
{
    public class VehicleCard : RoundedPanel
    {
        private Vehicle _vehicle;
        private PictureBox vehicleImage;
        private Label brandModelLabel;
        private Label yearLabel;
        private Label priceLabel;
        private Label detailsLabel;
        private Label statusLabel;
        private RoundedButton viewDetailsButton;

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
            vehicleImage = new PictureBox();
            brandModelLabel = new Label();
            yearLabel = new Label();
            priceLabel = new Label();
            detailsLabel = new Label();
            statusLabel = new Label();
            viewDetailsButton = new RoundedButton();

            // Vehicle Card
            this.Size = new Size(300, 400);
            this.BorderRadius = 15;
            this.BorderColor = Color.FromArgb(220, 220, 220);
            this.BorderSize = 1;
            this.BackColor = Color.White;
            this.Padding = new Padding(10);

            // Vehicle Image
            vehicleImage.Size = new Size(280, 180);
            vehicleImage.Location = new Point(10, 10);
            vehicleImage.SizeMode = PictureBoxSizeMode.Zoom;
            vehicleImage.BackColor = Color.FromArgb(245, 245, 245);

            // Brand Model Label
            brandModelLabel.AutoSize = false;
            brandModelLabel.Size = new Size(280, 30);
            brandModelLabel.Location = new Point(10, 200);
            brandModelLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            brandModelLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Year Label
            yearLabel.AutoSize = false;
            yearLabel.Size = new Size(280, 20);
            yearLabel.Location = new Point(10, 230);
            yearLabel.Font = new Font("Segoe UI", 9F);
            yearLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Price Label
            priceLabel.AutoSize = false;
            priceLabel.Size = new Size(280, 30);
            priceLabel.Location = new Point(10, 250);
            priceLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            priceLabel.TextAlign = ContentAlignment.MiddleLeft;
            priceLabel.ForeColor = Color.FromArgb(0, 120, 215);

            // Details Label
            detailsLabel.AutoSize = false;
            detailsLabel.Size = new Size(280, 40);
            detailsLabel.Location = new Point(10, 280);
            detailsLabel.Font = new Font("Segoe UI", 9F);
            detailsLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Status Label
            statusLabel.AutoSize = false;
            statusLabel.Size = new Size(100, 25);
            statusLabel.Location = new Point(10, 320);
            statusLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            statusLabel.BackColor = Color.FromArgb(240, 240, 240);
            statusLabel.BorderStyle = BorderStyle.FixedSingle;

            // View Details Button
            viewDetailsButton.Text = "Ver Detalles";
            viewDetailsButton.Size = new Size(150, 35);
            viewDetailsButton.Location = new Point(140, 320);
            viewDetailsButton.BorderRadius = 10;
            viewDetailsButton.BackColor = Color.FromArgb(50, 50, 50);
            viewDetailsButton.ForeColor = Color.White;
            viewDetailsButton.Click += ViewDetailsButton_Click;

            // Add controls to panel
            this.Controls.Add(vehicleImage);
            this.Controls.Add(brandModelLabel);
            this.Controls.Add(yearLabel);
            this.Controls.Add(priceLabel);
            this.Controls.Add(detailsLabel);
            this.Controls.Add(statusLabel);
            this.Controls.Add(viewDetailsButton);
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
                    return Color.FromArgb(230, 255, 230);
                case VehicleStatus.InGarage:
                    return Color.FromArgb(230, 230, 255);
                case VehicleStatus.ForSale:
                    return Color.FromArgb(255, 255, 230);
                case VehicleStatus.Sold:
                    return Color.FromArgb(255, 230, 230);
                case VehicleStatus.InRepair:
                    return Color.FromArgb(255, 240, 220);
                default:
                    return Color.FromArgb(240, 240, 240);
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
