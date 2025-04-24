using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DRCars.Controls;

namespace DRCars
{
    public class Dashboard : UserControl
    {
        private Timer animationTimer;
        private int animationProgress = 0;

        public Dashboard()
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
                this.Invalidate(); // Redibujar el control
            };
            animationTimer.Start();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Configuración básica del UserControl
            this.Name = "Dashboard";
            this.Size = new Size(1030, 750);
            this.Paint += Dashboard_Paint;

            this.ResumeLayout(false);
        }

        private void Dashboard_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Dibujar título
            using (Font titleFont = new Font("Segoe UI", 24, FontStyle.Bold))
            {
                float titleOpacity = Math.Min(1.0f, animationProgress / 50.0f);
                using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(255 * titleOpacity), Form1.TextColor)))
                {
                    g.DrawString("Dashboard", titleFont, brush, new PointF(30, 20));
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

            // Dibujar tarjetas de resumen si la animación ha progresado lo suficiente
            if (animationProgress >= 50)
            {
                float cardOpacity = Math.Min(1.0f, (animationProgress - 50) / 50.0f);
                DrawSummaryCard(g, "Vehículos en Stock", "124", new Rectangle(30, 100, 220, 140), cardOpacity);
                DrawSummaryCard(g, "Ventas del Mes", "28", new Rectangle(270, 100, 220, 140), cardOpacity);
                DrawSummaryCard(g, "Valor del Inventario", "€4.5M", new Rectangle(510, 100, 220, 140), cardOpacity);
                DrawSummaryCard(g, "Clientes Nuevos", "15", new Rectangle(750, 100, 220, 140), cardOpacity);
            }

            // Dibujar área para gráfico
            if (animationProgress >= 70)
            {
                float graphOpacity = Math.Min(1.0f, (animationProgress - 70) / 30.0f);
                DrawGlassPanel(g, new Rectangle(30, 260, 700, 400), graphOpacity);

                using (Font font = new Font("Segoe UI", 14, FontStyle.Regular))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(255 * graphOpacity), Form1.TextColor)))
                {
                    g.DrawString("Estadísticas de Inventario", font, brush, new PointF(50, 280));
                }

                // Dibujar gráfico de ejemplo
                if (animationProgress >= 80)
                {
                    DrawSampleGraph(g, new Rectangle(50, 320, 660, 320), Math.Min(1.0f, (animationProgress - 80) / 20.0f));
                }
            }

            // Dibujar panel de actividad reciente
            if (animationProgress >= 60)
            {
                float activityOpacity = Math.Min(1.0f, (animationProgress - 60) / 40.0f);
                DrawGlassPanel(g, new Rectangle(750, 260, 250, 400), activityOpacity);

                using (Font font = new Font("Segoe UI", 14, FontStyle.Regular))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(255 * activityOpacity), Form1.TextColor)))
                {
                    g.DrawString("Actividad Reciente", font, brush, new PointF(770, 280));
                }

                // Dibujar elementos de actividad
                if (animationProgress >= 75)
                {
                    DrawActivityItem(g, "Mercedes-Benz S-Class vendido", "Hace 2 horas", new Rectangle(770, 320, 210, 60), activityOpacity);
                    DrawActivityItem(g, "BMW 7 Series añadido", "Hace 5 horas", new Rectangle(770, 390, 210, 60), activityOpacity);
                    DrawActivityItem(g, "Audi A8 reservado", "Hace 1 día", new Rectangle(770, 460, 210, 60), activityOpacity);
                    DrawActivityItem(g, "Porsche Panamera en mantenimiento", "Hace 2 días", new Rectangle(770, 530, 210, 60), activityOpacity);
                }
            }
        }

        private void DrawSummaryCard(Graphics g, string title, string value, Rectangle bounds, float opacity)
        {
            // Dibujar fondo de tarjeta con efecto de vidrio
            DrawGlassPanel(g, bounds, opacity);

            // Dibujar título
            using (Font titleFont = new Font("Segoe UI", 12, FontStyle.Regular))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(255 * opacity), Form1.SecondaryColor)))
            {
                g.DrawString(title, titleFont, brush, new PointF(bounds.X + 15, bounds.Y + 15));
            }

            // Dibujar valor
            using (Font valueFont = new Font("Segoe UI", 24, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(255 * opacity), Form1.TextColor)))
            {
                g.DrawString(value, valueFont, brush, new PointF(bounds.X + 15, bounds.Y + 50));
            }
        }

        private void DrawActivityItem(Graphics g, string title, string time, Rectangle bounds, float opacity)
        {
            // Dibujar fondo con efecto de vidrio más sutil
            using (GraphicsPath path = new GraphicsPath())
            {
                AddRoundedRectangle(path, bounds, 8);

                // Fondo semi-transparente
                using (SolidBrush backBrush = new SolidBrush(Color.FromArgb((int)(50 * opacity), 60, 70, 75)))
                {
                    g.FillPath(backBrush, path);
                }

                // Borde sutil
                using (Pen borderPen = new Pen(Color.FromArgb((int)(30 * opacity), Form1.SecondaryColor), 1))
                {
                    g.DrawPath(borderPen, path);
                }
            }

            // Dibujar título
            using (Font titleFont = new Font("Segoe UI", 10, FontStyle.Regular))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(255 * opacity), Form1.TextColor)))
            {
                g.DrawString(title, titleFont, brush, new PointF(bounds.X + 10, bounds.Y + 10));
            }

            // Dibujar tiempo
            using (Font timeFont = new Font("Segoe UI", 8, FontStyle.Italic))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(200 * opacity), Form1.SecondaryColor)))
            {
                g.DrawString(time, timeFont, brush, new PointF(bounds.X + 10, bounds.Y + 35));
            }
        }

        private void DrawGlassPanel(Graphics g, Rectangle bounds, float opacity)
        {
            // Crear efecto de vidrio esmerilado
            using (GraphicsPath path = new GraphicsPath())
            {
                AddRoundedRectangle(path, bounds, 15);

                // Fondo semi-transparente
                using (SolidBrush backBrush = new SolidBrush(Color.FromArgb((int)(100 * opacity), 50, 60, 65)))
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

        private void DrawSampleGraph(Graphics g, Rectangle bounds, float opacity)
        {
            // Datos de ejemplo para el gráfico
            int[] data = { 15, 25, 40, 30, 45, 35, 50, 42, 38, 48, 55, 47 };
            string[] labels = { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };

            // Calcular dimensiones
            int barWidth = (bounds.Width - 60) / data.Length;
            int maxValue = 60; // Valor máximo para escalar

            // Dibujar ejes
            using (Pen axisPen = new Pen(Color.FromArgb((int)(150 * opacity), Form1.SecondaryColor), 1))
            {
                // Eje Y
                g.DrawLine(axisPen, bounds.X, bounds.Y, bounds.X, bounds.Y + bounds.Height);

                // Eje X
                g.DrawLine(axisPen, bounds.X, bounds.Y + bounds.Height, bounds.X + bounds.Width, bounds.Y + bounds.Height);
            }

            // Dibujar líneas de cuadrícula horizontales
            for (int i = 1; i <= 5; i++)
            {
                int y = bounds.Y + bounds.Height - (i * bounds.Height / 5);
                using (Pen gridPen = new Pen(Color.FromArgb((int)(50 * opacity), Form1.SecondaryColor), 1))
                {
                    gridPen.DashStyle = DashStyle.Dot;
                    g.DrawLine(gridPen, bounds.X, y, bounds.X + bounds.Width, y);
                }

                // Dibujar valores en el eje Y
                using (Font font = new Font("Segoe UI", 8))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(180 * opacity), Form1.SecondaryColor)))
                {
                    string value = (i * maxValue / 5).ToString();
                    g.DrawString(value, font, brush, new PointF(bounds.X - 25, y - 10));
                }
            }

            // Puntos para la línea de tendencia
            Point[] points = new Point[data.Length];

            // Dibujar barras y recopilar puntos para la línea
            for (int i = 0; i < data.Length; i++)
            {
                // Calcular altura de la barra
                int barHeight = (int)((data[i] / (float)maxValue) * bounds.Height);

                // Calcular posición X
                int x = bounds.X + (i * barWidth) + (barWidth / 2);

                // Punto para la línea de tendencia
                points[i] = new Point(x, bounds.Y + bounds.Height - barHeight);

                // Crear rectángulo para la barra
                Rectangle barRect = new Rectangle(
                    x - (barWidth / 2) + 5,
                    bounds.Y + bounds.Height - barHeight,
                    barWidth - 10,
                    barHeight
                );

                // Dibujar barra con gradiente
                using (LinearGradientBrush barBrush = new LinearGradientBrush(
                    barRect,
                    Color.FromArgb((int)(180 * opacity), Form1.AccentColor),
                    Color.FromArgb((int)(100 * opacity),
                                  Form1.AccentColor.R / 2,
                                  Form1.AccentColor.G / 2,
                                  Form1.AccentColor.B / 2),
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(barBrush, barRect);
                }

                // Dibujar etiqueta
                using (Font labelFont = new Font("Segoe UI", 8))
                using (SolidBrush textBrush = new SolidBrush(Color.FromArgb((int)(200 * opacity), Form1.SecondaryColor)))
                {
                    g.DrawString(
                        labels[i],
                        labelFont,
                        textBrush,
                        new PointF(x - 10, bounds.Y + bounds.Height + 5)
                    );
                }
            }

            // Dibujar línea de tendencia
            using (Pen linePen = new Pen(Color.FromArgb((int)(200 * opacity), Color.FromArgb(255, 152, 0)), 2))
            {
                linePen.StartCap = LineCap.Round;
                linePen.EndCap = LineCap.Round;
                g.DrawLines(linePen, points);

                // Dibujar puntos en la línea
                foreach (Point p in points)
                {
                    g.FillEllipse(new SolidBrush(Color.FromArgb((int)(255 * opacity), Color.FromArgb(255, 152, 0))),
                                 p.X - 4, p.Y - 4, 8, 8);
                    g.DrawEllipse(new Pen(Color.FromArgb((int)(255 * opacity), Color.White), 1),
                                 p.X - 4, p.Y - 4, 8, 8);
                }
            }

            // Dibujar leyenda
            // Dibujar leyenda en la parte superior derecha, fuera del área del gráfico
            Rectangle legendRect = new Rectangle(bounds.X + bounds.Width - 180, bounds.Y - 60, 180, 50);
            using (GraphicsPath path = new GraphicsPath())
            {
                AddRoundedRectangle(path, legendRect, 8);

                // Fondo semi-transparente
                using (SolidBrush backBrush = new SolidBrush(Color.FromArgb((int)(80 * opacity), 50, 60, 65)))
                {
                    g.FillPath(backBrush, path);
                }

                // Borde
                using (Pen borderPen = new Pen(Color.FromArgb((int)(50 * opacity), Form1.SecondaryColor), 1))
                {
                    g.DrawPath(borderPen, path);
                }
            }

            // Dibujar elementos de la leyenda
            // Barra
            Rectangle barSample = new Rectangle(legendRect.X + 15, legendRect.Y + 15, 20, 10);
            using (LinearGradientBrush barBrush = new LinearGradientBrush(
                barSample,
                Color.FromArgb((int)(180 * opacity), Form1.AccentColor),
                Color.FromArgb((int)(100 * opacity), Form1.AccentColor.R / 2, Form1.AccentColor.G / 2, Form1.AccentColor.B / 2),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(barBrush, barSample);
            }

            // Línea
            using (Pen linePen = new Pen(Color.FromArgb((int)(200 * opacity), Color.FromArgb(255, 152, 0)), 2))
            {
                g.DrawLine(linePen, legendRect.X + 15, legendRect.Y + 35, legendRect.X + 35, legendRect.Y + 35);
                g.FillEllipse(new SolidBrush(Color.FromArgb((int)(255 * opacity), Color.FromArgb(255, 152, 0))),
                             legendRect.X + 25 - 3, legendRect.Y + 35 - 3, 6, 6);
                g.DrawEllipse(new Pen(Color.FromArgb((int)(255 * opacity), Color.White), 1),
                             legendRect.X + 25 - 3, legendRect.Y + 35 - 3, 6, 6);
            }

            // Textos
            using (Font font = new Font("Segoe UI", 8))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(220 * opacity), Form1.TextColor)))
            {
                g.DrawString("Inventario", font, brush, new PointF(legendRect.X + 45, legendRect.Y + 12));
                g.DrawString("Tendencia", font, brush, new PointF(legendRect.X + 45, legendRect.Y + 30));
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
