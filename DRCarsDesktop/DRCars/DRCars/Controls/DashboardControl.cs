using System;
using System.Collections.Generic;
using System.Drawing;
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
        private Chart salesChart;
        private Chart vehiclesChart;
        private Label totalVehiclesLabel;
        private Label totalSalesLabel;
        private Label pendingRequestsLabel;
        private Label averagePriceLabel;

        private ApiClient apiClient;

        public DashboardControl()
        {
            apiClient = new ApiClient();
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            statsPanel = new RoundedPanel();
            salesChartPanel = new RoundedPanel();
            vehiclesChartPanel = new RoundedPanel();
            salesChart = new Chart();
            vehiclesChart = new Chart();
            totalVehiclesLabel = new Label();
            totalSalesLabel = new Label();
            pendingRequestsLabel = new Label();
            averagePriceLabel = new Label();

            // Stats Panel
            statsPanel.BorderRadius = 15;
            statsPanel.BorderColor = Color.FromArgb(220, 220, 220);
            statsPanel.BorderSize = 1;
            statsPanel.Dock = DockStyle.Top;
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

            // Sales Chart Panel
            salesChartPanel.BorderRadius = 15;
            salesChartPanel.BorderColor = Color.FromArgb(220, 220, 220);
            salesChartPanel.BorderSize = 1;
            salesChartPanel.Size = new Size(450, 350);
            salesChartPanel.Location = new Point(0, 120);
            salesChartPanel.BackColor = Color.White;
            salesChartPanel.Padding = new Padding(10);
            salesChartPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;

            // Sales Chart
            salesChart.Size = new Size(430, 330);
            salesChart.Location = new Point(10, 10);
            salesChart.BackColor = Color.White;
            salesChart.BorderlineColor = Color.White;
            salesChart.Titles.Add(new Title("Ventas Mensuales", Docking.Top, new Font("Segoe UI", 12F, FontStyle.Bold), Color.Black));

            // Configure sales chart
            ChartArea salesChartArea = new ChartArea("SalesChartArea");
            salesChartArea.AxisX.Title = "Mes";
            salesChartArea.AxisY.Title = "Cantidad";
            salesChartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            salesChartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            salesChartArea.BackColor = Color.White;
            salesChart.ChartAreas.Add(salesChartArea);

            // Add series for sales chart
            Series salesSeries = new Series("Ventas Cerradas");
            salesSeries.ChartType = SeriesChartType.Column;
            salesSeries.Color = Color.FromArgb(0, 120, 215);
            salesChart.Series.Add(salesSeries);

            Series requestsSeries = new Series("Ventas Solicitadas");
            requestsSeries.ChartType = SeriesChartType.Column;
            requestsSeries.Color = Color.FromArgb(255, 140, 0);
            salesChart.Series.Add(requestsSeries);

            // Add legend for sales chart
            Legend salesLegend = new Legend("SalesLegend");
            salesLegend.Docking = Docking.Bottom;
            salesChart.Legends.Add(salesLegend);

            // Vehicles Chart Panel
            vehiclesChartPanel.BorderRadius = 15;
            vehiclesChartPanel.BorderColor = Color.FromArgb(220, 220, 220);
            vehiclesChartPanel.BorderSize = 1;
            vehiclesChartPanel.Size = new Size(450, 350);
            vehiclesChartPanel.Location = new Point(470, 120);
            vehiclesChartPanel.BackColor = Color.White;
            vehiclesChartPanel.Padding = new Padding(10);
            vehiclesChartPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

            // Vehicles Chart
            vehiclesChart.Size = new Size(430, 330);
            vehiclesChart.Location = new Point(10, 10);
            vehiclesChart.BackColor = Color.White;
            vehiclesChart.BorderlineColor = Color.White;
            vehiclesChart.Titles.Add(new Title("Entrada de Vehículos", Docking.Top, new Font("Segoe UI", 12F, FontStyle.Bold), Color.Black));

            // Configure vehicles chart
            ChartArea vehiclesChartArea = new ChartArea("VehiclesChartArea");
            vehiclesChartArea.AxisX.Title = "Mes";
            vehiclesChartArea.AxisY.Title = "Cantidad";
            vehiclesChartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            vehiclesChartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            vehiclesChartArea.BackColor = Color.White;
            vehiclesChart.ChartAreas.Add(vehiclesChartArea);

            // Add series for vehicles chart
            Series vehiclesSeries = new Series("Vehículos");
            vehiclesSeries.ChartType = SeriesChartType.Line;
            vehiclesSeries.Color = Color.FromArgb(0, 150, 50);
            vehiclesSeries.BorderWidth = 3;
            vehiclesSeries.MarkerStyle = MarkerStyle.Circle;
            vehiclesSeries.MarkerSize = 8;
            vehiclesChart.Series.Add(vehiclesSeries);

            // Add controls to panels
            statsPanel.Controls.Add(totalVehiclesLabel);
            statsPanel.Controls.Add(totalSalesLabel);
            statsPanel.Controls.Add(pendingRequestsLabel);
            statsPanel.Controls.Add(averagePriceLabel);

            salesChartPanel.Controls.Add(salesChart);
            vehiclesChartPanel.Controls.Add(vehiclesChart);

            // Add panels to control
            this.Controls.Add(statsPanel);
            this.Controls.Add(salesChartPanel);
            this.Controls.Add(vehiclesChartPanel);

            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.Padding = new Padding(20);

            // Handle resize event
            this.Resize += DashboardControl_Resize;
        }

        private void DashboardControl_Resize(object sender, EventArgs e)
        {
            // Adjust panel positions and sizes when control is resized
            int halfWidth = (this.Width - 40 - 20) / 2; // 40 for padding, 20 for gap

            salesChartPanel.Width = halfWidth;
            vehiclesChartPanel.Width = halfWidth;
            vehiclesChartPanel.Location = new Point(salesChartPanel.Width + 40, 120);

            salesChart.Width = salesChartPanel.Width - 20;
            vehiclesChart.Width = vehiclesChartPanel.Width - 20;
        }

        private async void LoadData()
        {
            try
            {
                // Load vehicles
                var vehicles = await apiClient.GetVehiclesAsync();
                totalVehiclesLabel.Text = $"Vehículos Totales\n{vehicles.Count}";

                // Calculate average price
                decimal totalPrice = 0;
                foreach (var vehicle in vehicles)
                {
                    totalPrice += vehicle.Price;
                }
                decimal averagePrice = vehicles.Count > 0 ? totalPrice / vehicles.Count : 0;
                averagePriceLabel.Text = $"Precio Medio\n{averagePrice:N0} €";

                // Load sales
                var sales = await apiClient.GetSalesAsync();
                totalSalesLabel.Text = $"Ventas Totales\n{sales.Count}";

                // Load sale requests
                var requests = await apiClient.GetSaleRequestsAsync();
                int pendingRequests = 0;
                foreach (var request in requests)
                {
                    if (request.Status == RequestStatus.Pending || request.Status == RequestStatus.Scheduled)
                    {
                        pendingRequests++;
                    }
                }
                pendingRequestsLabel.Text = $"Solicitudes Pendientes\n{pendingRequests}";

                // Populate charts
                PopulateSalesChart(sales, requests);
                PopulateVehiclesChart(vehicles);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateSalesChart(List<Sale> sales, List<SaleRequest> requests)
        {
            // Clear existing data
            salesChart.Series["Ventas Cerradas"].Points.Clear();
            salesChart.Series["Ventas Solicitadas"].Points.Clear();

            // Group sales by month
            Dictionary<string, int> salesByMonth = new Dictionary<string, int>();
            Dictionary<string, int> requestsByMonth = new Dictionary<string, int>();

            // Initialize months (last 6 months)
            for (int i = 5; i >= 0; i--)
            {
                string month = DateTime.Now.AddMonths(-i).ToString("MMM");
                salesByMonth[month] = 0;
                requestsByMonth[month] = 0;
            }

            // Count sales by month
            foreach (var sale in sales)
            {
                string month = sale.SaleDate.ToString("MMM");
                if (salesByMonth.ContainsKey(month))
                {
                    salesByMonth[month]++;
                }
            }

            // Count requests by month
            foreach (var request in requests)
            {
                string month = request.CreatedAt.ToString("MMM");
                if (requestsByMonth.ContainsKey(month))
                {
                    requestsByMonth[month]++;
                }
            }

            // Add data to chart
            foreach (var month in salesByMonth.Keys)
            {
                salesChart.Series["Ventas Cerradas"].Points.AddXY(month, salesByMonth[month]);
                salesChart.Series["Ventas Solicitadas"].Points.AddXY(month, requestsByMonth[month]);
            }
        }

        private void PopulateVehiclesChart(List<Vehicle> vehicles)
        {
            // Clear existing data
            vehiclesChart.Series["Vehículos"].Points.Clear();

            // Group vehicles by month
            Dictionary<string, int> vehiclesByMonth = new Dictionary<string, int>();

            // Initialize months (last 6 months)
            for (int i = 5; i >= 0; i--)
            {
                string month = DateTime.Now.AddMonths(-i).ToString("MMM");
                vehiclesByMonth[month] = 0;
            }

            // Count vehicles by month
            foreach (var vehicle in vehicles)
            {
                string month = vehicle.CreatedAt.ToString("MMM");
                if (vehiclesByMonth.ContainsKey(month))
                {
                    vehiclesByMonth[month]++;
                }
            }

            // Add data to chart
            foreach (var month in vehiclesByMonth.Keys)
            {
                vehiclesChart.Series["Vehículos"].Points.AddXY(month, vehiclesByMonth[month]);
            }
        }
    }
}
