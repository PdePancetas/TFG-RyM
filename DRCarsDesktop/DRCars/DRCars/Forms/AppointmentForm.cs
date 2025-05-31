using System;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Controls;
using DRCars.Models;
using DRCars.Utils;

namespace DRCars.Forms
{
    public partial class AppointmentForm : Form
    {
        private RoundedPanel mainPanel;
        private Label titleLabel;
        private Label customerInfoLabel;
        private Label dateLabel;
        private DateTimePicker datePicker;
        private Label timeLabel;
        private DateTimePicker timePicker;
        private Label notesLabel;
        private TextBox notesTextBox;
        private RoundedButton saveButton;
        private RoundedButton cancelButton;

        private SaleRequest _request;
        private ApiClient apiClient;

        public AppointmentForm(SaleRequest request)
        {
            _request = request;
            apiClient = new ApiClient();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            mainPanel = new RoundedPanel();
            titleLabel = new Label();
            customerInfoLabel = new Label();
            dateLabel = new Label();
            datePicker = new DateTimePicker();
            timeLabel = new Label();
            timePicker = new DateTimePicker();
            notesLabel = new Label();
            notesTextBox = new TextBox();
            saveButton = new RoundedButton();
            cancelButton = new RoundedButton();

            // Form
            this.Text = "Programar Cita";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Main Panel
            mainPanel.BorderRadius = 15;
            mainPanel.BorderColor = Color.FromArgb(220, 220, 220);
            mainPanel.BorderSize = 1;
            mainPanel.Size = new Size(450, 450);
            mainPanel.Location = new Point((this.ClientSize.Width - 450) / 2, (this.ClientSize.Height - 450) / 2);
            mainPanel.BackColor = Color.White;
            mainPanel.Padding = new Padding(30);

            // Title Label
            titleLabel.AutoSize = false;
            titleLabel.Size = new Size(390, 40);
            titleLabel.Location = new Point(30, 20);
            titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Text = "Programar Cita";

            // Customer Info Label
            customerInfoLabel.AutoSize = false;
            customerInfoLabel.Size = new Size(390, 60);
            customerInfoLabel.Location = new Point(30, 70);
            customerInfoLabel.Font = new Font("Segoe UI", 9F);
            customerInfoLabel.Text = $"Cliente: {_request.cliente.Name}\nEmail: {_request.cliente.Name}";//\nTeléfono: {_request.cliente.Name}";

            // Date Label
            dateLabel.AutoSize = true;
            dateLabel.Location = new Point(30, 140);
            dateLabel.Font = new Font("Segoe UI", 9F);
            dateLabel.Text = "Fecha";

            // Date Picker
            datePicker.Size = new Size(200, 30);
            datePicker.Location = new Point(30, 165);
            datePicker.Font = new Font("Segoe UI", 9F);
            datePicker.Format = DateTimePickerFormat.Short;
            datePicker.MinDate = DateTime.Today;

            // Time Label
            timeLabel.AutoSize = true;
            timeLabel.Location = new Point(250, 140);
            timeLabel.Font = new Font("Segoe UI", 9F);
            timeLabel.Text = "Hora";

            // Time Picker
            timePicker.Size = new Size(170, 30);
            timePicker.Location = new Point(250, 165);
            timePicker.Font = new Font("Segoe UI", 9F);
            timePicker.Format = DateTimePickerFormat.Time;
            timePicker.ShowUpDown = true;

            // Notes Label
            notesLabel.AutoSize = true;
            notesLabel.Location = new Point(30, 205);
            notesLabel.Font = new Font("Segoe UI", 9F);
            notesLabel.Text = "Notas";

            // Notes TextBox
            notesTextBox.Size = new Size(390, 100);
            notesTextBox.Location = new Point(30, 230);
            notesTextBox.Font = new Font("Segoe UI", 9F);
            notesTextBox.Multiline = true;

            // Save Button
            saveButton.Text = "Guardar";
            saveButton.Size = new Size(180, 45);
            saveButton.Location = new Point(30, 350);
            saveButton.BorderRadius = 10;
            saveButton.BackColor = Color.FromArgb(0, 120, 215);
            saveButton.ForeColor = Color.White;
            saveButton.Click += SaveButton_Click;

            // Cancel Button
            cancelButton.Text = "Cancelar";
            cancelButton.Size = new Size(180, 45);
            cancelButton.Location = new Point(240, 350);
            cancelButton.BorderRadius = 10;
            cancelButton.BackColor = Color.FromArgb(200, 50, 50);
            cancelButton.ForeColor = Color.White;
            cancelButton.Click += CancelButton_Click;

            // Add controls to panel
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(customerInfoLabel);
            mainPanel.Controls.Add(dateLabel);
            mainPanel.Controls.Add(datePicker);
            mainPanel.Controls.Add(timeLabel);
            mainPanel.Controls.Add(timePicker);
            mainPanel.Controls.Add(notesLabel);
            mainPanel.Controls.Add(notesTextBox);
            mainPanel.Controls.Add(saveButton);
            mainPanel.Controls.Add(cancelButton);

            // Add panel to form
            this.Controls.Add(mainPanel);
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Combine date and time
                DateTime appointmentDate = datePicker.Value.Date.Add(timePicker.Value.TimeOfDay);

                // Update request with appointment date
                _request.ScheduledDate = appointmentDate;
                _request.AppointmentDate = appointmentDate;
                _request.Status = RequestStatus.Scheduled;
                _request.AdditionalDetails = (_request.AdditionalDetails ?? "") + "\n\nNotas de la cita: " + notesTextBox.Text;

                // Save changes
                String estado = await apiClient.UpdateSaleRequestAsync(_request);
                MessageBox.Show(estado);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la cita: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
