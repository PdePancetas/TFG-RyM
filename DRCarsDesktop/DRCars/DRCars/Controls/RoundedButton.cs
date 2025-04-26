using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DRCars.Controls
{
    public class RoundedButton : Button
    {
        private int borderRadius = 10;
        private Color borderColor = Color.Silver;
        private int borderSize = 0;
        private bool isHovering = false;

        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                borderRadius = value;
                this.Invalidate();
            }
        }

        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }

        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                borderSize = value;
                this.Invalidate();
            }
        }

        public RoundedButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(150, 40);
            this.BackColor = Color.FromArgb(0, 120, 215);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);
            this.Cursor = Cursors.Hand;

            this.MouseEnter += (sender, e) =>
            {
                isHovering = true;
                this.Invalidate();
            };

            this.MouseLeave += (sender, e) =>
            {
                isHovering = false;
                this.Invalidate();
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Rectangle rectSurface = this.ClientRectangle;
            Rectangle rectBorder = Rectangle.Inflate(rectSurface, -borderSize, -borderSize);
            int smoothSize = 2;

            if (borderRadius > 2) // Rounded button
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
                using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    // Button surface
                    this.Region = new Region(pathSurface);

                    // Draw surface border for HD result
                    e.Graphics.DrawPath(penSurface, pathSurface);

                    // Button border
                    if (borderSize >= 1)
                        e.Graphics.DrawPath(penBorder, pathBorder);

                    // Apply hover effect
                    if (isHovering)
                    {
                        using (SolidBrush brushHover = new SolidBrush(Color.FromArgb(30, Color.White)))
                        {
                            e.Graphics.FillPath(brushHover, pathSurface);
                        }
                    }
                }
            }
            else // Normal button
            {
                e.Graphics.SmoothingMode = SmoothingMode.None;

                // Button surface
                this.Region = new Region(rectSurface);

                // Button border
                if (borderSize >= 1)
                {
                    using (Pen penBorder = new Pen(borderColor, borderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        e.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                    }
                }

                // Apply hover effect
                if (isHovering)
                {
                    using (SolidBrush brushHover = new SolidBrush(Color.FromArgb(30, Color.White)))
                    {
                        e.Graphics.FillRectangle(brushHover, rectSurface);
                    }
                }
            }
        }

        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
