using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DRCars.Controls
{
    public class RoundedComboBox : ComboBox
    {
        private int borderRadius = 10;
        private Color borderColor = Color.FromArgb(8, 146, 208);
        private int borderSize = 1;
        private bool isFocused = false;
        private Color borderFocusColor = Color.FromArgb(8, 146, 208);

        public RoundedComboBox()
        {
            this.DropDownStyle = ComboBoxStyle.DropDownList;
            this.FlatStyle = FlatStyle.Flat;
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.BackColor = Color.FromArgb(60, 70, 75);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 11F);
            this.DrawItem += new DrawItemEventHandler(ComboBox_DrawItem);
        }

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

        public Color BorderFocusColor
        {
            get { return borderFocusColor; }
            set
            {
                borderFocusColor = value;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Corregido: Eliminado el uso de float en rectángulos
            Rectangle rectComboBox = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            Rectangle rectDropButton = new Rectangle(this.Width - 30, 0, 30, this.Height);

            // Draw the background of the ComboBox
            using (SolidBrush brushComboBox = new SolidBrush(this.BackColor))
            {
                g.FillRectangle(brushComboBox, rectComboBox);
            }

            // Draw the border of the ComboBox
            using (GraphicsPath pathBorder = GetFigurePath(rectComboBox, borderRadius))
            using (Pen penBorder = new Pen(isFocused ? borderFocusColor : borderColor, borderSize))
            {
                this.Region = new Region(pathBorder);
                g.DrawPath(penBorder, pathBorder);
            }

            // Draw the dropdown button
            using (SolidBrush brushDropButton = new SolidBrush(Color.FromArgb(70, 80, 85)))
            using (SolidBrush brushArrow = new SolidBrush(Color.White))
            {
                g.FillRectangle(brushDropButton, rectDropButton);

                // Draw the arrow
                Point[] arrow = new Point[]
                {
                    new Point(this.Width - 20, this.Height / 2 - 2),
                    new Point(this.Width - 15, this.Height / 2 + 3),
                    new Point(this.Width - 10, this.Height / 2 - 2)
                };
                g.FillPolygon(brushArrow, arrow);
            }
        }

        private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            // Set the text color based on whether the item is selected
            Color textColor = e.State.HasFlag(DrawItemState.Selected) ? Color.FromArgb(8, 146, 208) : this.ForeColor;

            // Draw the item text
            using (SolidBrush brushText = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, brushText, e.Bounds.X + 5, e.Bounds.Y + 2);
            }

            e.DrawFocusRectangle();
        }

        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);
            isFocused = true;
            this.Invalidate();
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);
            isFocused = false;
            this.Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            isFocused = true;
            this.Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            isFocused = false;
            this.Invalidate();
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
