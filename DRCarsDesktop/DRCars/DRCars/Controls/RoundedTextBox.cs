using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DRCars.Controls
{
    public class RoundedTextBox : UserControl
    {
        private TextBox textBox;
        private Color borderColor = Color.FromArgb(206, 212, 218);
        private Color borderFocusColor = Color.FromArgb(0, 160, 157);
        private int borderSize = 1;
        private int borderRadius = 4;
        private bool underlinedStyle = false;
        private bool passwordChar = false;
        private bool isFocused = false;
        private string placeholderText = "";
        private Color placeholderColor = Color.DarkGray;

        public event EventHandler TextChanged;

        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }

        public Color BorderFocusColor
        {
            get { return borderFocusColor; }
            set { borderFocusColor = value; }
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

        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                if (value >= 0)
                {
                    borderRadius = value;
                    this.Invalidate();
                }
            }
        }

        public bool UnderlinedStyle
        {
            get { return underlinedStyle; }
            set
            {
                underlinedStyle = value;
                this.Invalidate();
            }
        }

        public bool PasswordChar
        {
            get { return passwordChar; }
            set
            {
                passwordChar = value;
                textBox.UseSystemPasswordChar = value;
            }
        }

        public string Texts
        {
            get
            {return isPlaceholder ? string.Empty : textBox.Text;}
            set
            {
                isPlaceholder = false;
                textBox.ForeColor = this.ForeColor;
                textBox.Text = value;
            }
        }


        public string PlaceholderText
        {
            get { return placeholderText; }
            set
            {
                placeholderText = value;
                textBox.Text = "";
                SetPlaceholder();
            }
        }

        public Color PlaceholderColor
        {
            get { return placeholderColor; }
            set
            {
                placeholderColor = value;
                if (isPlaceholder)
                    textBox.ForeColor = value;
            }
        }

        private bool isPlaceholder = false;

        public RoundedTextBox()
        {
            textBox = new TextBox();
            this.SuspendLayout();

            // TextBox
            textBox.BackColor = this.BackColor;
            textBox.BorderStyle = BorderStyle.None;
            textBox.Dock = DockStyle.Fill;
            textBox.Location = new Point(10, 7);
            textBox.Name = "textBox";
            textBox.Size = new Size(this.Width - 20, this.Height - 14);
            textBox.Font = new Font("Segoe UI", 10F);
            textBox.Enter += TextBox_Enter;
            textBox.Leave += TextBox_Leave;
            textBox.KeyPress += TextBox_KeyPress;
            textBox.TextChanged += TextBox_TextChanged;

            // Control
            this.Controls.Add(textBox);
            this.Size = new Size(250, 40);
            this.ForeColor = Color.FromArgb(51, 51, 51);
            this.BackColor = Color.White;
            this.Padding = new Padding(10, 7, 10, 7);
            this.Font = new Font("Segoe UI", 10F);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void SetPlaceholder()
        {
            if (string.IsNullOrWhiteSpace(textBox.Text) && !isFocused && placeholderText != "")
            {
                isPlaceholder = true;
                textBox.Text = placeholderText;
                textBox.ForeColor = placeholderColor;
            }
        }


        private void RemovePlaceholder()
        {
            if (isPlaceholder && placeholderText != "")
            {
                isPlaceholder = false;
                textBox.Text = "";
                textBox.ForeColor = this.ForeColor;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            if (borderRadius > 1) // Rounded TextBox
            {
                // Fields
                var rectBorderSmooth = this.ClientRectangle;
                var rectBorder = Rectangle.Inflate(rectBorderSmooth, -borderSize, -borderSize);
                int smoothSize = borderSize > 0 ? borderSize : 1;

                using (GraphicsPath pathBorderSmooth = GetFigurePath(rectBorderSmooth, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
                using (Pen penBorderSmooth = new Pen(this.Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    // Drawing
                    this.Region = new Region(pathBorderSmooth); // Set the rounded region of UserControl
                    if (borderRadius > 15) SetTextBoxRoundedRegion(); // Set the rounded region of TextBox
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    penBorder.Alignment = PenAlignment.Center;

                    if (isFocused) penBorder.Color = borderFocusColor;

                    if (underlinedStyle) // Line Style
                    {
                        // Draw border smoothing
                        g.DrawPath(penBorderSmooth, pathBorderSmooth);
                        // Draw border
                        g.SmoothingMode = SmoothingMode.None;
                        g.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                    }
                    else // Normal Style
                    {
                        // Draw border smoothing
                        g.DrawPath(penBorderSmooth, pathBorderSmooth);
                        // Draw border
                        g.DrawPath(penBorder, pathBorder);
                    }
                }
            }
            else // Square/Normal TextBox
            {
                // Draw border
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    this.Region = new Region(this.ClientRectangle);
                    penBorder.Alignment = PenAlignment.Inset;

                    if (isFocused) penBorder.Color = borderFocusColor;

                    if (underlinedStyle) // Line Style
                        g.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                    else // Normal Style
                        g.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
                }
            }
        }

        private void SetTextBoxRoundedRegion()
        {
            GraphicsPath pathTxt = GetFigurePath(textBox.ClientRectangle, borderRadius - borderSize);
            textBox.Region = new Region(pathTxt);
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

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (TextChanged != null)
                TextChanged.Invoke(sender, e);
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            isFocused = true;
            this.Invalidate();
            RemovePlaceholder();
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            isFocused = false;
            this.Invalidate();
            SetPlaceholder();
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }
    }
}
