using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using DRCars.Controls;
using DRCars.Models;
using DRCars.Utils;

namespace DRCars.Forms
{
    public partial class VehicleForm : Form
    {
        // Cambiar las variables de imagen por arrays
        private Label imageLabel;
        private RoundedButton[] uploadImageButtons;
        private PictureBox[] imagePreviews;
        private Label[] imageInfoLabels;
        private string[] _selectedImagePaths;
        private Image[] _originalImages;

        private RoundedPanel mainPanel;
        private Label titleLabel;
        private Label brandLabel;
        private RoundedTextBox brandTextBox;
        private Label modelLabel;
        private RoundedTextBox modelTextBox;
        private Label yearLabel;
        private RoundedTextBox yearTextBox;
        private Label priceLabel;
        private RoundedTextBox priceTextBox;
        private Label categoryLabel;
        private ComboBox categoryComboBox;
        private Label fuelTypeLabel;
        private ComboBox fuelTypeComboBox;
        private Label transmissionLabel;
        private ComboBox transmissionComboBox;
        private Label kilometersLabel;
        private RoundedTextBox kilometersTextBox;
        private Label statusLabel;
        private ComboBox statusComboBox;
        private RoundedButton saveButton;
        private RoundedButton cancelButton;

        private Vehicle _vehicle;
        private bool _isEditMode;
        private ApiClient apiClient;
        private FirestoreImageService _firestoreImageService;

        public VehicleForm(Vehicle vehicle = null)
        {
            _vehicle = vehicle;
            _isEditMode = vehicle != null;
            apiClient = new ApiClient();
            _firestoreImageService = new FirestoreImageService();
            InitializeComponent();
            LoadVehicleData();
        }

        private void InitializeComponent()
        {
            mainPanel = new RoundedPanel();
            titleLabel = new Label();
            brandLabel = new Label();
            brandTextBox = new RoundedTextBox();
            modelLabel = new Label();
            modelTextBox = new RoundedTextBox();
            yearLabel = new Label();
            yearTextBox = new RoundedTextBox();
            priceLabel = new Label();
            priceTextBox = new RoundedTextBox();
            categoryLabel = new Label();
            categoryComboBox = new ComboBox();
            fuelTypeLabel = new Label();
            fuelTypeComboBox = new ComboBox();
            transmissionLabel = new Label();
            transmissionComboBox = new ComboBox();
            kilometersLabel = new Label();
            kilometersTextBox = new RoundedTextBox();
            statusLabel = new Label();
            statusComboBox = new ComboBox();
            saveButton = new RoundedButton();
            cancelButton = new RoundedButton();

            // Form
            this.Text = _isEditMode ? "Editar Vehículo" : "Añadir Vehículo";
            this.Size = new Size(800, 750);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Main Panel
            mainPanel.BorderRadius = 15;
            mainPanel.BorderColor = Color.FromArgb(220, 220, 220);
            mainPanel.BorderSize = 1;
            mainPanel.Size = new Size(750, 700);
            mainPanel.Location = new Point((this.ClientSize.Width - 750) / 2, (this.ClientSize.Height - 700) / 2);
            mainPanel.BackColor = Color.White;
            mainPanel.Padding = new Padding(30);

            // Title Label
            titleLabel.AutoSize = false;
            titleLabel.Size = new Size(690, 40);
            titleLabel.Location = new Point(30, 20);
            titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Text = _isEditMode ? "Editar Vehículo" : "Añadir Vehículo";

            // Brand Label
            brandLabel.AutoSize = true;
            brandLabel.Location = new Point(30, 80);
            brandLabel.Font = new Font("Segoe UI", 9F);
            brandLabel.Text = "Marca";

            // Brand TextBox
            brandTextBox.Size = new Size(200, 40);
            brandTextBox.Location = new Point(30, 105);
            brandTextBox.BorderRadius = 10;
            brandTextBox.PlaceholderText = "Marca del vehículo";

            // Model Label
            modelLabel.AutoSize = true;
            modelLabel.Location = new Point(250, 80);
            modelLabel.Font = new Font("Segoe UI", 9F);
            modelLabel.Text = "Modelo";

            // Model TextBox
            modelTextBox.Size = new Size(200, 40);
            modelTextBox.Location = new Point(250, 105);
            modelTextBox.BorderRadius = 10;
            modelTextBox.PlaceholderText = "Modelo del vehículo";

            // Year Label
            yearLabel.AutoSize = true;
            yearLabel.Location = new Point(470, 80);
            yearLabel.Font = new Font("Segoe UI", 9F);
            yearLabel.Text = "Año";

            // Year TextBox
            yearTextBox.Size = new Size(200, 40);
            yearTextBox.Location = new Point(470, 105);
            yearTextBox.BorderRadius = 10;
            yearTextBox.PlaceholderText = "Año del vehículo";

            // Price Label
            priceLabel.AutoSize = true;
            priceLabel.Location = new Point(30, 155);
            priceLabel.Font = new Font("Segoe UI", 9F);
            priceLabel.Text = "Precio (€)";

            // Price TextBox
            priceTextBox.Size = new Size(200, 40);
            priceTextBox.Location = new Point(30, 180);
            priceTextBox.BorderRadius = 10;
            priceTextBox.PlaceholderText = "Precio del vehículo";

            // Category Label
            categoryLabel.AutoSize = true;
            categoryLabel.Location = new Point(250, 155);
            categoryLabel.Font = new Font("Segoe UI", 9F);
            categoryLabel.Text = "Categoría";

            // Category ComboBox
            categoryComboBox.Size = new Size(200, 40);
            categoryComboBox.Location = new Point(250, 180);
            categoryComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            categoryComboBox.Font = new Font("Segoe UI", 9F);
            categoryComboBox.Items.AddRange(new object[] { "Lujo", "Estándar", "Deportivo", "SUV", "Compacto" });

            // Fuel Type Label
            fuelTypeLabel.AutoSize = true;
            fuelTypeLabel.Location = new Point(470, 155);
            fuelTypeLabel.Font = new Font("Segoe UI", 9F);
            fuelTypeLabel.Text = "Combustible";

            // Fuel Type ComboBox
            fuelTypeComboBox.Size = new Size(200, 40);
            fuelTypeComboBox.Location = new Point(470, 180);
            fuelTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            fuelTypeComboBox.Font = new Font("Segoe UI", 9F);
            fuelTypeComboBox.Items.AddRange(new object[] { "Gasolina", "Diésel", "Híbrido", "Eléctrico" });

            // Transmission Label
            transmissionLabel.AutoSize = true;
            transmissionLabel.Location = new Point(30, 230);
            transmissionLabel.Font = new Font("Segoe UI", 9F);
            transmissionLabel.Text = "Transmisión";

            // Transmission ComboBox
            transmissionComboBox.Size = new Size(200, 40);
            transmissionComboBox.Location = new Point(30, 255);
            transmissionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            transmissionComboBox.Font = new Font("Segoe UI", 9F);
            transmissionComboBox.Items.AddRange(new object[] { "Manual", "Automática" });

            // Kilometers Label
            kilometersLabel.AutoSize = true;
            kilometersLabel.Location = new Point(250, 230);
            kilometersLabel.Font = new Font("Segoe UI", 9F);
            kilometersLabel.Text = "Kilómetros";

            // Kilometers TextBox
            kilometersTextBox.Size = new Size(200, 40);
            kilometersTextBox.Location = new Point(250, 255);
            kilometersTextBox.BorderRadius = 10;
            kilometersTextBox.PlaceholderText = "Kilómetros del vehículo";

            // Status Label
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(470, 230);
            statusLabel.Font = new Font("Segoe UI", 9F);
            statusLabel.Text = "Estado";

            // Status ComboBox
            statusComboBox.Size = new Size(200, 40);
            statusComboBox.Location = new Point(470, 255);
            statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            statusComboBox.Font = new Font("Segoe UI", 9F);
            statusComboBox.Items.AddRange(new object[] { "En Stock", "En Garaje", "En Venta", "Vendido" });

            // Image Section - Multiple Images (up to 5)
            imageLabel = new Label();
            uploadImageButtons = new RoundedButton[5];
            imagePreviews = new PictureBox[5];
            imageInfoLabels = new Label[5];
            _selectedImagePaths = new string[5];
            _originalImages = new Image[5];

            // Image Label
            imageLabel.AutoSize = true;
            imageLabel.Location = new Point(30, 305);
            imageLabel.Font = new Font("Segoe UI", 9F);
            imageLabel.Text = "Imágenes del Vehículo (máximo 5)";

            // Create 5 image upload sections
            for (int i = 0; i < 5; i++)
            {
                int imageIndex = i; // Capture for lambda

                // Upload Image Button
                uploadImageButtons[i] = new RoundedButton();
                uploadImageButtons[i].Text = $"📷 Imagen {i + 1}";
                uploadImageButtons[i].Size = new Size(130, 35);
                uploadImageButtons[i].Location = new Point(30 + (i * 140), 330);
                uploadImageButtons[i].BorderRadius = 8;
                uploadImageButtons[i].BackColor = Color.FromArgb(0, 160, 157);
                uploadImageButtons[i].ForeColor = Color.White;
                uploadImageButtons[i].Click += (s, e) => UploadImageButton_Click(s, e, imageIndex);

                // Image Preview
                imagePreviews[i] = new PictureBox();
                imagePreviews[i].Size = new Size(130, 80);
                imagePreviews[i].Location = new Point(30 + (i * 140), 375);
                imagePreviews[i].BorderStyle = BorderStyle.FixedSingle;
                imagePreviews[i].SizeMode = PictureBoxSizeMode.Zoom;
                imagePreviews[i].BackColor = Color.FromArgb(245, 245, 245);

                // Image Info Label
                imageInfoLabels[i] = new Label();
                imageInfoLabels[i].AutoSize = false;
                imageInfoLabels[i].Size = new Size(130, 40);
                imageInfoLabels[i].Location = new Point(30 + (i * 140), 460);
                imageInfoLabels[i].Font = new Font("Segoe UI", 8F);
                imageInfoLabels[i].Text = "Sin imagen";
                imageInfoLabels[i].ForeColor = Color.FromArgb(108, 117, 125);
                imageInfoLabels[i].TextAlign = ContentAlignment.TopCenter;

                // Add to panel
                mainPanel.Controls.Add(uploadImageButtons[i]);
                mainPanel.Controls.Add(imagePreviews[i]);
                mainPanel.Controls.Add(imageInfoLabels[i]);
            }

            // Save Button (Guardar - Azul - Derecha)
            saveButton.Text = "Guardar";
            saveButton.Size = new Size(180, 45);
            saveButton.Location = new Point(470, 520);
            saveButton.BorderRadius = 4;
            saveButton.BackColor = Color.FromArgb(0, 120, 215); // Azul
            saveButton.ForeColor = Color.White;
            saveButton.Click += SaveButton_Click;

            // Cancel Button (Cancelar - Rojo - Izquierda)
            cancelButton.Text = "Cancelar";
            cancelButton.Size = new Size(180, 45);
            cancelButton.Location = new Point(250, 520);
            cancelButton.BorderRadius = 4;
            cancelButton.BackColor = Color.FromArgb(220, 53, 69); // Rojo
            cancelButton.ForeColor = Color.White;
            cancelButton.Click += CancelButton_Click;

            // Add image label to panel
            mainPanel.Controls.Add(imageLabel);

            // Add controls to panel
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(brandLabel);
            mainPanel.Controls.Add(brandTextBox);
            mainPanel.Controls.Add(modelLabel);
            mainPanel.Controls.Add(modelTextBox);
            mainPanel.Controls.Add(yearLabel);
            mainPanel.Controls.Add(yearTextBox);
            mainPanel.Controls.Add(priceLabel);
            mainPanel.Controls.Add(priceTextBox);
            mainPanel.Controls.Add(categoryLabel);
            mainPanel.Controls.Add(categoryComboBox);
            mainPanel.Controls.Add(fuelTypeLabel);
            mainPanel.Controls.Add(fuelTypeComboBox);
            mainPanel.Controls.Add(transmissionLabel);
            mainPanel.Controls.Add(transmissionComboBox);
            mainPanel.Controls.Add(kilometersLabel);
            mainPanel.Controls.Add(kilometersTextBox);
            mainPanel.Controls.Add(statusLabel);
            mainPanel.Controls.Add(statusComboBox);
            mainPanel.Controls.Add(saveButton);
            mainPanel.Controls.Add(cancelButton);

            // Add panel to form
            this.Controls.Add(mainPanel);
        }

        
        private string GetTransmission(TransmissionType transmissionType)
        {
            switch (transmissionType.ToString())
            {
                case "MANUAL": return "Manual";
                case "AUTOMATICA": return "Automática";
                default: return "Desconocido";
            }
        }

        private string GetFuel(FuelType fuelType)
        {
            switch (fuelType.ToString())
            {
                case "GASOLINA": return "Gasolina";
                case "DIESEL": return "Diésel";
                case "HIBRIDO": return "Híbrido";
                case "ELECTRICO": return "Eléctrico";
                default: return "Desconocido";
            }
        }

        private void LoadVehicleData()
        {
            if (_isEditMode && _vehicle != null)
            {
                brandTextBox.Texts = _vehicle.Brand;
                modelTextBox.Texts = _vehicle.Model;
                yearTextBox.Texts = _vehicle.Year.ToString();
                priceTextBox.Texts = _vehicle.Price.ToString();
                categoryComboBox.SelectedItem = _vehicle.Category;
                fuelTypeComboBox.SelectedItem = GetFuel(_vehicle.FuelType);
                transmissionComboBox.SelectedItem = GetTransmission(_vehicle.TransmissionType);
                kilometersTextBox.Texts = _vehicle.Kilometers.ToString();
                statusComboBox.SelectedIndex = (int)_vehicle.Status;

                // Load existing image if available
                if (!string.IsNullOrEmpty(_vehicle.ImageUrl))
                {
                    LoadExistingImage(_vehicle.ImageUrl);
                }
            }
            else
            {
                // Set defaults for new vehicle
                categoryComboBox.SelectedIndex = 0;
                fuelTypeComboBox.SelectedIndex = 0;
                transmissionComboBox.SelectedIndex = 0;
                statusComboBox.SelectedIndex = 0;
            }
        }

        private void LoadExistingImage(string imagePath)
        {
            try
            {
                if (File.Exists(imagePath))
                {
                    using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                    {
                        _originalImages[0] = Image.FromStream(fs);
                        imagePreviews[0].Image = ResizeImage(_originalImages[0], imagePreviews[0].Size);
                        _selectedImagePaths[0] = imagePath;

                        var fileInfo = new FileInfo(imagePath);
                        imageInfoLabels[0].Text = $"{fileInfo.Name}\n{FormatFileSize(fileInfo.Length)}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Modify the UploadImageButton_Click method:
        private void UploadImageButton_Click(object sender, EventArgs e, int imageIndex)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = $"Seleccionar imagen {imageIndex + 1} del vehículo";
                openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Todos los archivos|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Console.WriteLine($"🔄 Cargando imagen {imageIndex + 1}: {openFileDialog.FileName}");

                        // Dispose previous image if exists
                        if (_originalImages[imageIndex] != null)
                        {
                            _originalImages[imageIndex].Dispose();
                            _originalImages[imageIndex] = null;
                            Console.WriteLine($"  - Imagen anterior {imageIndex + 1} liberada de memoria");
                        }

                        // Dispose previous preview if exists
                        if (imagePreviews[imageIndex].Image != null)
                        {
                            imagePreviews[imageIndex].Image.Dispose();
                            imagePreviews[imageIndex].Image = null;
                        }

                        // Validar que el archivo existe
                        if (!File.Exists(openFileDialog.FileName))
                        {
                            throw new FileNotFoundException($"El archivo no existe: {openFileDialog.FileName}");
                        }

                        var fileInfo = new FileInfo(openFileDialog.FileName);
                        Console.WriteLine($"  - Archivo: {fileInfo.Name}");
                        Console.WriteLine($"  - Tamaño: {FormatFileSize(fileInfo.Length)}");

                        // Validar tamaño del archivo (máximo 10MB)
                        if (fileInfo.Length > 10 * 1024 * 1024)
                        {
                            throw new Exception("El archivo es demasiado grande. Máximo permitido: 10MB");
                        }

                        // Load the selected image using a more robust method
                        _originalImages[imageIndex] = LoadImageSafely(openFileDialog.FileName);

                        if (_originalImages[imageIndex] == null)
                        {
                            throw new Exception("No se pudo cargar la imagen. Formato no válido o archivo corrupto.");
                        }

                        Console.WriteLine($"  - Imagen cargada: {_originalImages[imageIndex].Width}x{_originalImages[imageIndex].Height}");
                        Console.WriteLine($"  - Formato: {_originalImages[imageIndex].RawFormat}");
                        Console.WriteLine($"  - PixelFormat: {_originalImages[imageIndex].PixelFormat}");

                        // Create preview image
                        imagePreviews[imageIndex].Image = ResizeImage(_originalImages[imageIndex], imagePreviews[imageIndex].Size);

                        // Store the selected path
                        _selectedImagePaths[imageIndex] = openFileDialog.FileName;

                        // Update info label
                        imageInfoLabels[imageIndex].Text = $"{fileInfo.Name}\n{FormatFileSize(fileInfo.Length)}";

                        // Update button text
                        uploadImageButtons[imageIndex].Text = $"✅ Imagen {imageIndex + 1}";
                        uploadImageButtons[imageIndex].BackColor = Color.FromArgb(40, 167, 69);

                        Console.WriteLine($"✅ Imagen {imageIndex + 1} cargada exitosamente");
                    }
                    catch (OutOfMemoryException memEx)
                    {
                        Console.WriteLine($"❌ Error de memoria al cargar imagen {imageIndex + 1}: {memEx.Message}");
                        MessageBox.Show($"Error de memoria al cargar la imagen {imageIndex + 1}. La imagen puede ser demasiado grande o estar corrupta.", "Error de Memoria", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (ExternalException extEx)
                    {
                        Console.WriteLine($"❌ Error de GDI+ al cargar imagen {imageIndex + 1}: {extEx.Message}");
                        MessageBox.Show($"Error al procesar la imagen {imageIndex + 1}. El formato puede no ser compatible o el archivo puede estar corrupto.", "Error de Imagen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Error general al cargar imagen {imageIndex + 1}: {ex.Message}");
                        MessageBox.Show($"Error al cargar la imagen {imageIndex + 1}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Carga una imagen de forma segura desde un archivo
        /// </summary>
        private Image LoadImageSafely(string filePath)
        {
            try
            {
                // Leer el archivo completo en memoria primero
                byte[] imageBytes = File.ReadAllBytes(filePath);
                Console.WriteLine($"    - Archivo leído: {imageBytes.Length:N0} bytes");

                // Crear imagen desde los bytes en memoria
                using (var ms = new MemoryStream(imageBytes))
                {
                    var image = Image.FromStream(ms);

                    // Crear una copia en memoria para evitar problemas de acceso al archivo
                    var bitmap = new Bitmap(image);
                    image.Dispose(); // Liberar la imagen original

                    return bitmap;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    ❌ Error en LoadImageSafely: {ex.Message}");
                throw;
            }
        }

        private Image ResizeImage(Image originalImage, Size targetSize)
        {
            if (originalImage == null) return null;

            // Calculate the best fit size maintaining aspect ratio
            float ratioX = (float)targetSize.Width / originalImage.Width;
            float ratioY = (float)targetSize.Height / originalImage.Height;
            float ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(originalImage.Width * ratio);
            int newHeight = (int)(originalImage.Height * ratio);

            var resizedImage = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }

            return resizedImage;
        }

        private Image ResizeImageToExactSize(Image originalImage, Size targetSize, bool maintainAspectRatio = true)
        {
            if (originalImage == null) return null;

            var resizedImage = new Bitmap(targetSize.Width, targetSize.Height);
            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                if (maintainAspectRatio)
                {
                    // Calculate the best fit size maintaining aspect ratio
                    float ratioX = (float)targetSize.Width / originalImage.Width;
                    float ratioY = (float)targetSize.Height / originalImage.Height;
                    float ratio = Math.Min(ratioX, ratioY);

                    int newWidth = (int)(originalImage.Width * ratio);
                    int newHeight = (int)(originalImage.Height * ratio);

                    int x = (targetSize.Width - newWidth) / 2;
                    int y = (targetSize.Height - newHeight) / 2;

                    graphics.Clear(Color.White);
                    graphics.DrawImage(originalImage, x, y, newWidth, newHeight);
                }
                else
                {
                    graphics.DrawImage(originalImage, 0, 0, targetSize.Width, targetSize.Height);
                }
            }

            return resizedImage;
        }

        private string SaveOptimizedImages(Image originalImage, string vehicleId)
        {
            if (originalImage == null) return null;

            try
            {
                // Usar SimpleImageService en lugar de ImageSharp
                var imageService = new SimpleImageService();
                var imageVersions = imageService.CreateImageVersionsAsync(originalImage);

                // Crear directorio de imágenes si no existe
                string imagesDir = Path.Combine(Application.StartupPath, "Images", "Vehicles");
                if (!Directory.Exists(imagesDir))
                {
                    Directory.CreateDirectory(imagesDir);
                }

                string baseFileName = $"vehicle_{vehicleId}_{DateTime.Now:yyyyMMdd_HHmmss}";
                string mainImagePath = null;

                // Guardar imagen medium como principal
                if (imageVersions.ContainsKey("medium"))
                {
                    string fileName = $"{baseFileName}_medium.jpg";
                    string filePath = Path.Combine(imagesDir, fileName);

                    File.WriteAllBytes(filePath, imageVersions["medium"]);
                    mainImagePath = filePath;
                }

                return mainImagePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar las imágenes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType == mimeType)
                {
                    return codec;
                }
            }
            return null;
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        // Modify SaveButton_Click to handle multiple images:
        private async void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrEmpty(brandTextBox.Texts) ||
                    string.IsNullOrEmpty(modelTextBox.Texts) ||
                    string.IsNullOrEmpty(yearTextBox.Texts) ||
                    string.IsNullOrEmpty(priceTextBox.Texts) ||
                    string.IsNullOrEmpty(kilometersTextBox.Texts))
                {
                    MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Parse numeric values
                if (!int.TryParse(yearTextBox.Texts, out int year) ||
                    !decimal.TryParse(priceTextBox.Texts, out decimal price) ||
                    !int.TryParse(kilometersTextBox.Texts, out int kilometers))
                {
                    MessageBox.Show("Por favor, ingrese valores numéricos válidos para Año, Precio y Kilómetros.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Collect valid images
                var validImages = new List<Image>();
                for (int i = 0; i < 5; i++)
                {
                    if (_originalImages[i] != null)
                    {
                        validImages.Add(_originalImages[i]);
                    }
                }

                // Create or update vehicle
                if (_isEditMode)
                {
                    // Update existing vehicle
                    _vehicle.Brand = brandTextBox.Texts;
                    _vehicle.Model = modelTextBox.Texts;
                    _vehicle.Year = year;
                    _vehicle.Price = price;
                    _vehicle.Category = categoryComboBox.SelectedItem.ToString();
                    _vehicle.FuelType = (FuelType)fuelTypeComboBox.SelectedIndex;
                    _vehicle.TransmissionType = (TransmissionType)transmissionComboBox.SelectedIndex;
                    _vehicle.Kilometers = kilometers;
                    _vehicle.Status = (VehicleStatus)statusComboBox.SelectedIndex;
                    _vehicle.UpdatedAt = DateTime.Now;
                    

                    // Ejecutar ambas acciones de forma síncrona
                    var updateVehicleTask = apiClient.UpdateVehicleAsync(_vehicle);
                    var updateImagesTask = validImages.Count > 0 ?
                        _firestoreImageService.UpdateVehicleImagesInFirestoreAsync(validImages, _vehicle.Id.ToString()) :
                        Task.CompletedTask;

                    // Esperar a que ambas tareas terminen
                    await Task.WhenAll(updateVehicleTask, updateImagesTask);
                }
                else
                {
                    // Create new vehicle
                    Vehicle newVehicle = new Vehicle
                    {
                        Brand = brandTextBox.Texts,
                        Model = modelTextBox.Texts,
                        Year = year,
                        Price = price,
                        Category = categoryComboBox.SelectedItem.ToString(),
                        FuelType = (FuelType)fuelTypeComboBox.SelectedIndex,
                        TransmissionType = (TransmissionType)transmissionComboBox.SelectedIndex,
                        Kilometers = kilometers,
                        Status = (VehicleStatus) statusComboBox.SelectedIndex,
                        CreatedAt = DateTime.Now
                    };

                    // Save the vehicle first to get an ID
                    var savedVehicle = await apiClient.AddVehicleAsync(newVehicle);

                    // Save multiple images if any were selected
                    if (validImages.Count > 0)
                    {
                        await _firestoreImageService.ProcessAndStoreMultipleVehicleImagesAsync(validImages, savedVehicle.Id.ToString());
                    }
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el vehículo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // Update OnFormClosed to dispose all images:
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Dispose of all images to free memory
            for (int i = 0; i < 5; i++)
            {
                if (_originalImages[i] != null)
                {
                    _originalImages[i].Dispose();
                }
                if (imagePreviews[i].Image != null)
                {
                    imagePreviews[i].Image.Dispose();
                }
            }
            base.OnFormClosed(e);
        }
    }
}
