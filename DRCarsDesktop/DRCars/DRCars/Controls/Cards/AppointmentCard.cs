using System;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Models;

namespace DRCars.Controls
{
    public class AppointmentCard : UserControl
    {
        private Appointment _appointment;
        private RoundedPanel mainPanel;
        public Label customerNameLabel;
        public Label vehicleLabel;
        private Label dateLabel;
        private RoundedButton completeButton;
        private RoundedButton cancelButton;

        // Colores de Odoo
        private Color primaryColor = Color.FromArgb(0, 160, 157); // Verde Odoo
        private Color secondaryColor = Color.FromArgb(242, 242, 242); // Gris claro
        private Color textColor = Color.FromArgb(51, 51, 51); // Texto oscuro
        private Color accentColor = Color.FromArgb(108, 117, 125); // Gris para detalles

        
        public event EventHandler<Appointment> CompleteClicked;
        public event EventHandler<Appointment> CancelClicked;
        public event EventHandler<Appointment> ProcessClicked;

        public Appointment Appointment
        {
            get { return _appointment; }
            set
            {
                _appointment = value;
                UpdateCardInfo();
            }
        }

        public AppointmentCard()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            mainPanel = new RoundedPanel();
            customerNameLabel = new Label();
            vehicleLabel = new Label();
            dateLabel = new Label();
            completeButton = new RoundedButton();
            cancelButton = new RoundedButton();

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

            // Complete Button
            completeButton.Text = "Completar";
            completeButton.Size = new Size(90, 35);
            completeButton.Location = new Point(215, 130);
            completeButton.BorderRadius = 4;
            completeButton.BackColor = Color.FromArgb(40, 167, 69);
            completeButton.ForeColor = Color.White;
            completeButton.Click += AppointmentCompleteButton_Click;

            // Cancel Button
            cancelButton.Text = "Cancelar";
            cancelButton.Size = new Size(90, 35);
            cancelButton.Location = new Point(315, 130);
            cancelButton.BorderRadius = 4;
            cancelButton.BackColor = Color.FromArgb(220, 53, 69);
            cancelButton.ForeColor = Color.White;
            cancelButton.Click += AppointmentCancelButton_Click;

            // Add controls to panel
            mainPanel.Controls.Add(customerNameLabel);
            mainPanel.Controls.Add(vehicleLabel);
            mainPanel.Controls.Add(dateLabel);
            mainPanel.Controls.Add(completeButton);
            mainPanel.Controls.Add(cancelButton);

            // Add panel to control
            this.Controls.Add(mainPanel);
            this.Size = new Size(420, 180);
        }

        public void UpdateCardInfo()
        {
            if (_appointment != null)
            {
                customerNameLabel.Text = _appointment.client.Name+ " " + _appointment.client.Surname;

                if (_appointment.Vehicle != null)
                {
                    vehicleLabel.Text = $"{_appointment.Vehicle.Brand} {_appointment.Vehicle.Model} ({_appointment.Vehicle.Year})";
                }
                else
                {
                    vehicleLabel.Text = "Sin vehiculo asociado";
                }

                dateLabel.Text = $"Reserva: {_appointment.AppointmentDate.ToShortDateString()} - Precio: {_appointment.AppointmentPrice} €";
                

            }
        }

        private void AppointmentCompleteButton_Click(object sender, EventArgs e)
        {
            if (_appointment != null && CompleteClicked != null)
            {
                CompleteClicked(this, _appointment);
            }
        }

        private void AppointmentCancelButton_Click(object sender, EventArgs e)
        {
            if (_appointment != null && CancelClicked != null)
            {
                CancelClicked(this, _appointment);
            }
        }
    }
}
