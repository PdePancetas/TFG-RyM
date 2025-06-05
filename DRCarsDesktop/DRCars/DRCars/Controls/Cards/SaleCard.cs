using DRCars.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DRCars.Controls.Cards
{
    public class SaleCard : UserControl
    {
        private Sale _sale;
        private RoundedPanel mainPanel;
        private Label customerNameLabel;
        private Label vehicleLabel;
        private Label dateLabel;

        // Colores de Odoo
        private Color primaryColor = Color.FromArgb(0, 160, 157); // Verde Odoo
        private Color secondaryColor = Color.FromArgb(242, 242, 242); // Gris claro
        private Color textColor = Color.FromArgb(51, 51, 51); // Texto oscuro
        private Color accentColor = Color.FromArgb(108, 117, 125); // Gris para detalles

        public Sale Sale
        {
            get { return _sale; }
            set
            {
                _sale = value;
                UpdateCardInfo();
            }
        }

        public SaleCard()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            mainPanel = new RoundedPanel();
            customerNameLabel = new Label();
            vehicleLabel = new Label();
            dateLabel = new Label();

            // Main Panel
            mainPanel.BorderRadius = 8;
            mainPanel.BorderColor = Color.FromArgb(230, 230, 230);
            mainPanel.BorderSize = 1;
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.White;
            mainPanel.Padding = new Padding(15);

            // Customer Name Label
            customerNameLabel.AutoSize = false;
            customerNameLabel.Size = new Size(270, 30);
            customerNameLabel.Location = new Point(15, 15);
            customerNameLabel.Font = new Font("Segoe UI Semibold", 12F);
            customerNameLabel.ForeColor = textColor;
            customerNameLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Vehicle Label
            vehicleLabel.AutoSize = false;
            vehicleLabel.Size = new Size(270, 20);
            vehicleLabel.Location = new Point(15, 45);
            vehicleLabel.Font = new Font("Segoe UI", 9F);
            vehicleLabel.ForeColor = accentColor;
            vehicleLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Date Label
            dateLabel.AutoSize = false;
            dateLabel.Size = new Size(270, 20);
            dateLabel.Location = new Point(15, 65);
            dateLabel.Font = new Font("Segoe UI", 9F);
            dateLabel.ForeColor = accentColor;
            dateLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Add controls to panel
            mainPanel.Controls.Add(customerNameLabel);
            mainPanel.Controls.Add(vehicleLabel);
            mainPanel.Controls.Add(dateLabel);

            // Add panel to control
            this.Controls.Add(mainPanel);
            this.Size = new Size(420, 180);
        }

        public void UpdateCardInfo()
        {
            if (_sale != null)
            {
                customerNameLabel.Text = _sale.client.Name + " " + _sale.client.Surname;

                if (_sale.Vehicle != null)
                {
                    vehicleLabel.Text = $"{_sale.Vehicle.Brand} {_sale.Vehicle.Model} ({_sale.Vehicle.Year})";
                }
                else
                {
                    vehicleLabel.Text = "Sin vehiculo asociado";
                }

                dateLabel.Text = $"Reserva: {_sale.SaleDate.ToShortDateString()} - Precio: {_sale.SalePrice} €";


            }
        }
    }
}
