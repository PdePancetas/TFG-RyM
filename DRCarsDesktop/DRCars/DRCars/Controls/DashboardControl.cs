using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DRCars.Models;
using DRCars.Utils;

namespace DRCars.Controls
{
    public class DashboardControl : UserControl
    {
        private RoundedPanel statsPanel;
        private RoundedPanel salesChartPanel;
        private RoundedPanel vehiclesChartPanel;
        private RoundedPanel recentSalesPanel;
        private RoundedPanel pendingRequestsPanel;
        private Chart salesChart;
        private Chart vehiclesChart;
        private Label totalVehiclesLabel;
        private Label totalSalesLabel;
        private Label pendingRequestsLabel;
        private Label averagePriceLabel;
        private Label recentAppointmentsTitle;
        private Label pendingRequestsTitle;
        private FlowLayoutPanel recentAppointmentsFlow;
        private FlowLayoutPanel pendingRequestsFlow;

        private ApiClient apiClient;

        // Colores de Odoo
        private Color primaryColor = Color.FromArgb(0, 160, 157); // Verde Odoo
        private Color secondaryColor = Color.FromArgb(242, 242, 242); // Gris claro
        private Color textColor = Color.FromArgb(51, 51, 51); // Texto oscuro
        private Color accentColor = Color.FromArgb(108, 117, 125); // Gris para detalles

        public DashboardControl()
        {
            apiClient = new ApiClient();
            InitializeComponent();
           //LoadData();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadData(); 
        }

        private void InitializeComponent()
        {
            statsPanel = new RoundedPanel();
            salesChartPanel = new RoundedPanel();
            vehiclesChartPanel = new RoundedPanel();
            recentSalesPanel = new RoundedPanel();
            pendingRequestsPanel = new RoundedPanel();
            salesChart = new Chart();
            vehiclesChart = new Chart();
            totalVehiclesLabel = new Label();
            totalSalesLabel = new Label();
            pendingRequestsLabel = new Label();
            averagePriceLabel = new Label();
            recentAppointmentsTitle = new Label();
            pendingRequestsTitle = new Label();
            recentAppointmentsFlow = new FlowLayoutPanel();
            pendingRequestsFlow = new FlowLayoutPanel();

            //Sales Chart

            salesChart.Size = new Size(420, 270);
            salesChart.Location = new Point(15, 15);
            salesChart.BackColor = Color.White;
            salesChart.Dock = DockStyle.Fill;
            salesChart.ChartAreas.Add(new ChartArea());
            salesChart.ChartAreas[0].AxisY.Minimum = Double.NaN;
            salesChart.ChartAreas[0].AxisY.Maximum = Double.NaN;
            

            // Vehicles Chart

            vehiclesChart.Size = new Size(420, 270);
            vehiclesChart.Location = new Point(15, 15);
            vehiclesChart.BackColor = Color.White;
            vehiclesChart.Dock = DockStyle.Fill;
            vehiclesChart.ChartAreas.Add(new ChartArea());
            vehiclesChart.ChartAreas[0].AxisY.Minimum = Double.NaN;
            vehiclesChart.ChartAreas[0].AxisY.Maximum = Double.NaN;

            // Stats Panel
            statsPanel.BorderRadius = 8;
            statsPanel.BorderColor = Color.FromArgb(230, 230, 230);
            statsPanel.BorderSize = 1;
            statsPanel.Dock = DockStyle.Top;
            statsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left ;
            statsPanel.Height = 100;
            statsPanel.BackColor = Color.White;
            statsPanel.Padding = new Padding(20);
            statsPanel.Margin = new Padding(0, 0, 0, 20);

            // Total Vehicles Label
            totalVehiclesLabel.AutoSize = false;
            totalVehiclesLabel.Size = new Size(200, 60);
            totalVehiclesLabel.Location = new Point(20, 20);
            totalVehiclesLabel.Font = new Font("Segoe UI", 10F);
            totalVehiclesLabel.Text = "Vehículos Totales\n0";
            totalVehiclesLabel.TextAlign = ContentAlignment.MiddleCenter;

            // Total Sales Label
            totalSalesLabel.AutoSize = false;
            totalSalesLabel.Size = new Size(200, 60);
            totalSalesLabel.Location = new Point(240, 20);
            totalSalesLabel.Font = new Font("Segoe UI", 10F);
            totalSalesLabel.Text = "Ventas Totales\n0";
            totalSalesLabel.TextAlign = ContentAlignment.MiddleCenter;

            // Pending Requests Label
            pendingRequestsLabel.AutoSize = false;
            pendingRequestsLabel.Size = new Size(200, 60);
            pendingRequestsLabel.Location = new Point(460, 20);
            pendingRequestsLabel.Font = new Font("Segoe UI", 10F);
            pendingRequestsLabel.Text = "Solicitudes Pendientes\n0";
            pendingRequestsLabel.TextAlign = ContentAlignment.MiddleCenter;

            // Average Price Label
            averagePriceLabel.AutoSize = false;
            averagePriceLabel.Size = new Size(200, 60);
            averagePriceLabel.Location = new Point(680, 20);
            averagePriceLabel.Font = new Font("Segoe UI", 10F);
            averagePriceLabel.Text = "Precio Medio\n0 €";
            averagePriceLabel.TextAlign = ContentAlignment.MiddleCenter;

            // Charts Row
            salesChartPanel.BorderRadius = 8;
            salesChartPanel.BorderColor = Color.FromArgb(230, 230, 230);
            salesChartPanel.BorderSize = 1;
            salesChartPanel.Size = new Size(450, 300);
            salesChartPanel.Location = new Point(0, 120);
            salesChartPanel.BackColor = Color.White;
            salesChartPanel.Padding = new Padding(15);
            salesChartPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // Vehicles Chart Panel
            vehiclesChartPanel.BorderRadius = 8;
            vehiclesChartPanel.BorderColor = Color.FromArgb(230, 230, 230);
            vehiclesChartPanel.BorderSize = 1;
            vehiclesChartPanel.Size = new Size(450, 300);
            vehiclesChartPanel.Location = new Point(470, 120);
            vehiclesChartPanel.BackColor = Color.White;
            vehiclesChartPanel.Padding = new Padding(15);
            vehiclesChartPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // Recent Sales Panel
            recentSalesPanel.BorderRadius = 8;
            recentSalesPanel.BorderColor = Color.FromArgb(230, 230, 230);
            recentSalesPanel.BorderSize = 1;
            recentSalesPanel.Size = new Size(450, 300);
            recentSalesPanel.Location = new Point(0, 440);
            recentSalesPanel.BackColor = Color.White;
            recentSalesPanel.Padding = new Padding(15);
            recentSalesPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

            // Recent Sales Title
            recentAppointmentsTitle.AutoSize = false;
            recentAppointmentsTitle.Size = new Size(420, 30);
            recentAppointmentsTitle.Location = new Point(15, 15);
            recentAppointmentsTitle.Font = new Font("Segoe UI Semibold", 12F);
            recentAppointmentsTitle.Text = "Reservas Realizadas";
            recentAppointmentsTitle.TextAlign = ContentAlignment.MiddleLeft;

            // Recent Sales Flow
            recentAppointmentsFlow.AutoScroll = true;
            recentAppointmentsFlow.Size = new Size(420, 240);
            recentAppointmentsFlow.Location = new Point(15, 45);
            recentAppointmentsFlow.BackColor = Color.White;
            recentAppointmentsFlow.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            recentAppointmentsFlow.FlowDirection = FlowDirection.TopDown;
            recentAppointmentsFlow.WrapContents = false;
            recentAppointmentsFlow.Dock = DockStyle.Fill;
            recentAppointmentsFlow.Padding = new Padding(0, 30, 0, 0); // 10 píxeles de espacio arriba

            // Pending Requests Panel
            pendingRequestsPanel.BorderRadius = 8;
            pendingRequestsPanel.BorderColor = Color.FromArgb(230, 230, 230);
            pendingRequestsPanel.BorderSize = 1;
            pendingRequestsPanel.Size = new Size(450, 300);
            pendingRequestsPanel.Location = new Point(470, 440);
            pendingRequestsPanel.BackColor = Color.White;
            pendingRequestsPanel.Padding = new Padding(15);
            pendingRequestsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

            // Pending Requests Title
            pendingRequestsTitle.AutoSize = false;
            pendingRequestsTitle.Size = new Size(420, 30);
            pendingRequestsTitle.Location = new Point(15, 15);
            pendingRequestsTitle.Font = new Font("Segoe UI Semibold", 12F);
            pendingRequestsTitle.Text = "Solicitudes Pendientes";
            pendingRequestsTitle.TextAlign = ContentAlignment.MiddleLeft;

            // Pending Requests Flow
            pendingRequestsFlow.AutoScroll = true;
            pendingRequestsFlow.Size = new Size(420, 240);
            pendingRequestsFlow.Location = new Point(15, 45);
            pendingRequestsFlow.BackColor = Color.White;
            pendingRequestsFlow.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            pendingRequestsFlow.FlowDirection = FlowDirection.TopDown;
            recentAppointmentsFlow.Dock = DockStyle.Fill;
            pendingRequestsFlow.WrapContents = false;

            // Add controls to panels
            statsPanel.Controls.Add(totalVehiclesLabel);
            statsPanel.Controls.Add(totalSalesLabel);
            statsPanel.Controls.Add(pendingRequestsLabel);
            statsPanel.Controls.Add(averagePriceLabel);

            recentSalesPanel.Controls.Add(recentAppointmentsTitle);
            recentSalesPanel.Controls.Add(recentAppointmentsFlow);

            pendingRequestsPanel.Controls.Add(pendingRequestsTitle);
            pendingRequestsPanel.Controls.Add(pendingRequestsFlow);

            salesChartPanel.Controls.Add(salesChart);
            vehiclesChartPanel.Controls.Add(vehiclesChart);

            // Add panels to control
            this.Controls.Add(statsPanel);
            this.Controls.Add(salesChartPanel);
            this.Controls.Add(vehiclesChartPanel);
            this.Controls.Add(recentSalesPanel);
            this.Controls.Add(pendingRequestsPanel);

            this.Dock = DockStyle.Fill;
            //this.BackColor = secondaryColor;
            this.Padding = new Padding(20);

            // Handle resize event
            this.Resize += DashboardControl_Resize;
        }

        private void DashboardControl_Resize(object sender, EventArgs e)
        {

            int newWidth = Math.Max(0, salesChartPanel.Width - 30);
            int newHeight = Math.Max(0, salesChartPanel.Height - 30);

            int halfWidth = (this.Width - 40 - 20) / 2;

            statsPanel.Width = newWidth;


            salesChartPanel.Width = halfWidth;
            vehiclesChartPanel.Width = halfWidth;
            vehiclesChartPanel.Location = new Point(halfWidth + 40, 120);

            salesChart.Size = new Size(newWidth, newHeight);
            salesChart.Location = new Point(15, 15);

            vehiclesChart.Size = new Size(newWidth, newHeight);
            
            vehiclesChart.Location = new Point(15, 15);

            recentSalesPanel.Width = halfWidth;
            
            pendingRequestsPanel.Width = halfWidth;
            pendingRequestsPanel.Location = new Point(halfWidth + 40, 440);

            recentAppointmentsTitle.Width = recentSalesPanel.Width - 30;
            pendingRequestsTitle.Width = pendingRequestsPanel.Width - 30;

            //recentSalesFlow.Width = recentSalesPanel.Width - 30;
            //recentSalesFlow.Height = recentSalesPanel.Height - 60;
            pendingRequestsFlow.Width = pendingRequestsPanel.Width - 30;

            // Limitar altura de recentSalesPanel para que no se salga de la ventana
            int maxHeight = this.ClientSize.Height - recentSalesPanel.Location.Y - 20;
            recentSalesPanel.Height = maxHeight-5;

            // Ajustar recentSalesFlow para ocupar el área interna
            recentAppointmentsFlow.Width = recentSalesPanel.Width - recentSalesPanel.Padding.Left - recentSalesPanel.Padding.Right;
            recentAppointmentsFlow.Height = recentSalesPanel.Height;
        }

        public async void LoadData()
        {
            try
            {
                // Load vehicles
                var vehicles = await apiClient.GetVehiclesAsync();
                totalVehiclesLabel.Text = $"Vehículos Totales\n{vehicles.Count}";

                // Agrupar vehículos por mes
                var monthlyVehicles = vehicles
                    .GroupBy(v => v.Brand )
                    .Select(g => new
                    {
                        Brand = g.Key,
                        Count = g.Count()
                    })
                    .ToList();

                // Graficar vehículos
                vehiclesChart.Series.Clear();
                var vehiclesSeries = new Series("Vehículos")
                {
                    ChartType = SeriesChartType.Column,
                    Color = Color.FromArgb(72, 201, 176)
                };
                foreach (var entry in monthlyVehicles)
                {
                    vehiclesSeries.Points.AddXY(entry.Brand, entry.Count);
                }
                vehiclesChart.Series.Add(vehiclesSeries);

                // Calculate average price
                decimal totalPrice = 0;
                foreach (var vehicle in vehicles)
                {
                    totalPrice += vehicle.Price;
                }
                decimal averagePrice = vehicles.Count > 0 ? totalPrice / vehicles.Count : 0;
                averagePriceLabel.Text = $"Precio Medio Vehículos\n{averagePrice:N0} €";

                // Load sales
                var sales = await apiClient.GetSalesAsync();
                totalSalesLabel.Text = $"Ventas Totales\n{sales.Count}";

                // Agrupar ventas por mes
                var monthlySales = sales
                    .GroupBy(s => new { s.SaleDate.Year, s.SaleDate.Month })
                    .Select(g => new
                    {
                        Month = new DateTime(g.Key.Year, g.Key.Month, 1),
                        Total = g.Sum(s => s.SalePrice)
                    })
                    .OrderBy(g => g.Month)
                    .ToList();

                // Obtener solo los últimos 3 meses
                monthlySales = monthlySales
                    .OrderByDescending(x => x.Month)
                    .Take(3)
                    .OrderBy(x => x.Month) // Ordenar de nuevo cronológicamente
                    .ToList();

                // Graficar ventas
                salesChart.Series.Clear();
                var salesSeries = new Series("Ventas")
                {
                    ChartType = SeriesChartType.Column,
                    Color = Color.FromArgb(100, 149, 237) // Primary color
                };

                //Calculo los beneficios totales
                decimal totalSales = 0;
                foreach (var sale in monthlySales)
                {
                    salesSeries.Points.AddXY(sale.Month.ToString("MMM yyyy"), sale.Total);
                    totalSales += sale.Total;
                }

                salesChart.Series.Clear();
                salesChart.Series.Add(salesSeries);
                salesChart.Titles.Clear();
                salesChart.Titles.Add(new Title($"Ventas \n Beneficio: {totalSales} €", Docking.Top, new Font("Segoe UI Semibold", 12F), Color.FromArgb(51, 51, 51)));


                // Load requests
                var appointments = await apiClient.GetAppointmentsAsync();
                var requests = await apiClient.GetRequestsAsync();
                int pendingRequests = 0;
                foreach (var request in requests) { pendingRequests++; }


                pendingRequestsLabel.Text = $"Solicitudes Pendientes\n{pendingRequests}";

                // Populate pending requests
                PopulatePendingRequests(requests);

                // Populate recent sales
                PopulateRecentAppointments(appointments);

                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateRecentAppointments(List<Appointment> appointments)
        {
            recentAppointmentsFlow.Controls.Clear();

            // Sort sales by date (newest first)
            appointments.Sort((a, b) => b.AppointmentDate.CompareTo(a.AppointmentDate));

            // Take only the 5 most recent sales
            int count = Math.Min(appointments.Count, 5);
            for (int i = 0; i < count; i++)
            {
                var sale = appointments[i];

                RoundedPanel salePanel = new RoundedPanel
                {
                    Size = new Size(recentAppointmentsFlow.Width - 20, 80),
                    BorderRadius = 4,
                    BorderColor = Color.FromArgb(230, 230, 230),
                    BorderSize = 1,
                    BackColor = Color.White,
                    Margin = new Padding(0, 0, 0, 10)
                };

                Label customerLabel = new Label
                {
                    AutoSize = false,
                    Size = new Size(200, 25),
                    Location = new Point(15, 10),
                    Font = new Font("Segoe UI Semibold", 10F),
                    ForeColor = textColor,
                    Text = sale.client.Id,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                Label vehicleLabel = new Label
                {
                    AutoSize = false,
                    Size = new Size(200, 20),
                    Location = new Point(15, 35),
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = accentColor,
                    Text = sale.Vehicle != null ? $"{sale.Vehicle.Id} {sale.Vehicle.Brand} {sale.Vehicle.Model}" : "",
                    TextAlign = ContentAlignment.MiddleLeft
                };

                Label priceLabel = new Label
                {
                    AutoSize = false,
                    Size = new Size(100, 25),
                    Location = new Point(salePanel.Width - 115, 10),
                    Font = new Font("Segoe UI Semibold", 10F),
                    ForeColor = primaryColor,
                    Text = $"{sale.AppointmentPrice:N0} €",
                    TextAlign = ContentAlignment.MiddleRight
                };

                Label dateLabel = new Label
                {
                    AutoSize = false,
                    Size = new Size(100, 20),
                    Location = new Point(salePanel.Width - 115, 35),
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = accentColor,
                    Text = sale.AppointmentDate.ToShortDateString(),
                    TextAlign = ContentAlignment.MiddleRight
                };

                salePanel.Controls.Add(customerLabel);
                salePanel.Controls.Add(vehicleLabel);
                salePanel.Controls.Add(priceLabel);
                salePanel.Controls.Add(dateLabel);

                recentAppointmentsFlow.Controls.Add(salePanel);
            }

            if (count == 0)
            {
                Label noSalesLabel = new Label
                {
                    AutoSize = false,
                    Size = new Size(recentAppointmentsFlow.Width - 20, 80),
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = accentColor,
                    Text = "No hay ventas recientes",
                    TextAlign = ContentAlignment.MiddleCenter
                };

                recentAppointmentsFlow.Controls.Add(noSalesLabel);
            }
        }

        private void PopulatePendingRequests(List<Request> requests)
        {
            pendingRequestsFlow.Controls.Clear();

            // Filter pending requests
            var pendingReqs = requests.FindAll(r => r.Status == RequestStatus.Pending || r.Status == RequestStatus.Scheduled);

            // Sort by date (newest first)
            pendingReqs.Sort((a, b) => b.RequestDate.CompareTo(a.RequestDate));

            // Take only the 5 most recent requests
            int count = Math.Min(pendingReqs.Count, 5);
            for (int i = 0; i < count; i++)
            {
                var request = pendingReqs[i];

                RoundedPanel requestPanel = new RoundedPanel
                {
                    Size = new Size(pendingRequestsFlow.Width - 20, 80),
                    BorderRadius = 4,
                    BorderColor = Color.FromArgb(230, 230, 230),
                    BorderSize = 1,
                    BackColor = Color.White,
                    Margin = new Padding(0, 0, 0, 10)
                };

                Label customerLabel = new Label
                {
                    AutoSize = false,
                    Size = new Size(200, 25),
                    Location = new Point(15, 10),
                    Font = new Font("Segoe UI Semibold", 10F),
                    ForeColor = textColor,
                    Text = request.client.Name,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                Label vehicleLabel = new Label
                {
                    AutoSize = false,
                    Size = new Size(200, 20),
                    Location = new Point(15, 35),
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = accentColor,
                    Text = request.Vehicle != null ? $"{request.Vehicle.Id} {request.Vehicle.Brand} {request.Vehicle.Model}" : "",
                    TextAlign = ContentAlignment.MiddleLeft
                };

                Label statusLabel = new Label
                {
                    AutoSize = false,
                    Size = new Size(100, 25),
                    Location = new Point(requestPanel.Width - 115, 10),
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.Black,
                    Text = request.Status == RequestStatus.Pending ? "Pendiente" : "Programada",
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = request.Status == RequestStatus.Pending ?
                        Color.FromArgb(255, 243, 205) : Color.FromArgb(207, 226, 255)
                };

                Label dateLabel = new Label
                {
                    AutoSize = false,
                    Size = new Size(100, 20),
                    Location = new Point(requestPanel.Width - 115, 45),
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = accentColor,
                    Text = request.RequestDate.ToShortDateString(),
                    TextAlign = ContentAlignment.MiddleRight
                };

                requestPanel.Controls.Add(customerLabel);
                requestPanel.Controls.Add(vehicleLabel);
                requestPanel.Controls.Add(statusLabel);
                requestPanel.Controls.Add(dateLabel);

                pendingRequestsFlow.Controls.Add(requestPanel);
            }

            if (count == 0)
            {
                Label noRequestsLabel = new Label
                {
                    AutoSize = false,
                    Size = new Size(pendingRequestsFlow.Width - 20, 80),
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = accentColor,
                    Text = "No hay solicitudes pendientes",
                    TextAlign = ContentAlignment.MiddleCenter
                };

                pendingRequestsFlow.Controls.Add(noRequestsLabel);
            }
        }
    }
}
