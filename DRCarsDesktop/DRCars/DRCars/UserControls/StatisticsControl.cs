using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DRCars.Controls;

namespace DRCars
{
    public class StatisticsControl : UserControl
    {
        private Timer animationTimer;
        private int animationProgress = 0;

        // Datos de ejemplo para estadísticas
        private string[] categories = { "Sedán", "SUV", "Coupé", "Convertible", "Deportivo" };
        private int[] inventoryData = { 35, 25, 15, 10, 15 };
        private int[] salesData = { 12, 8, 6, 3, 5 };

        public StatisticsControl()
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
                animationProgress += 2;
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

            // Configuración básica del UserControl
            this.Name = "StatisticsControl";
            this.Size = new Size(900, 600);
            this.Paint += StatisticsControl_Paint;

            this.ResumeLayout(false);
        }

        private void StatisticsControl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Dibujar título
            if (animationProgress >= 10)
            {
                float titleOpacity = Math.Min(1.0f, (animationProgress - 10) / 40.0f);
                using (Font titleFont = new Font("Segoe UI", 24, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(255 * titleOpacity), Form1.TextColor)))
                {
                    g.DrawString("Estadísticas", titleFont, brush, new PointF(30, 20));
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

            // Dibujar paneles de estadísticas
            if (animationProgress >= 40)
            {
                float opacity = Math.Min(1.0f, (animationProgress - 40) / 60.0f);

                // Panel para gráfico de pastel
                DrawGlassPanel(g, new Rectangle(30, 100, 400, 400), opacity);

                // Panel para gráfico de barras
                DrawGlassPanel(g, new Rectangle(450, 100, 400, 400), opacity);

                // Dibujar títulos de los paneles
                using (Font font = new Font("Segoe UI", 14, FontStyle.Regular))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(255 * opacity), Form1.TextColor)))
                {
                    g.DrawString("Distribución de Inventario", font, brush, new PointF(50, 120));
                    g.DrawString("Ventas por Categoría", font, brush, new PointF(470, 120));
                }

                // Dibujar gráficos si la animación ha progresado lo suficiente
                if (animationProgress >= 60)
                {
                    float chartOpacity = Math.Min(1.0f, (animationProgress - 60) / 40.0f);

                    // Dibujar gráfico de pastel
                    DrawPieChart(g, new Rectangle(80, 160, 300, 300), chartOpacity);

                    // Dibujar gráfico de barras
                    DrawBarChart(g, new Rectangle(470, 160, 360, 300), chartOpacity);
                }
            }
        }

        private void DrawGlassPanel(Graphics g, Rectangle bounds, float opacity)
        {
            // Crear efecto de vidrio esmerilado
            using (GraphicsPath path = new GraphicsPath())
            {
                AddRoundedRectangle(path, bounds, 15);

                // Fondo semi-transparente
                using (SolidBrush backBrush = new SolidBrush(Color.FromArgb((int)(150 * opacity), 50, 60, 65)))
                {
                    g.FillPath(backBrush, path);
                }

                // Borde con gradiente
                using (LinearGradientBrush borderBrush = new LinearGradientBrush(
                    bounds,
                    Color.FromArgb((int)(80 * opacity), Form1.AccentColor),
                    Color.FromArgb((int)(40 * opacity), Form1.SecondaryColor),
                    LinearGradientMode.ForwardDiagonal))
                using (Pen borderPen = new Pen(borderBrush, 1))
                {
                    g.DrawPath(borderPen, path);
                }

                // Efecto de brillo en la parte superior
                Rectangle topRect = new Rectangle(bounds.X, bounds.Y, bounds.Width, 20);
                using (LinearGradientBrush highlightBrush = new LinearGradientBrush(
                    topRect,
                    Color.FromArgb((int)(60 * opacity), 255, 255, 255),
                    Color.FromArgb((int)(10 * opacity), 255, 255, 255),
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(highlightBrush, topRect);
                }
            }
        }

        private void DrawPieChart(Graphics g, Rectangle bounds, float opacity)
        {
            // Calcular el total para los porcentajes
            int total = 0;
            foreach (int value in inventoryData)
            {
                total += value;
            }

            // Colores para las secciones del gráfico
            Color[] sectionColors = {
                Color.FromArgb((int)(255 * opacity), 8, 146, 208),  // Azul (AccentColor)
                Color.FromArgb((int)(255 * opacity), 76, 175, 80),  // Verde
                Color.FromArgb((int)(255 * opacity), 255, 152, 0),  // Naranja
                Color.FromArgb((int)(255 * opacity), 156, 39, 176), // Púrpura
                Color.FromArgb((int)(255 * opacity), 244, 67, 54)   // Rojo
            };

            // Punto central y radio
            Point center = new Point(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
            int radius = Math.Min(bounds.Width, bounds.Height) / 2 - 20;

            // Ángulo inicial
            float startAngle = 0;

            // Dibujar cada sección del gráfico
            for (int i = 0; i < inventoryData.Length; i++)
            {
                // Calcular el ángulo de la sección
                float sweepAngle = (float)inventoryData[i] / total * 360;

                // Dibujar la sección
                using (SolidBrush brush = new SolidBrush(sectionColors[i % sectionColors.Length]))
                {
                    g.FillPie(brush, center.X - radius, center.Y - radius, radius * 2, radius * 2, startAngle, sweepAngle);
                }

                // Calcular posición para la etiqueta
                double radians = Math.PI * (startAngle + sweepAngle / 2) / 180.0;
                int labelRadius = radius + 20;
                Point labelPoint = new Point(
                    (int)(center.X + labelRadius * Math.Cos(radians)),
                    (int)(center.Y + labelRadius * Math.Sin(radians))
                );

                // Dibujar etiqueta con porcentaje
                string percentage = Math.Round((double)inventoryData[i] / total * 100) + "%";
                using (Font font = new Font("Segoe UI", 9))
                using (SolidBrush textBrush = new SolidBrush(Color.FromArgb((int)(255 * opacity), Form1.TextColor)))
                {
                    SizeF textSize = g.MeasureString(percentage, font);
                    g.DrawString(percentage, font, textBrush,
                                new PointF(labelPoint.X - textSize.Width / 2, labelPoint.Y - textSize.Height / 2));
                }

                // Actualizar ángulo inicial para la siguiente sección
                startAngle += sweepAngle;
            }

            // Dibujar leyenda
            // Dibujar leyenda a la derecha del gráfico, no debajo
            int legendY = bounds.Y + 50;
            int legendX = bounds.X + bounds.Width + 20;
            for (int i = 0; i < categories.Length; i++)
            {
                // Cuadrado de color
                // Cuadrado de color
                Rectangle colorRect = new Rectangle(legendX, legendY + i * 25, 15, 15);
                using (SolidBrush brush = new SolidBrush(sectionColors[i % sectionColors.Length]))
                {
                    g.FillRectangle(brush, colorRect);
                }

                // Texto de la categoría
                using (Font font = new Font("Segoe UI", 9))
                using (SolidBrush textBrush = new SolidBrush(Color.FromArgb((int)(255 * opacity), Form1.TextColor)))
                {
                    // Reemplazarlo por:
                    g.DrawString(categories[i], font, textBrush, new PointF(legendX + 25, legendY + i * 25));
                }
            }
        }

        private void DrawBarChart(Graphics g, Rectangle bounds, float opacity)
        {
            // Calcular dimensiones
            int barWidth = (bounds.Width - 60) / salesData.Length;
            int maxValue = 0;
            foreach (int value in salesData)
            {
                maxValue = Math.Max(maxValue, value);
            }

            // Dibujar ejes
            using (Pen axisPen = new Pen(Color.FromArgb((int)(150 * opacity), Form1.SecondaryColor), 1))
            {
                // Eje Y
                g.DrawLine(axisPen, bounds.X + 40, bounds.Y + 30, bounds.X + 40, bounds.Y + bounds.Height - 40);

                // Eje X
                g.DrawLine(axisPen, bounds.X + 40, bounds.Y + bounds.Height - 40, bounds.X + bounds.Width - 20, bounds.Y + bounds.Height - 40);
            }

            // Dibujar barras
            for (int i = 0; i < salesData.Length; i++)
            {
                // Calcular altura de la barra
                int barHeight = (int)((salesData[i] / (float)maxValue) * (bounds.Height - 80));

                // Calcular posición X
                int x = bounds.X + 50 + i * barWidth;

                // Crear rectángulo para la barra
                Rectangle barRect = new Rectangle(
                    x,
                    bounds.Y + bounds.Height - 40 - barHeight,
                    barWidth - 10,
                    barHeight
                );

                // Dibujar barra con gradiente
                using (LinearGradientBrush barBrush = new LinearGradientBrush(
                    barRect,
                    Color.FromArgb((int)(255 * opacity), Form1.AccentColor),
                    Color.FromArgb((int)(150 * opacity),
                                  Form1.AccentColor.R / 2,
                                  Form1.AccentColor.G / 2,
                                  Form1.AccentColor.B / 2),
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(barBrush, barRect);
                }

                // Dibujar valor encima de la barra
                using (Font valueFont = new Font("Segoe UI", 9))
                using (SolidBrush textBrush = new SolidBrush(Color.FromArgb((int)(200 * opacity), Form1.TextColor)))
                {
                    string value = salesData[i].ToString();
                    SizeF textSize = g.MeasureString(value, valueFont);
                    g.DrawString(
                        value,
                        valueFont,
                        textBrush,
                        new PointF(x + (barWidth - 10) / 2 - textSize.Width / 2,
                                  bounds.Y + bounds.Height - 45 - barHeight - textSize.Height)
                    );
                }

                // Dibujar etiqueta debajo de la barra
                using (Font labelFont = new Font("Segoe UI", 8))
                using (SolidBrush textBrush = new SolidBrush(Color.FromArgb((int)(200 * opacity), Form1.SecondaryColor)))
                {
                    SizeF textSize = g.MeasureString(categories[i], labelFont);
                    g.DrawString(
                        categories[i],
                        labelFont,
                        textBrush,
                        new PointF(x + (barWidth - 10) / 2 - textSize.Width / 2,
                                  bounds.Y + bounds.Height - 35)
                    );
                }
            }

            // Dibujar marcas en el eje Y
            for (int i = 0; i <= 5; i++)
            {
                int y = bounds.Y + bounds.Height - 40 - i * (bounds.Height - 80) / 5;
                int value = i * maxValue / 5;

                using (Pen tickPen = new Pen(Color.FromArgb((int)(100 * opacity), Form1.SecondaryColor), 1))
                {
                    g.DrawLine(tickPen, bounds.X + 35, y, bounds.X + 45, y);
                }

                using (Font font = new Font("Segoe UI", 8))
                using (SolidBrush textBrush = new SolidBrush(Color.FromArgb((int)(200 * opacity), Form1.SecondaryColor)))
                {
                    g.DrawString(
                        value.ToString(),
                        font,
                        textBrush,
                        new PointF(bounds.X + 30 - g.MeasureString(value.ToString(), font).Width, y - 6)
                    );
                }
            }
        }

        // Método para crear rectángulos redondeados
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
