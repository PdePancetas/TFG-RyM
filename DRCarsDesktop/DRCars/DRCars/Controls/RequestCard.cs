using System;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Models;

namespace DRCars.Controls
{
    public class RequestCard : UserControl
    {
        private SaleRequest _request;
        private RoundedPanel mainPanel;
        private Label customerNameLabel;
        private Label vehicleLabel;
        private Label dateLabel;
        private Label statusLabel;
        private RoundedButton viewDetailsButton;
        private RoundedButton scheduleButton;
        private RoundedButton completeButton;
        private RoundedButton cancelButton;

        // Colores de Odoo
        private Color primaryColor = Color.FromArgb(0, 160, 157); // Verde Odoo
        private Color secondaryColor = Color.FromArgb(242, 242, 242); // Gris claro
        private Color textColor = Color.FromArgb(51, 51, 51); // Texto oscuro
        private Color accentColor = Color.FromArgb(108, 117, 125); // Gris para detalles

        public event EventHandler<SaleRequest> ViewDetailsClicked;
        public event EventHandler<SaleRequest> ScheduleClicked;
        public event EventHandler<SaleRequest> CompleteClicked;
        public event EventHandler<SaleRequest> CancelClicked;
        public event EventHandler<SaleRequest> ProcessClicked;

        public SaleRequest Request
        {
            get { return _request; }
            set
            {
                _request = value;
                UpdateCardInfo();
            }
        }

        public RequestCard()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            mainPanel = new RoundedPanel();
            customerNameLabel = new Label();
            vehicleLabel = new Label();
            dateLabel = new Label();
            statusLabel = new Label();
            viewDetailsButton = new RoundedButton();
            scheduleButton = new RoundedButton();
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

            // Status Label
            statusLabel.AutoSize = false;
            statusLabel.Size = new Size(100, 25);
            statusLabel.Location = new Point(15, 95);
            statusLabel.Font = new Font("Segoe UI", 9F);
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            statusLabel.BackColor = Color.FromArgb(240, 240, 240);
            statusLabel.BorderStyle = BorderStyle.None;

            // View Details Button
            viewDetailsButton.Text = "Ver Detalles";
            viewDetailsButton.Size = new Size(90, 35);
            viewDetailsButton.Location = new Point(15, 130);
            viewDetailsButton.BorderRadius = 4;
            viewDetailsButton.BackColor = Color.FromArgb(108, 117, 125);
            viewDetailsButton.ForeColor = Color.White;
            viewDetailsButton.Click += ViewDetailsButton_Click;

            // Schedule Button
            scheduleButton.Text = "Programar";
            scheduleButton.Size = new Size(90, 35);
            scheduleButton.Location = new Point(115, 130);
            scheduleButton.BorderRadius = 4;
            scheduleButton.BackColor = primaryColor;
            scheduleButton.ForeColor = Color.White;
            scheduleButton.Click += ScheduleButton_Click;

            // Complete Button
            completeButton.Text = "Completar";
            completeButton.Size = new Size(90, 35);
            completeButton.Location = new Point(215, 130);
            completeButton.BorderRadius = 4;
            completeButton.BackColor = Color.FromArgb(40, 167, 69);
            completeButton.ForeColor = Color.White;
            completeButton.Click += CompleteButton_Click;

            // Cancel Button
            cancelButton.Text = "Cancelar";
            cancelButton.Size = new Size(90, 35);
            cancelButton.Location = new Point(315, 130);
            cancelButton.BorderRadius = 4;
            cancelButton.BackColor = Color.FromArgb(220, 53, 69);
            cancelButton.ForeColor = Color.White;
            cancelButton.Click += CancelButton_Click;

            // Add controls to panel
            mainPanel.Controls.Add(customerNameLabel);
            mainPanel.Controls.Add(vehicleLabel);
            mainPanel.Controls.Add(dateLabel);
            mainPanel.Controls.Add(statusLabel);
            mainPanel.Controls.Add(viewDetailsButton);
            mainPanel.Controls.Add(scheduleButton);
            mainPanel.Controls.Add(completeButton);
            mainPanel.Controls.Add(cancelButton);

            // Add panel to control
            this.Controls.Add(mainPanel);
            this.Size = new Size(420, 180);
        }

        private void UpdateCardInfo()
        {
            if (_request != null)
            {
                customerNameLabel.Text = _request.CustomerName;

                if (_request.Vehicle != null)
                {
                    vehicleLabel.Text = $"{_request.Vehicle.Brand} {_request.Vehicle.Model} ({_request.Vehicle.Year})";
                }
                else if (!string.IsNullOrEmpty(_request.DesiredBrand) || !string.IsNullOrEmpty(_request.DesiredModel))
                {
                    vehicleLabel.Text = $"Deseado: {_request.DesiredBrand} {_request.DesiredModel}";
                }
                else
                {
                    vehicleLabel.Text = $"Vehículo ID: {_request.VehicleId}";
                }

                dateLabel.Text = $"Solicitud: {_request.RequestDate.ToShortDateString()}";

                if (_request.ScheduledDate.HasValue)
                {
                    dateLabel.Text += $" | Cita: {_request.ScheduledDate.Value.ToShortDateString()}";
                }
                else if (_request.AppointmentDate.HasValue)
                {
                    dateLabel.Text += $" | Cita: {_request.AppointmentDate.Value.ToShortDateString()}";
                }

                // Set status label
                statusLabel.Text = GetStatusText(_request.Status);
                statusLabel.BackColor = GetStatusColor(_request.Status);

                // Enable/disable buttons based on status
                scheduleButton.Visible = _request.Status == RequestStatus.Pending;
                completeButton.Visible = _request.Status == RequestStatus.Scheduled;
                cancelButton.Visible = _request.Status != RequestStatus.Completed && _request.Status != RequestStatus.Cancelled;
            }
        }

        private string GetStatusText(RequestStatus status)
        {
            switch (status)
            {
                case RequestStatus.Pending:
                    return "Pendiente";
                case RequestStatus.Scheduled:
                    return "Programada";
                case RequestStatus.Completed:
                    return "Completada";
                case RequestStatus.Cancelled:
                    return "Cancelada";
                default:
                    return "Desconocido";
            }
        }

        private Color GetStatusColor(RequestStatus status)
        {
            switch (status)
            {
                case RequestStatus.Pending:
                    return Color.FromArgb(255, 243, 205); // Amarillo claro
                case RequestStatus.Scheduled:
                    return Color.FromArgb(207, 226, 255); // Azul claro
                case RequestStatus.Completed:
                    return Color.FromArgb(209, 231, 221); // Verde claro
                case RequestStatus.Cancelled:
                    return Color.FromArgb(248, 215, 218); // Rojo claro
                default:
                    return Color.FromArgb(240, 240, 240); // Gris claro
            }
        }

        private void ViewDetailsButton_Click(object sender, EventArgs e)
        {
            if (_request != null && ViewDetailsClicked != null)
            {
                ViewDetailsClicked(this, _request);
            }
        }

        private void ScheduleButton_Click(object sender, EventArgs e)
        {
            if (_request != null && ScheduleClicked != null)
            {
                ScheduleClicked(this, _request);
            }
        }

        private void CompleteButton_Click(object sender, EventArgs e)
        {
            if (_request != null && CompleteClicked != null)
            {
                CompleteClicked(this, _request);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (_request != null && CancelClicked != null)
            {
                CancelClicked(this, _request);
            }
        }
    }
}
