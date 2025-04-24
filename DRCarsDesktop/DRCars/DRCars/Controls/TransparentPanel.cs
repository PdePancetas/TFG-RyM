using System;
using System.Drawing;
using System.Windows.Forms;

namespace DRCars.Controls
{
    public class TransparentPanel : Panel
    {
        private double opacity = 100.0;

        public double Opacity
        {
            get { return opacity; }
            set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;

                if (opacity != value)
                {
                    opacity = value;
                    this.Invalidate();
                }
            }
        }

        public TransparentPanel()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(opacity * 255 / 100), this.BackColor)))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }

            base.OnPaint(e);
        }
    }
}
