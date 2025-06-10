using System;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Models;

namespace DRCars.Controls
{
    public class RequestCard : UserControl
    {
        private Request _request;
        private RoundedPanel mainPanel;
        public Label customerNameLabel;
        public Label vehicleLabel;
        private Label dateLabel;
        private Label statusLabel;
        private Label reasonLabel;
        private Label descriptionLabel;
        private RoundedButton scheduleButton;
        private RoundedButton completeButton;
        private RoundedButton cancelButton;

        // Colores de Odoo
        private Color primaryColor = Color.FromArgb(0, 160, 157); // Verde Odoo
        private Color secondaryColor = Color.FromArgb(242, 242, 242); // Gris claro
        private Color textColor = Color.FromArgb(51, 51, 51); // Texto oscuro
        private Color accentColor = Color.FromArgb(108, 117, 125); // Gris para detalles

        public event EventHandler<Request> ScheduleClicked;
        public event EventHandler<Request> CompleteClicked;
        public event EventHandler<Request> CancelClicked;

        public Request Request
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
            this.mainPanel = new DRCars.Controls.RoundedPanel();
            this.customerNameLabel = new System.Windows.Forms.Label();
            this.vehicleLabel = new System.Windows.Forms.Label();
            this.dateLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.reasonLabel = new Label();
            this.descriptionLabel = new Label();
            this.scheduleButton = new DRCars.Controls.RoundedButton();
            this.completeButton = new DRCars.Controls.RoundedButton();
            this.cancelButton = new DRCars.Controls.RoundedButton();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.White;
            this.mainPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.mainPanel.BorderRadius = 8;
            this.mainPanel.BorderSize = 1;
            this.mainPanel.Controls.Add(this.customerNameLabel);
            this.mainPanel.Controls.Add(this.vehicleLabel);
            this.mainPanel.Controls.Add(this.dateLabel);
            this.mainPanel.Controls.Add(this.statusLabel);
            this.mainPanel.Controls.Add(this.reasonLabel);
            this.mainPanel.Controls.Add(this.descriptionLabel);
            this.mainPanel.Controls.Add(this.scheduleButton);
            this.mainPanel.Controls.Add(this.completeButton);
            this.mainPanel.Controls.Add(this.cancelButton);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(15);
            this.mainPanel.Size = new System.Drawing.Size(420, 180);
            this.mainPanel.TabIndex = 0;
            // 
            // customerNameLabel
            // 
            this.customerNameLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.customerNameLabel.Location = new System.Drawing.Point(15, 15);
            this.customerNameLabel.Name = "customerNameLabel";
            this.customerNameLabel.Size = new System.Drawing.Size(270, 30);
            this.customerNameLabel.TabIndex = 0;
            this.customerNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // vehicleLabel
            // 
            this.vehicleLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.vehicleLabel.Location = new System.Drawing.Point(15, 45);
            this.vehicleLabel.Name = "vehicleLabel";
            this.vehicleLabel.Size = new System.Drawing.Size(270, 20);
            this.vehicleLabel.TabIndex = 1;
            this.vehicleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dateLabel
            // 
            this.dateLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dateLabel.Location = new System.Drawing.Point(15, 65);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(270, 20);
            this.dateLabel.TabIndex = 2;
            this.dateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statusLabel.Location = new System.Drawing.Point(15, 95);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(100, 25);
            this.statusLabel.TabIndex = 3;
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // motiveLabel
            // 
            this.reasonLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.reasonLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.reasonLabel.Location = new System.Drawing.Point(15, 180);
            this.reasonLabel.Name = "reasonLabel";
            this.reasonLabel.Size = new System.Drawing.Size(200, 25);
            this.reasonLabel.TabIndex = 3;
            this.reasonLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.descriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.descriptionLabel.Location = new System.Drawing.Point(15, 210);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(200, 25);
            this.descriptionLabel.TabIndex = 3;
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scheduleButton
            // 
            this.scheduleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.scheduleButton.BorderColor = System.Drawing.Color.Silver;
            this.scheduleButton.BorderRadius = 4;
            this.scheduleButton.BorderSize = 0;
            this.scheduleButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.scheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.scheduleButton.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.scheduleButton.ForeColor = System.Drawing.Color.White;
            this.scheduleButton.Location = new System.Drawing.Point(115, 130);
            this.scheduleButton.Name = "scheduleButton";
            this.scheduleButton.Size = new System.Drawing.Size(90, 35);
            this.scheduleButton.TabIndex = 5;
            this.scheduleButton.Text = "Programar";
            this.scheduleButton.UseVisualStyleBackColor = false;
            this.scheduleButton.Click += ScheduleButton_Click;
            // 
            // completeButton
            // 
            this.completeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.completeButton.BorderColor = System.Drawing.Color.Silver;
            this.completeButton.BorderRadius = 4;
            this.completeButton.BorderSize = 0;
            this.completeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.completeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.completeButton.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.completeButton.ForeColor = System.Drawing.Color.White;
            this.completeButton.Location = new System.Drawing.Point(215, 130);
            this.completeButton.Name = "completeButton";
            this.completeButton.Size = new System.Drawing.Size(90, 35);
            this.completeButton.TabIndex = 6;
            this.completeButton.Text = "Aceptar";
            this.completeButton.UseVisualStyleBackColor = false;
            this.completeButton.Click += CompleteButton_Click;
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.cancelButton.BorderColor = System.Drawing.Color.Silver;
            this.cancelButton.BorderRadius = 4;
            this.cancelButton.BorderSize = 0;
            this.cancelButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(315, 130);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(90, 35);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancelar";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += CancelButton_Click;
            // 
            // RequestCard
            // 
            this.Controls.Add(this.mainPanel);
            this.Name = "RequestCard";
            this.Size = new System.Drawing.Size(420, 180);
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        public void UpdateCardInfo()
        {
            if (_request != null)
            {
                customerNameLabel.Text = _request.client.Name+ " " + _request.client.Surname;

                if (_request.Vehicle != null)
                {
                    vehicleLabel.Text = $"{_request.Vehicle.Brand} {_request.Vehicle.Model} ({_request.Vehicle.Year})";
                }
                else
                {
                    vehicleLabel.Text = "Sin vehiculo asociado";
                }

                dateLabel.Text = $"Solicitud: {_request.RequestDate.ToShortDateString()} - Precio: {_request.Budget} €";
                
                // Set status label
                statusLabel.Text = GetStatusText(_request.Status);
                statusLabel.BackColor = GetStatusColor(_request.Status);

                reasonLabel.Text = _request.RequestReason;
                descriptionLabel.Text = _request.RequestDescription;

                // Enable/disable buttons based on status
                scheduleButton.Visible = _request.Status == RequestStatus.Pending;
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
