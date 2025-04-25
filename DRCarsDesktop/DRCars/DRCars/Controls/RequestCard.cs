using System;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Models;

namespace DRCars.Controls
{
    public class RequestCard : RoundedPanel
    {
        private SaleRequest _request;
        private Label customerNameLabel;
        private Label customerContactLabel;
        private Label requestDetailsLabel;
        private Label statusLabel;
        private Label dateLabel;
        private RoundedButton scheduleButton;
        private RoundedButton completeButton;
        private RoundedButton cancelButton;

        public event EventHandler<SaleRequest> ScheduleClicked;
        public event EventHandler<SaleRequest> CompleteClicked;
        public event EventHandler<SaleRequest> CancelClicked;

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
            customerNameLabel = new Label();
            customerContactLabel = new Label();
            requestDetailsLabel = new Label();
            statusLabel = new Label();
            dateLabel = new Label();
            scheduleButton = new RoundedButton();
            completeButton = new RoundedButton();
            cancelButton = new RoundedButton();

            // Request Card
            this.Size = new Size(400, 250);
            this.BorderRadius = 15;
            this.BorderColor = Color.FromArgb(220, 220, 220);
            this.BorderSize = 1;
            this.BackColor = Color.White;
            this.Padding = new Padding(15);

            // Customer Name Label
            customerNameLabel.AutoSize = false;
            customerNameLabel.Size = new Size(370, 30);
            customerNameLabel.Location = new Point(15, 15);
            customerNameLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            customerNameLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Customer Contact Label
            customerContactLabel.AutoSize = false;
            customerContactLabel.Size = new Size(370, 25);
            customerContactLabel.Location = new Point(15, 45);
            customerContactLabel.Font = new Font("Segoe UI", 9F);
            customerContactLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Request Details Label
            requestDetailsLabel.AutoSize = false;
            requestDetailsLabel.Size = new Size(370, 60);
            requestDetailsLabel.Location = new Point(15, 75);
            requestDetailsLabel.Font = new Font("Segoe UI", 9F);
            requestDetailsLabel.TextAlign = ContentAlignment.TopLeft;

            // Status Label
            statusLabel.AutoSize = false;
            statusLabel.Size = new Size(100, 25);
            statusLabel.Location = new Point(15, 140);
            statusLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            statusLabel.BackColor = Color.FromArgb(240, 240, 240);
            statusLabel.BorderStyle = BorderStyle.FixedSingle;

            // Date Label
            dateLabel.AutoSize = false;
            dateLabel.Size = new Size(250, 25);
            dateLabel.Location = new Point(135, 140);
            dateLabel.Font = new Font("Segoe UI", 9F);
            dateLabel.TextAlign = ContentAlignment.MiddleRight;

            // Schedule Button
            scheduleButton.Text = "Programar Cita";
            scheduleButton.Size = new Size(120, 35);
            scheduleButton.Location = new Point(15, 180);
            scheduleButton.BorderRadius = 10;
            scheduleButton.BackColor = Color.FromArgb(0, 120, 215);
            scheduleButton.ForeColor = Color.White;
            scheduleButton.Click += ScheduleButton_Click;

            // Complete Button
            completeButton.Text = "Completar";
            completeButton.Size = new Size(120, 35);
            completeButton.Location = new Point(145, 180);
            completeButton.BorderRadius = 10;
            completeButton.BackColor = Color.FromArgb(0, 150, 50);
            completeButton.ForeColor = Color.White;
            completeButton.Click += CompleteButton_Click;

            // Cancel Button
            cancelButton.Text = "Cancelar";
            cancelButton.Size = new Size(100, 35);
            cancelButton.Location = new Point(275, 180);
            cancelButton.BorderRadius = 10;
            cancelButton.BackColor = Color.FromArgb(200, 50, 50);
            cancelButton.ForeColor = Color.White;
            cancelButton.Click += CancelButton_Click;

            // Add controls to panel
            this.Controls.Add(customerNameLabel);
            this.Controls.Add(customerContactLabel);
            this.Controls.Add(requestDetailsLabel);
            this.Controls.Add(statusLabel);
            this.Controls.Add(dateLabel);
            this.Controls.Add(scheduleButton);
            this.Controls.Add(completeButton);
            this.Controls.Add(cancelButton);
        }

        private void UpdateCardInfo()
        {
            if (_request != null)
            {
                customerNameLabel.Text = _request.CustomerName;
                customerContactLabel.Text = $"Email: {_request.CustomerEmail} | Teléfono: {_request.CustomerPhone}";

                string details = $"Vehículo deseado: {_request.DesiredBrand} {_request.DesiredModel}";
                if (_request.Budget.HasValue)
                {
                    details += $"\nPresupuesto: {_request.Budget:N0} €";
                }
                if (!string.IsNullOrEmpty(_request.DeliveryTimeframe))
                {
                    details += $" | Plazo de entrega: {_request.DeliveryTimeframe}";
                }
                requestDetailsLabel.Text = details;

                // Set status label
                statusLabel.Text = GetStatusText(_request.Status);
                statusLabel.BackColor = GetStatusColor(_request.Status);

                // Set date label
                dateLabel.Text = $"Creado: {_request.CreatedAt:dd/MM/yyyy}";
                if (_request.AppointmentDate.HasValue)
                {
                    dateLabel.Text += $" | Cita: {_request.AppointmentDate.Value:dd/MM/yyyy HH:mm}";
                }

                // Enable/disable buttons based on status
                UpdateButtonsState();
            }
        }

        private void UpdateButtonsState()
        {
            switch (_request.Status)
            {
                case RequestStatus.Pending:
                    scheduleButton.Enabled = true;
                    completeButton.Enabled = true;
                    cancelButton.Enabled = true;
                    break;
                case RequestStatus.Scheduled:
                    scheduleButton.Enabled = false;
                    completeButton.Enabled = true;
                    cancelButton.Enabled = true;
                    break;
                case RequestStatus.Completed:
                case RequestStatus.Cancelled:
                    scheduleButton.Enabled = false;
                    completeButton.Enabled = false;
                    cancelButton.Enabled = false;
                    break;
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
                    return Color.FromArgb(255, 255, 230);
                case RequestStatus.Scheduled:
                    return Color.FromArgb(230, 255, 230);
                case RequestStatus.Completed:
                    return Color.FromArgb(230, 230, 255);
                case RequestStatus.Cancelled:
                    return Color.FromArgb(255, 230, 230);
                default:
                    return Color.FromArgb(240, 240, 240);
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
