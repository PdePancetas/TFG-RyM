using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DRCars.Controls
{
    public class RoundedTextBox : UserControl
    {
        private Color borderColor = Color.Silver;
        private int borderSize = 1;
        private int borderRadius = 10;
        private Color placeholderColor = Color.DarkGray;
        private string placeholderText = "";
        private bool isPlaceholderActive = false;
        private bool isPasswordChar = false;
        private TextBox textBox;

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

        public Color PlaceholderColor
        {
            get { return placeholderColor; }
            set
            {
                placeholderColor = value;
                if (isPlaceholderActive)
                    textBox.ForeColor = value;
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

        public bool PasswordChar
        {
            get { return isPasswordChar; }
            set
            {
                isPasswordChar = value;
                if (!isPlaceholderActive)
                    textBox.UseSystemPasswordChar = value;
            }
        }

        public string Texts
        {
            get
            {
                if (isPlaceholderActive)
                    return "";
                else
                    return textBox.Text;
            }
            set
            {
                textBox.Text = value;
                SetPlaceholder();
            }
        }

        public RoundedTextBox()
        {
            this.Size = new Size(250, 40);
            this.BackColor = Color.White;
            this.Padding = new Padding(10, 7, 10, 7);
            this.Margin = new Padding(4);

            textBox = new TextBox();
            textBox.BorderStyle = BorderStyle.None;
            textBox.BackColor = this.BackColor;
            textBox.Font = new Font("Segoe UI", 9.5F);
            textBox.Dock = DockStyle.Fill;
            textBox.Enter += TextBox_Enter;
            textBox.Leave += TextBox_Leave;
            textBox.TextChanged += TextBox_TextChanged;

            this.Controls.Add(textBox);
            SetPlaceholder();
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (TextChanged != null)
                TextChanged.Invoke(sender, e);
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            if (isPlaceholderActive)
            {
                isPlaceholderActive = false;
                textBox.Text = "";
                textBox.ForeColor = Color.Black;
                if (isPasswordChar)
                    textBox.UseSystemPasswordChar = true;
            }
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            SetPlaceholder();
        }

        private void SetPlaceholder()
        {
            if (string.IsNullOrWhiteSpace(textBox.Text) && placeholderText != "")
            {
                isPlaceholderActive = true;
                textBox.Text = placeholderText;
                textBox.ForeColor = placeholderColor;
                if (isPasswordChar)
                    textBox.UseSystemPasswordChar = false;
            }
            else
            {
                isPlaceholderActive = false;
                textBox.ForeColor = Color.Black;
                if (isPasswordChar)
                    textBox.UseSystemPasswordChar = true;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            if (borderRadius > 1) // Rounded TextBox
            {
                // Draw border
                using (GraphicsPath path = GetFigurePath(this.ClientRectangle, borderRadius))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                using (SolidBrush brushBg = new SolidBrush(this.BackColor))
                {
                    this.Region = new Region(path);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.FillPath(brushBg, path); // Fill background
                    g.DrawPath(penBorder, path); // Draw border
                }
            }
            else // Normal TextBox
            {
                // Draw border
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    this.Region = new Region(this.ClientRectangle);
                    g.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
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
