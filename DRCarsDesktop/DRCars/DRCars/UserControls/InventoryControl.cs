using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DRCars.Controls;

namespace DRCars
{
    public class InventoryControl : UserControl
    {
        private BufferedPanel searchPanel;
        private RoundedTextBox searchBox;
        private RoundedButton searchButton;
        private BufferedPanel filtersPanel;
        private BufferedPanel cardsContainer;
        private Timer animationTimer;
        private int animationProgress = 0;

        // Datos de ejemplo para vehículos
        private string[] carModels = {
            "Mercedes-Benz S-Class", "BMW 7 Series", "Audi A8",
            "Porsche Panamera", "Bentley Continental GT", "Maserati Quattroporte",
            "Jaguar XJ", "Lexus LS", "Tesla Model S", "Rolls-Royce Ghost",
            "Ferrari Roma", "Lamborghini Huracán", "Aston Martin DB11"
        };

        private string[] carStatuses = {
            "Disponible", "Reservado", "En Mantenimiento", "Disponible",
            "Disponible", "Vendido", "Disponible", "Reservado", "Disponible",
            "Vendido", "Disponible", "Reservado", "En Mantenimiento"
        };

        private string[] carPrices = {
            "€120,000", "€95,000", "€110,000", "€130,000", "€220,000",
            "€140,000", "€90,000", "€105,000", "€95,000", "€280,000",
            "€210,000", "€240,000", "€180,000"
        };

        public InventoryControl()
        {
            // Primero inicializamos los componentes
            InitializeComponent();

            // Configurar el control
            this.BackColor = Form1.BaseColor;
            this.Dock = DockStyle.Fill;

            // Habilitar DoubleBuffered para evitar parpadeos
            this.DoubleBuffered = true;

            // Iniciar animación de carga DESPUÉS de inicializar los componentes
            animationTimer = new Timer { Interval = 20 };
            animationTimer.Tick += (s, e) => {
                animationProgress += 3;
                if (animationProgress >= 100)
                {
                    animationTimer.Stop();
                }
                this.Invalidate();

                // Verificar que cardsContainer no sea null antes de usarlo
                if (animationProgress > 50 && cardsContainer != null && cardsContainer.Controls.Count < carModels.Length)
                {
                    int index = cardsContainer.Controls.Count;
                    if (index < carModels.Length)
                    {
                        AddCarCard(index);
                    }
                }
            };

            // Iniciar el timer después de que todo esté configurado
            animationTimer.Start();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Contenedor para las tarjetas de vehículos con scroll - Inicializado primero
            cardsContainer = new BufferedPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Form1.BaseColor,
                Padding = new Padding(20)
            };

            // Panel de búsqueda en la parte superior
            searchPanel = new BufferedPanel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(35, 42, 45),
                Padding = new Padding(20)
            };

            // Caja de búsqueda redondeada
            searchBox = new RoundedTextBox
            {
                Size = new Size(400, 40),
                Location = new Point(20, 20),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.FromArgb(60, 70, 75),
                ForeColor = Form1.TextColor,
                BorderRadius = 20,
                PlaceholderText = "Buscar vehículo...",
                Padding = new Padding(10, 8, 10, 8) // Ajustar el padding para centrar verticalmente el texto
            };

            // Botón de búsqueda redondeado
            searchButton = new RoundedButton
            {
                Size = new Size(120, 40),
                Location = new Point(430, 20),
                Text = "Buscar",
                FlatStyle = FlatStyle.Flat,
                BackColor = Form1.AccentColor,
                ForeColor = Form1.TextColor,
                Font = new Font("Segoe UI", 11),
                BorderRadius = 20
            };
            searchButton.FlatAppearance.BorderSize = 0;

            // Panel de filtros a la izquierda
            filtersPanel = new BufferedPanel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(35, 42, 45),
                Padding = new Padding(15)
            };

            // Título del panel de filtros
            Label filtersTitle = new Label
            {
                Text = "Filtros",
                Dock = DockStyle.Top,
                Height = 40,
                Font = new Font("Segoe UI Semibold", 14),
                ForeColor = Form1.TextColor,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0)
            };

            // Agregar el título al panel de filtros
            filtersPanel.Controls.Add(filtersTitle);

            // Agregar filtros de ejemplo
            AddFilterGroup(filtersPanel, "Marca", new string[] { "Todas", "Mercedes-Benz", "BMW", "Audi", "Porsche", "Bentley" });
            AddFilterGroup(filtersPanel, "Estado", new string[] { "Todos", "Disponible", "Reservado", "Vendido", "En Mantenimiento" });
            AddFilterGroup(filtersPanel, "Precio", new string[] { "Todos", "< €100,000", "€100,000 - €150,000", "> €150,000" });

            // Agregar controles a los paneles
            searchPanel.Controls.Add(searchButton);
            searchPanel.Controls.Add(searchBox);

            // Agregar paneles al control - Asegurarse de que cardsContainer se agrega primero
            this.Controls.Add(cardsContainer);
            this.Controls.Add(filtersPanel);
            this.Controls.Add(searchPanel);

            this.Name = "InventoryControl";
            this.Size = new Size(1030, 750);
            this.Paint += InventoryControl_Paint;

            this.ResumeLayout(false);
        }

        private void InventoryControl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Dibujar título con animación de aparición
            if (animationProgress >= 10)
            {
                float titleOpacity = Math.Min(1.0f, (animationProgress - 10) / 40.0f);
                using (Font titleFont = new Font("Segoe UI", 24, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(255 * titleOpacity), Form1.TextColor)))
                {
                    g.DrawString("Inventario de Vehículos", titleFont, brush, new PointF(280, 20));
                }
            }
        }

        private void AddFilterGroup(Panel container, string title, string[] options)
        {
            // Crear panel para el grupo de filtros
            BufferedPanel groupPanel = new BufferedPanel
            {
                Dock = DockStyle.Top,
                Height = 30 + options.Length * 30,
                Padding = new Padding(0, 10, 0, 10)
            };

            // Título del grupo
            Label groupTitle = new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                Height = 25,
                Font = new Font("Segoe UI Semibold", 12),
                ForeColor = Form1.SecondaryColor
            };

            groupPanel.Controls.Add(groupTitle);

            // Agregar opciones como RadioButtons
            for (int i = 0; i < options.Length; i++)
            {
                RadioButton option = new RadioButton
                {
                    Text = options[i],
                    Dock = DockStyle.Top,
                    Height = 25,
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Form1.TextColor,
                    Checked = i == 0
                };

                groupPanel.Controls.Add(option);
            }

            container.Controls.Add(groupPanel);
        }

        private void AddCarCard(int index)
        {
            // Verificar que cardsContainer no sea null antes de usarlo
            if (cardsContainer == null) return;

            // Crear panel para la tarjeta de vehículo
            TransparentPanel cardPanel = new TransparentPanel
            {
                Size = new Size(220, 300),
                Location = new Point(20 + (index % 3) * 240, 80 + (index / 3) * 320),
                BackColor = Color.FromArgb(50, 60, 65)
            };

            // Hacer que la tarjeta tenga esquinas redondeadas y efecto de vidrio
            cardPanel.Paint += (s, e) => {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle bounds = new Rectangle(0, 0, cardPanel.Width, cardPanel.Height);

                using (GraphicsPath path = new GraphicsPath())
                {
                    // Reemplazado GraphicsExtensions.AddRoundedRectangle con implementación directa
                    AddRoundedRectangle(path, bounds, 15);

                    // Fondo semi-transparente
                    using (SolidBrush backBrush = new SolidBrush(Color.FromArgb(180, 35, 42, 45)))
                    {
                        g.FillPath(backBrush, path);
                    }

                    // Borde con gradiente
                    using (LinearGradientBrush borderBrush = new LinearGradientBrush(
                        bounds,
                        Color.FromArgb(100, Form1.AccentColor),
                        Color.FromArgb(50, Form1.SecondaryColor),
                        LinearGradientMode.ForwardDiagonal))
                    using (Pen borderPen = new Pen(borderBrush, 1))
                    {
                        g.DrawPath(borderPen, path);
                    }

                    // Efecto de brillo en la parte superior
                    Rectangle topRect = new Rectangle(0, 0, cardPanel.Width, 20);
                    using (LinearGradientBrush highlightBrush = new LinearGradientBrush(
                        topRect,
                        Color.FromArgb(60, 255, 255, 255),
                        Color.FromArgb(10, 255, 255, 255),
                        LinearGradientMode.Vertical))
                    {
                        g.FillRectangle(highlightBrush, topRect);
                    }
                }
            };

            // Imagen del vehículo (placeholder)
            Panel imagePanel = new Panel
            {
                Size = new Size(200, 130),
                Location = new Point(10, 10),
                BackColor = Color.FromArgb(30, 35, 40)
            };

            // Etiqueta para el modelo del vehículo
            Label modelLabel = new Label
            {
                Text = carModels[index],
                Size = new Size(200, 40),
                Location = new Point(10, 150),
                Font = new Font("Segoe UI Semibold", 12),
                ForeColor = Form1.TextColor,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Etiqueta para el precio
            Label priceLabel = new Label
            {
                Text = carPrices[index],
                Size = new Size(200, 30),
                Location = new Point(10, 190),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Form1.AccentColor,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Etiqueta para el estado
            Label statusLabel = new Label
            {
                Text = carStatuses[index],
                Size = new Size(200, 25),
                Location = new Point(10, 220),
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Configurar color según el estado
            switch (carStatuses[index])
            {
                case "Disponible":
                    statusLabel.ForeColor = Color.FromArgb(100, 200, 100);
                    break;
                case "Reservado":
                    statusLabel.ForeColor = Color.FromArgb(255, 180, 0);
                    break;
                case "Vendido":
                    statusLabel.ForeColor = Color.FromArgb(200, 100, 100);
                    break;
                case "En Mantenimiento":
                    statusLabel.ForeColor = Color.FromArgb(100, 150, 200);
                    break;
            }

            // Botón para ver detalles
            RoundedButton detailsButton = new RoundedButton
            {
                Text = "Ver Detalles",
                Size = new Size(200, 35),
                Location = new Point(10, 255),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(60, 70, 75),
                ForeColor = Form1.TextColor,
                Font = new Font("Segoe UI", 9),
                BorderRadius = 17
            };
            detailsButton.FlatAppearance.BorderSize = 0;

            // Agregar controles a la tarjeta
            cardPanel.Controls.Add(detailsButton);
            cardPanel.Controls.Add(statusLabel);
            cardPanel.Controls.Add(priceLabel);
            cardPanel.Controls.Add(modelLabel);
            cardPanel.Controls.Add(imagePanel);

            // Animar la aparición de la tarjeta
            cardPanel.Visible = true;
            cardPanel.Opacity = 0;

            Timer fadeInTimer = new Timer { Interval = 20 };
            double opacity = 0;

            fadeInTimer.Tick += (s, e) => {
                opacity += 10;
                if (opacity >= 100)
                {
                    fadeInTimer.Stop();
                    cardPanel.Opacity = 100;
                }
                else
                {
                    cardPanel.Opacity = opacity;
                }
            };

            cardsContainer.Controls.Add(cardPanel);
            fadeInTimer.Start();
        }

        // Método para crear rectángulos redondeados (reemplaza GraphicsExtensions.AddRoundedRectangle)
        private void AddRoundedRectangle(GraphicsPath path, Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Rectangle arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

            // Esquina superior izquierda
            path.AddArc(arc, 180, 90);

            // Esquina superior derecha
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Esquina inferior derecha
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Esquina inferior izquierda
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
        }
    }
}
