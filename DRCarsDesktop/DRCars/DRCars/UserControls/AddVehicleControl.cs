using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DRCars.Controls;

namespace DRCars
{
    public class AddVehicleControl : UserControl
    {
        private BufferedPanel formPanel;
        private RoundedButton submitButton;
        private Timer animationTimer;
        private int animationProgress = 0;

        public AddVehicleControl()
        {
            InitializeComponent();

            // Configurar el control
            this.BackColor = Form1.BaseColor;
            this.Dock = DockStyle.Fill;

            // Habilitar DoubleBuffered para evitar parpadeos
            this.DoubleBuffered = true;

            // Iniciar animación de carga
            animationTimer = new Timer { Interval = 20 };
            animationTimer.Tick += (s, e) => {
                animationProgress += 3;
                if (animationProgress >= 100)
                {
                    animationTimer.Stop();
                }
                this.Invalidate();
            };
            animationTimer.Start();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Panel para el formulario con efecto de vidrio
            formPanel = new BufferedPanel
            {
                Size = new Size(700, 550),
                Location = new Point(250, 100),
                BackColor = Color.FromArgb(40, 50, 55),
                AutoScroll = true // Habilitar scroll
            };

            // Hacer que el panel tenga esquinas redondeadas y efecto de vidrio
            formPanel.Paint += (s, e) => {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle bounds = new Rectangle(0, 0, formPanel.Width, formPanel.Height);

                using (GraphicsPath path = new GraphicsPath())
                {
                    // Reemplazado GraphicsExtensions.AddRoundedRectangle con implementación directa
                    AddRoundedRectangle(path, bounds, 15);

                    // Fondo semi-transparente
                    using (SolidBrush backBrush = new SolidBrush(Color.FromArgb(200, 35, 42, 45)))
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
                    Rectangle topRect = new Rectangle(0, 0, formPanel.Width, 30);
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

            // Título del formulario
            Label titleLabel = new Label
            {
                Text = "Añadir Nuevo Vehículo",
                Size = new Size(700, 50),
                Location = new Point(0, 20),
                Font = new Font("Segoe UI Semibold", 18),
                ForeColor = Form1.TextColor,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Crear campos del formulario
            CreateFormField(formPanel, "Marca:", 80, new string[] { "Mercedes-Benz", "BMW", "Audi", "Porsche", "Bentley", "Maserati" });
            CreateTextField(formPanel, "Modelo:", 140);
            CreateTextField(formPanel, "Año:", 200);
            CreateTextField(formPanel, "Precio:", 260);
            CreateFormField(formPanel, "Estado:", 320, new string[] { "Disponible", "Reservado", "En Mantenimiento" });

            // Panel para subir imágenes
            BufferedPanel imageUploadPanel = new BufferedPanel
            {
                Size = new Size(560, 80),
                Location = new Point(70, 380),
                BackColor = Color.FromArgb(50, 60, 65)
            };

            Label imageLabel = new Label
            {
                Text = "Imágenes del Vehículo",
                Size = new Size(200, 30),
                Location = new Point(10, 10),
                Font = new Font("Segoe UI", 10),
                ForeColor = Form1.TextColor
            };

            RoundedButton uploadButton = new RoundedButton
            {
                Text = "Seleccionar Archivos",
                Size = new Size(150, 30),
                Location = new Point(10, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(60, 70, 75),
                ForeColor = Form1.TextColor,
                Font = new Font("Segoe UI", 9),
                BorderRadius = 15
            };
            uploadButton.FlatAppearance.BorderSize = 0;

            imageUploadPanel.Controls.Add(uploadButton);
            imageUploadPanel.Controls.Add(imageLabel);

            // Botón para enviar el formulario
            submitButton = new RoundedButton
            {
                Text = "Guardar Vehículo",
                Size = new Size(200, 40),
                Location = new Point(250, 480),
                FlatStyle = FlatStyle.Flat,
                BackColor = Form1.AccentColor,
                ForeColor = Form1.TextColor,
                Font = new Font("Segoe UI", 12),
                Cursor = Cursors.Hand,
                BorderRadius = 20
            };
            submitButton.FlatAppearance.BorderSize = 0;

            // Agregar controles al panel del formulario
            formPanel.Controls.Add(titleLabel);
            formPanel.Controls.Add(imageUploadPanel);
            formPanel.Controls.Add(submitButton);

            // Agregar controles al UserControl
            this.Controls.Add(formPanel);

            this.Name = "AddVehicleControl";
            this.Size = new Size(1030, 750);
            this.Paint += AddVehicleControl_Paint;

            this.ResumeLayout(false);
        }

        private void AddVehicleControl_Paint(object sender, PaintEventArgs e)
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
                    g.DrawString("Añadir Nuevo Vehículo", titleFont, brush, new PointF(30, 20));
                }
            }

            // Dibujar línea decorativa debajo del título
            if (animationProgress >= 30)
            {
                int lineWidth = (int)(Math.Min(1.0, (animationProgress - 30) / 30.0) * 100);
                using (Pen pen = new Pen(Form1.AccentColor, 3))
                {
                    g.DrawLine(pen, new Point(30, 70), new Point(30 + lineWidth, 70));
                }
            }
        }

        private void CreateTextField(Panel container, string labelText, int yPosition)
        {
            // Etiqueta para el campo
            Label fieldLabel = new Label
            {
                Text = labelText,
                Size = new Size(120, 30),
                Location = new Point(30, yPosition),
                Font = new Font("Segoe UI", 12),
                ForeColor = Form1.TextColor,
                TextAlign = ContentAlignment.MiddleRight
            };

            // Campo de texto redondeado
            RoundedTextBox textBox = new RoundedTextBox
            {
                Size = new Size(300, 36),
                Location = new Point(160, yPosition - 3),
                BorderRadius = 18,
                BorderSize = 1,
                BorderColor = Color.FromArgb(100, Form1.SecondaryColor),
                BackColor = Color.FromArgb(60, 70, 75),
                ForeColor = Form1.TextColor,
                Font = new Font("Segoe UI", 12)
            };

            container.Controls.Add(textBox);
            container.Controls.Add(fieldLabel);
        }

        private void CreateFormField(Panel container, string labelText, int yPosition, string[] options)
        {
            // Etiqueta para el campo
            Label fieldLabel = new Label
            {
                Text = labelText,
                Size = new Size(120, 30),
                Location = new Point(30, yPosition),
                Font = new Font("Segoe UI", 12),
                ForeColor = Form1.TextColor,
                TextAlign = ContentAlignment.MiddleRight
            };

            // ComboBox redondeado para las opciones
            RoundedComboBox comboBox = new RoundedComboBox
            {
                Size = new Size(300, 36),
                Location = new Point(160, yPosition - 3),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.FromArgb(60, 70, 75),
                ForeColor = Form1.TextColor,
                BorderRadius = 18,
                BorderSize = 1
            };

            comboBox.Items.AddRange(options);
            if (options.Length > 0)
            {
                comboBox.SelectedIndex = 0;
            }

            container.Controls.Add(comboBox);
            container.Controls.Add(fieldLabel);
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
