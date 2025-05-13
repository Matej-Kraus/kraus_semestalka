using System;
using System.Drawing;
using System.Windows.Forms;

namespace kraus_semestalka.Components
{
    public class MotorcycleView : Control
    {
        private Image bikeImage = null!;  // null-forgiving, vždy nastavíme před vykreslením
        private float roll;

        public Image BikeImage
        {
            get => bikeImage;
            set { bikeImage = value; Invalidate(); }
        }

        public float Roll
        {
            get => roll;
            set { roll = value; Invalidate(); }
        }

        public MotorcycleView()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint,
                true);
            UpdateStyles();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (bikeImage == null) return;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float cx = Width / 2f;
            float cy = Height / 2f;

            g.TranslateTransform(cx, cy);
            g.RotateTransform(roll);

            float scale = Math.Min(Width / (float)bikeImage.Width,
                                   Height / (float)bikeImage.Height);
            float w = bikeImage.Width * scale;
            float h = bikeImage.Height * scale;

            g.DrawImage(bikeImage, -w / 2, -h / 2, w, h);
            g.ResetTransform();
        }
    }
}
