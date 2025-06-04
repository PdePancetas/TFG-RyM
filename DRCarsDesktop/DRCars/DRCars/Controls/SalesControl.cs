using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Models;
using DRCars.Utils;
using DRCars.Forms;

namespace DRCars.Controls
{
    public class SalesControl : UserControl
    {
        private TabControl salesTabControl;
        private TabPage requestsTabPage;
        private TabPage salesTabPage;
        private TabPage appointmentsTabPage;

        private FlowLayoutPanel requestsPanel;
        private FlowLayoutPanel appointmentsPanel;
        private FlowLayoutPanel salesPanel;

        private RoundedPanel requestsHeaderPanel;
        private RoundedPanel appointmentsHeaderPanel;
        private RoundedPanel salesHeaderPanel;

        private RoundedButton refreshRequestsButton;
        private RoundedButton refreshAppointmentsButton;
        private RoundedButton refreshSalesButton;

        private RoundedTextBox searchRequestsTextBox;
        private RoundedTextBox searchAppointmentsTextBox;
        private RoundedTextBox searchSalesTextBox;

        private Label requestsCountLabel;
        private Label appointmentsCountLabel;
        private Label salesCountLabel;

        private ApiClient apiClient;

        private List<Request> allRequests;
        private List<Appointment> allAppointments;
        private List<Sale> allSales;

        public SalesControl()
        {
            apiClient = new ApiClient();
            InitializeComponent();
            // Eliminamos la carga automática: LoadData();
        }

        public async void LoadData()
        {
            try
            {
                // Load sale requests
                allRequests = await apiClient.GetRequestsAsync();
                requestsCountLabel.Text = $"Solicitudes: {allRequests.Count}";
                PopulateRequestCards();

                // Load sales
                allAppointments = await apiClient.GetAppointmentsAsync();
                appointmentsCountLabel.Text = $"Reservas: {allAppointments.Count}";
                PopulateAppointmentCards();

                // Load appointments
                allSales = await apiClient.GetSalesAsync();
                salesCountLabel.Text = $"Ventas: {allSales.Count}";
                PopulateSalesCards(); // If you want to display appointments in a similar way
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            salesTabControl = new TabControl();
            requestsTabPage = new TabPage();
            salesTabPage = new TabPage();
            appointmentsTabPage = new TabPage();

            requestsPanel = new FlowLayoutPanel();
            appointmentsPanel = new FlowLayoutPanel();
            salesPanel = new FlowLayoutPanel();

            requestsHeaderPanel = new RoundedPanel();
            appointmentsHeaderPanel = new RoundedPanel();
            salesHeaderPanel = new RoundedPanel();

            refreshRequestsButton = new RoundedButton();
            refreshAppointmentsButton = new RoundedButton();
            refreshSalesButton = new RoundedButton();

            searchRequestsTextBox = new RoundedTextBox();
            searchAppointmentsTextBox = new RoundedTextBox();
            searchSalesTextBox = new RoundedTextBox();

            requestsCountLabel = new Label();
            appointmentsCountLabel = new Label();
            salesCountLabel = new Label();

            // Sales Tab Control
            salesTabControl.Dock = DockStyle.Fill;
            salesTabControl.Font = new Font("Segoe UI", 10F);

            // Requests Tab Page
            requestsTabPage.Text = "Solicitudes";
            requestsTabPage.Padding = new Padding(10);

            // Appointments Tab Page
            appointmentsTabPage.Text = "Reservas";
            appointmentsTabPage.Padding = new Padding(10);

            // Sales Tab Page
            salesTabPage.Text = "Ventas";
            salesTabPage.Padding = new Padding(10);


            // Requests Header Panel
            requestsHeaderPanel.BorderRadius = 15;
            requestsHeaderPanel.BorderColor = Color.FromArgb(220, 220, 220);
            requestsHeaderPanel.BorderSize = 1;
            requestsHeaderPanel.Dock = DockStyle.Top;
            requestsHeaderPanel.Height = 70;
            requestsHeaderPanel.Padding = new Padding(15);
            requestsHeaderPanel.Margin = new Padding(0, 0, 0, 10);

            // Search Requests TextBox
            searchRequestsTextBox.PlaceholderText = "Buscar solicitudes...";
            searchRequestsTextBox.Size = new Size(300, 40);
            searchRequestsTextBox.Location = new Point(15, 15);
            searchRequestsTextBox.BorderRadius = 10;
            searchRequestsTextBox.TextChanged += SearchRequestsTextBox_TextChanged;

            // Requests Count Label
            requestsCountLabel.AutoSize = false;
            requestsCountLabel.Size = new Size(200, 40);
            requestsCountLabel.Location = new Point(330, 15);
            requestsCountLabel.Font = new Font("Segoe UI", 10F);
            requestsCountLabel.TextAlign = ContentAlignment.MiddleLeft;
            requestsCountLabel.Text = "Solicitudes: 0";

            // Refresh Requests Button
            refreshRequestsButton.Text = "Actualizar";
            refreshRequestsButton.Size = new Size(120, 40);
            refreshRequestsButton.Location = new Point(requestsHeaderPanel.Width - 150, 15);
            refreshRequestsButton.BorderRadius = 10;
            refreshRequestsButton.BackColor = Color.FromArgb(50, 50, 50);
            refreshRequestsButton.ForeColor = Color.White;
            refreshRequestsButton.Click += RefreshRequestsButton_Click;

            // Requests Panel
            requestsPanel.Dock = DockStyle.Fill;
            requestsPanel.AutoScroll = true;
            requestsPanel.Padding = new Padding(10);

            // Appointments Header Panel
            appointmentsHeaderPanel.BorderRadius = 15;
            appointmentsHeaderPanel.BorderColor = Color.FromArgb(220, 220, 220);
            appointmentsHeaderPanel.BorderSize = 1;
            appointmentsHeaderPanel.Dock = DockStyle.Top;
            appointmentsHeaderPanel.Height = 70;
            appointmentsHeaderPanel.Padding = new Padding(15);
            appointmentsHeaderPanel.Margin = new Padding(0, 0, 0, 10);

            // Search Appointments TextBox
            searchAppointmentsTextBox.PlaceholderText = "Buscar reservas...";
            searchAppointmentsTextBox.Size = new Size(300, 40);
            searchAppointmentsTextBox.Location = new Point(15, 15);
            searchAppointmentsTextBox.BorderRadius = 10;
            searchAppointmentsTextBox.TextChanged += SearchAppointmentsTextBox_TextChanged;

            // Appointments Count Label
            appointmentsCountLabel.AutoSize = false;
            appointmentsCountLabel.Size = new Size(200, 40);
            appointmentsCountLabel.Location = new Point(330, 15);
            appointmentsCountLabel.Font = new Font("Segoe UI", 10F);
            appointmentsCountLabel.TextAlign = ContentAlignment.MiddleLeft;
            appointmentsCountLabel.Text = "Reservas: 0";

            // Refresh Appointments Button
            refreshAppointmentsButton.Text = "Actualizar";
            refreshAppointmentsButton.Size = new Size(120, 40);
            refreshAppointmentsButton.Location = new Point(appointmentsHeaderPanel.Width - 150, 15);
            refreshAppointmentsButton.BorderRadius = 10;
            refreshAppointmentsButton.BackColor = Color.FromArgb(50, 50, 50);
            refreshAppointmentsButton.ForeColor = Color.White;
            refreshAppointmentsButton.Click += RefreshAppointmentsButton_Click;

            // Appointments Panel
            appointmentsPanel.Dock = DockStyle.Fill;
            appointmentsPanel.AutoScroll = true;
            appointmentsPanel.Padding = new Padding(10);

            // Sales Header Panel
            salesHeaderPanel.BorderRadius = 15;
            salesHeaderPanel.BorderColor = Color.FromArgb(220, 220, 220);
            salesHeaderPanel.BorderSize = 1;
            salesHeaderPanel.Dock = DockStyle.Top;
            salesHeaderPanel.Height = 70;
            salesHeaderPanel.Padding = new Padding(15);
            salesHeaderPanel.Margin = new Padding(0, 0, 0, 10);

            // Search Sales TextBox
            searchSalesTextBox.PlaceholderText = "Buscar ventas...";
            searchSalesTextBox.Size = new Size(300, 40);
            searchSalesTextBox.Location = new Point(15, 15);
            searchSalesTextBox.BorderRadius = 10;
            searchSalesTextBox.TextChanged += SearchSalesTextBox_TextChanged;

            // Sales Count Label
            salesCountLabel.AutoSize = false;
            salesCountLabel.Size = new Size(200, 40);
            salesCountLabel.Location = new Point(330, 15);
            salesCountLabel.Font = new Font("Segoe UI", 10F);
            salesCountLabel.TextAlign = ContentAlignment.MiddleLeft;
            salesCountLabel.Text = "Ventas: 0";

            // Refresh Sales Button
            refreshSalesButton.Text = "Actualizar";
            refreshSalesButton.Size = new Size(120, 40);
            refreshSalesButton.Location = new Point(salesHeaderPanel.Width - 150, 15);
            refreshSalesButton.BorderRadius = 10;
            refreshSalesButton.BackColor = Color.FromArgb(50, 50, 50);
            refreshSalesButton.ForeColor = Color.White;
            refreshSalesButton.Click += RefreshSalesButton_Click;

            // Sales Panel
            salesPanel.Dock = DockStyle.Fill;
            salesPanel.AutoScroll = true;
            salesPanel.Padding = new Padding(10);

            // Add controls to panels
            requestsHeaderPanel.Controls.Add(searchRequestsTextBox);
            requestsHeaderPanel.Controls.Add(requestsCountLabel);
            requestsHeaderPanel.Controls.Add(refreshRequestsButton);

            salesHeaderPanel.Controls.Add(searchSalesTextBox);
            salesHeaderPanel.Controls.Add(salesCountLabel);
            salesHeaderPanel.Controls.Add(refreshSalesButton);

            appointmentsHeaderPanel.Controls.Add(searchAppointmentsTextBox);
            appointmentsHeaderPanel.Controls.Add(appointmentsCountLabel);
            appointmentsHeaderPanel.Controls.Add(refreshAppointmentsButton);

            // Add panels to tab pages
            requestsTabPage.Controls.Add(requestsPanel);
            requestsTabPage.Controls.Add(requestsHeaderPanel);

            salesTabPage.Controls.Add(salesPanel);
            salesTabPage.Controls.Add(salesHeaderPanel);

            appointmentsTabPage.Controls.Add(appointmentsPanel);
            appointmentsTabPage.Controls.Add(appointmentsHeaderPanel);

            // Add tab pages to tab control
            salesTabControl.TabPages.Add(requestsTabPage);
            salesTabControl.TabPages.Add(appointmentsTabPage);
            salesTabControl.TabPages.Add(salesTabPage);

            // Add tab control to user control
            this.Controls.Add(salesTabControl);

            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 245, 245);

            // Handle resize event to reposition buttons
            this.Resize += SalesControl_Resize;
        }

        private void SalesControl_Resize(object sender, EventArgs e)
        {
            // Reposition buttons when control is resized
            refreshRequestsButton.Location = new Point(requestsHeaderPanel.Width - 150, 15);
            refreshAppointmentsButton.Location = new Point(appointmentsHeaderPanel.Width - 150, 15);
            refreshSalesButton.Location = new Point(salesHeaderPanel.Width - 150, 15);
        }


        private void PopulateSalesCards()
        {
            
        }

        private void PopulateRequestCards()
        {
            requestsPanel.Controls.Clear();

            foreach (var request in allRequests)
            {
                RequestCard card = new RequestCard
                {
                    Margin = new Padding(10),
                    Size = new Size(400, 250)
                };
                card.Request = request;
                card.ScheduleClicked += RequestCard_ScheduleClicked;
                card.CompleteClicked += RequestCard_CompleteClicked;
                card.CancelClicked += RequestCard_CancelClicked;

                requestsPanel.Controls.Add(card);
            }
        }

        private void PopulateAppointmentCards()
        {
            appointmentsPanel.Controls.Clear();

            // Here you would create and add sale cards
            // This is a placeholder for future implementation
            foreach (var appointment in allAppointments)
            {
                RoundedPanel appointmentCard = new RoundedPanel
                {
                    BorderRadius = 15,
                    BorderColor = Color.FromArgb(220, 220, 220),
                    BorderSize = 1,
                    Size = new Size(400, 200),
                    Margin = new Padding(10)
                };

                Label appointmentIdLabel = new Label
                {
                    Text = $"Reserva #{appointment.Id}",
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    AutoSize = false,
                    Size = new Size(380, 30),
                    Location = new Point(10, 10)
                };

                Label vehicleLabel = new Label
                {
                    Text = $"Vehículo: (Id: {appointment.Vehicle.Id}) {appointment.Vehicle.Brand} {appointment.Vehicle.Model}",
                    Font = new Font("Segoe UI", 9F),
                    AutoSize = false,
                    Size = new Size(380, 25),
                    Location = new Point(10, 45)
                };

                Label customerLabel = new Label
                {
                    Text = $"Cliente ID: {appointment.client.Id}",
                    Font = new Font("Segoe UI", 9F),
                    AutoSize = false,
                    Size = new Size(380, 25),
                    Location = new Point(10, 70)
                };

                Label priceLabel = new Label
                {
                    Text = $"Precio: {appointment.AppointmentPrice:N0} €",
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    AutoSize = false,
                    Size = new Size(380, 25),
                    Location = new Point(10, 95)
                };

                Label dateLabel = new Label
                {
                    Text = $"Fecha: {appointment.AppointmentDate:dd/MM/yyyy}",
                    Font = new Font("Segoe UI", 9F),
                    AutoSize = false,
                    Size = new Size(380, 25),
                    Location = new Point(10, 120)
                };

                appointmentCard.Controls.Add(appointmentIdLabel);
                appointmentCard.Controls.Add(vehicleLabel);
                appointmentCard.Controls.Add(customerLabel);
                appointmentCard.Controls.Add(priceLabel);
                appointmentCard.Controls.Add(dateLabel);

                appointmentsPanel.Controls.Add(appointmentCard);
            }
        }

        private void SearchRequestsTextBox_TextChanged(object sender, EventArgs e)
        {
            FilterRequests();
        }

        private void SearchAppointmentsTextBox_TextChanged(object sender, EventArgs e)
        {
            FilterAppointments();
        }
        private void SearchSalesTextBox_TextChanged(object sender, EventArgs e)
        {
            FilterSales();
        }

        private void FilterAppointments()
        {
            string searchText = searchAppointmentsTextBox.Texts.ToLower();

            appointmentsPanel.Controls.Clear();

            foreach (var appointment in allAppointments)
            {
                if (string.IsNullOrEmpty(searchText) ||
                    appointment.client.Name.ToLower().Contains(searchText) ||
                    appointment.client.Name.ToLower().Contains(searchText) ||
                    appointment.client.Name.ToLower().Contains(searchText) ||
                    appointment.Vehicle.Brand?.ToLower().Contains(searchText) == true ||
                    appointment.Vehicle.Model?.ToLower().Contains(searchText) == true)
                {
                    AppointmentCard card = new AppointmentCard
                    {
                        Appointment = appointment,
                        Margin = new Padding(10),
                        Size = new Size(400, 250)
                    };

                    card.CompleteClicked += AppointmentCard_CompleteClicked;
                    card.CancelClicked += AppointmentCard_CancelClicked;

                    appointmentsPanel.Controls.Add(card);
                }
            }
        }

        private void FilterRequests()
        {
            string searchText = searchRequestsTextBox.Texts.ToLower();

            requestsPanel.Controls.Clear();

            foreach (var request in allRequests)
            {
                if (string.IsNullOrEmpty(searchText) ||
                    request.cliente.Name.ToLower().Contains(searchText) ||
                    request.cliente.Name.ToLower().Contains(searchText) ||
                    request.cliente.Name.ToLower().Contains(searchText) ||
                    request.DesiredBrand?.ToLower().Contains(searchText) == true ||
                    request.DesiredModel?.ToLower().Contains(searchText) == true)
                {
                    RequestCard card = new RequestCard
                    {
                        Request = request,
                        Margin = new Padding(10),
                        Size = new Size(400, 250)
                    };

                    card.ScheduleClicked += RequestCard_ScheduleClicked;
                    card.CompleteClicked += RequestCard_CompleteClicked;
                    card.CancelClicked += RequestCard_CancelClicked;

                    requestsPanel.Controls.Add(card);
                }
            }
        }

        private void FilterSales()
        {
            
        }

        private void RefreshRequestsButton_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void RefreshSalesButton_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        private void RefreshAppointmentsButton_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private async void RequestCard_ScheduleClicked(object sender, Request request)
        {
            // Open appointment scheduling form
            RequestForm requestForm = new RequestForm(request);
            if (requestForm.ShowDialog() == DialogResult.OK)
            {
                // Update request status
                request.Status = RequestStatus.Scheduled;
                await apiClient.UpdateSaleRequestAsync(request);
                LoadData();
            }
        }

        private async void RequestCard_CompleteClicked(object sender, Request request)
        {
            // Mark request as completed
            request.Status = RequestStatus.Completed;
            await apiClient.UpdateSaleRequestAsync(request);
            LoadData();
        }

        private async void RequestCard_CancelClicked(object sender, Request request)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar esta solicitud?", "Cancelar Solicitud",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Cancel request
                request.Status = RequestStatus.Cancelled;
                await apiClient.UpdateSaleRequestAsync(request);
                LoadData();
            }
        }

        private async void AppointmentCard_CompleteClicked(object sender, Appointment appointment)
        {
            await apiClient.completeAppointmentAsync(appointment);
            LoadData();
        }

        private async void AppointmentCard_CancelClicked(object sender, Appointment appointment)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar esta reserva?", "Cancelar reserva",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                await apiClient.deleteAppointmentAsync(appointment);
                LoadData();
            }
        }
    }
}
