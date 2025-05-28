using System;
using System.Drawing;
using System.Windows.Forms;
using DRCars.Models;
using DRCars.Utils;

namespace DRCars.Controls
{
    public class VehicleCard : UserControl
    {
        private Vehicle _vehicle;
        private VehicleImageData _imageData;
        private RoundedPanel mainPanel;
        private PictureBox vehicleImage;
        private Label brandModelLabel;
        private Label yearLabel;
        private Label priceLabel;
        private Label detailsLabel;
        private Label statusLabel;
        private RoundedButton viewDetailsButton;
        private Label imageTypeLabel; // Nuevo: mostrar si es WebP o SVG

        // Servicios
        private FirestoreImageService _imageService;

        // Colores de Odoo
        private Color primaryColor = Color.FromArgb(0, 160, 157); // Verde Odoo
        private Color secondaryColor = Color.FromArgb(242, 242, 242); // Gris claro
        private Color textColor = Color.FromArgb(51, 51, 51); // Texto oscuro
        private Color accentColor = Color.FromArgb(108, 117, 125); // Gris para detalles

        public event EventHandler<Vehicle> ViewDetailsClicked;

        public Vehicle Vehicle
        {
            get { return _vehicle; }
            set
            {
                _vehicle = value;
                UpdateCardInfo();
                LoadVehicleImage();
            }
        }

        public VehicleCard()
        {
            _imageService = new FirestoreImageService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            mainPanel = new RoundedPanel();
            vehicleImage = new PictureBox();
            brandModelLabel = new Label();
            yearLabel = new Label();
            priceLabel = new Label();
            detailsLabel = new Label();
            statusLabel = new Label();
            viewDetailsButton = new RoundedButton();
            imageTypeLabel = new Label(); // Nuevo

            // Main Panel
            mainPanel.BorderRadius = 8;
            mainPanel.BorderColor = Color.FromArgb(230, 230, 230);
            mainPanel.BorderSize = 1;
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.White;
            mainPanel.Padding = new Padding(0);

            // Vehicle Image
            vehicleImage.Size = new Size(300, 180);
            vehicleImage.Location = new Point(0, 0);
            vehicleImage.SizeMode = PictureBoxSizeMode.Zoom;
            vehicleImage.BackColor = Color.FromArgb(245, 245, 245);
            vehicleImage.Dock = DockStyle.Top;

            // Image Type Label (nuevo)
            imageTypeLabel.AutoSize = false;
            imageTypeLabel.Size = new Size(60, 20);
            imageTypeLabel.Location = new Point(240, 5);
            imageTypeLabel.Font = new Font("Segoe UI", 8F);
            imageTypeLabel.ForeColor = Color.White;
            imageTypeLabel.BackColor = Color.FromArgb(100, 0, 160, 157);
            imageTypeLabel.TextAlign = ContentAlignment.MiddleCenter;
            imageTypeLabel.Text = "";
            imageTypeLabel.Visible = false;

            // Brand Model Label
            brandModelLabel.AutoSize = false;
            brandModelLabel.Size = new Size(280, 30);
            brandModelLabel.Location = new Point(15, 195);
            brandModelLabel.Font = new Font("Segoe UI Semibold", 12F);
            brandModelLabel.ForeColor = textColor;
            brandModelLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Year Label
            yearLabel.AutoSize = false;
            yearLabel.Size = new Size(280, 20);
            yearLabel.Location = new Point(15, 225);
            yearLabel.Font = new Font("Segoe UI", 9F);
            yearLabel.ForeColor = accentColor;
            yearLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Price Label
            priceLabel.AutoSize = false;
            priceLabel.Size = new Size(280, 30);
            priceLabel.Location = new Point(15, 245);
            priceLabel.Font = new Font("Segoe UI Semibold", 12F);
            priceLabel.ForeColor = primaryColor;
            priceLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Details Label
            detailsLabel.AutoSize = false;
            detailsLabel.Size = new Size(280, 40);
            detailsLabel.Location = new Point(15, 275);
            detailsLabel.Font = new Font("Segoe UI", 9F);
            detailsLabel.ForeColor = accentColor;
            detailsLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Status Label
            statusLabel.AutoSize = false;
            statusLabel.Size = new Size(100, 25);
            statusLabel.Location = new Point(15, 320);
            statusLabel.Font = new Font("Segoe UI", 9F);
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            statusLabel.BackColor = Color.FromArgb(240, 240, 240);
            statusLabel.BorderStyle = BorderStyle.None;

            // View Details Button
            viewDetailsButton.Text = "Ver Detalles";
            viewDetailsButton.Size = new Size(150, 35);
            viewDetailsButton.Location = new Point(135, 320);
            viewDetailsButton.BorderRadius = 4;
            viewDetailsButton.BackColor = primaryColor;
            viewDetailsButton.ForeColor = Color.White;
            viewDetailsButton.Click += ViewDetailsButton_Click;

            // Add controls to panel
            mainPanel.Controls.Add(vehicleImage);
            mainPanel.Controls.Add(imageTypeLabel); // Añadir nuevo label
            mainPanel.Controls.Add(brandModelLabel);
            mainPanel.Controls.Add(yearLabel);
            mainPanel.Controls.Add(priceLabel);
            mainPanel.Controls.Add(detailsLabel);
            mainPanel.Controls.Add(statusLabel);
            mainPanel.Controls.Add(viewDetailsButton);

            // Add panel to control
            this.Controls.Add(mainPanel);
            this.Size = new Size(300, 380);
        }

        private async void LoadVehicleImage()
        {
            if (_vehicle == null) return;

            try
            {
                // Cargar múltiples imágenes desde Firestore
                var multipleImages = await _imageService.GetMultipleVehicleImagesAsync(_vehicle.Id.ToString());

                if (multipleImages?.Images != null)
                {
                    // Buscar la primera imagen disponible
                    VehicleImageData primaryImage = null;

                    // Prioridad: imagen 1, luego 2, 3, 4, 5
                    for (int i = 1; i <= 5; i++)
                    {
                        var key = $"{_vehicle.Id}.{i}";
                        if (multipleImages.Images.ContainsKey(key) && multipleImages.Images[key] != null)
                        {
                            primaryImage = multipleImages.Images[key];
                            break;
                        }
                    }

                    if (primaryImage?.HasAnyImage() == true)
                    {
                        // Prioridad: SVG > Medium > Thumbnail
                        if (!string.IsNullOrEmpty(primaryImage.VectorSVG))
                        {
                            LoadSVGImage(primaryImage.VectorSVG);
                            imageTypeLabel.Text = "SVG";
                            imageTypeLabel.BackColor = Color.FromArgb(100, 76, 175, 80);
                            imageTypeLabel.Visible = true;
                        }
                        else if (!string.IsNullOrEmpty(primaryImage.MediumBase64))
                        {
                            LoadBase64Image(primaryImage.MediumBase64);
                            imageTypeLabel.Text = "WebP";
                            imageTypeLabel.BackColor = Color.FromArgb(100, 33, 150, 243);
                            imageTypeLabel.Visible = true;
                        }
                        else if (!string.IsNullOrEmpty(primaryImage.ThumbnailBase64))
                        {
                            LoadBase64Image(primaryImage.ThumbnailBase64);
                            imageTypeLabel.Text = "WebP";
                            imageTypeLabel.BackColor = Color.FromArgb(100, 33, 150, 243);
                            imageTypeLabel.Visible = true;
                        }

                        // Mostrar contador de imágenes si hay más de una
                        var totalImages = multipleImages.GetImageCount();
                        if (totalImages > 1)
                        {
                            imageTypeLabel.Text += $" ({totalImages})";
                        }
                    }
                    else
                    {
                        LoadPlaceholderImage();
                        imageTypeLabel.Visible = false;
                    }
                }
                else
                {
                    LoadPlaceholderImage();
                    imageTypeLabel.Visible = false;
                }
            }
            catch (Exception ex)
            {
                LoadPlaceholderImage();
                imageTypeLabel.Visible = false;
                Console.WriteLine($"Error cargando imágenes: {ex.Message}");
            }
        }

        private void LoadBase64Image(string base64String)
        {
            try
            {
                var image = _imageService.Base64ToImage(base64String);
                if (image != null)
                {
                    vehicleImage.Image = image;
                }
                else
                {
                    LoadPlaceholderImage();
                }
            }
            catch
            {
                LoadPlaceholderImage();
            }
        }

        private void LoadSVGImage(string svgString)
        {
            try
            {
                // Para SVG, crear una imagen temporal con el contenido
                // En una implementación real usarías una librería SVG como Svg.NET
                // Por ahora, mostrar un placeholder especial para SVG
                LoadVectorPlaceholder();
            }
            catch
            {
                LoadPlaceholderImage();
            }
        }

        private void LoadVectorPlaceholder()
        {
            // Crear imagen placeholder para vectores
            var bitmap = new Bitmap(300, 180);
            using (var g = Graphics.FromImage(bitmap))
            {
                // Fondo degradado
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    new Rectangle(0, 0, 300, 180),
                    Color.FromArgb(0, 160, 157),
                    Color.FromArgb(0, 120, 117),
                    45f))
                {
                    g.FillRectangle(brush, 0, 0, 300, 180);
                }

                // Texto "VECTOR"
                using (var font = new Font("Segoe UI", 16, FontStyle.Bold))
                using (var textBrush = new SolidBrush(Color.White))
                {
                    var text = "🎨 VECTOR";
                    var size = g.MeasureString(text, font);
                    var x = (300 - size.Width) / 2;
                    var y = (180 - size.Height) / 2;
                    g.DrawString(text, font, textBrush, x, y);
                }
            }

            vehicleImage.Image = bitmap;
        }

        private void LoadPlaceholderImage()
        {
            // Crear imagen placeholder
            var bitmap = new Bitmap(300, 180);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(245, 245, 245)), 0, 0, 300, 180);

                using (var font = new Font("Segoe UI", 12))
                using (var brush = new SolidBrush(Color.FromArgb(108, 117, 125)))
                {
                    var text = "🚗 Sin imagen";
                    var size = g.MeasureString(text, font);
                    var x = (300 - size.Width) / 2;
                    var y = (180 - size.Height) / 2;
                    g.DrawString(text, font, brush, x, y);
                }
            }

            vehicleImage.Image = bitmap;
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
            }
        }

        private string GetStatusText(VehicleStatus status)
        {
            switch (status)
            {
                case VehicleStatus.InStock: return "En Stock";
                case VehicleStatus.InGarage: return "En Garaje";
                case VehicleStatus.ForSale: return "En Venta";
                case VehicleStatus.Sold: return "Vendido";
                case VehicleStatus.InRepair: return "En Reparación";
                default: return "Desconocido";
            }
        }

        private Color GetStatusColor(VehicleStatus status)
        {
            switch (status)
            {
                case VehicleStatus.InStock: return Color.FromArgb(209, 231, 221);
                case VehicleStatus.InGarage: return Color.FromArgb(207, 226, 255);
                case VehicleStatus.ForSale: return Color.FromArgb(255, 243, 205);
                case VehicleStatus.Sold: return Color.FromArgb(248, 215, 218);
                case VehicleStatus.InRepair: return Color.FromArgb(255, 228, 208);
                default: return Color.FromArgb(240, 240, 240);
            }
        }

        private void ViewDetailsButton_Click(object sender, EventArgs e)
        {
            if (_vehicle != null && ViewDetailsClicked != null)
            {
                ViewDetailsClicked(this, _vehicle);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Posicionar el label de tipo de imagen sobre la imagen
            imageTypeLabel.BringToFront();
        }
    }
}
