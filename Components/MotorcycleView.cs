using System;
using System.Drawing;
using System.Windows.Forms;

namespace kraus_semestalka.Components
{
    public class MotorcycleView : Control
    {
        private Image bikeImage;
        private float roll;

        /// <summary>
        /// Obrázek řídítek + přístrojové desky. Můžeš jej nastavit z Properties.Resources nebo načíst z disku.
        /// </summary>
        public Image BikeImage
        {
            get => bikeImage;
            set
            {
                bikeImage = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Náklon v stupních (kladné = doprava, záporné = doleva).
        /// </summary>
        public float Roll
        {
            get => roll;
            set
            {
                roll = value;
                Invalidate();
            }
        }

        public MotorcycleView()
        {
            // povolíme hladké překreslování i otočení
            SetStyle(ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.UserPaint,
                     true);
            UpdateStyles();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (bikeImage == null) return;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // střed pro rotaci: střed kontroly
            float cx = Width / 2f;
            float cy = Height / 2f;

            // přepneme do souřadnic rotace
            g.TranslateTransform(cx, cy);
            g.RotateTransform(roll);

            // spočítáme, jak obrázek umístit, aby zůstal vycentrovaný
            float imgW = bikeImage.Width;
            float imgH = bikeImage.Height;
            float scale = Math.Min(Width / imgW, Height / imgH);

            float drawW = imgW * scale;
            float drawH = imgH * scale;

            // vykreslíme obrázek tak, aby jeho střed byl ve (0,0)
            g.DrawImage(
                bikeImage,
                -drawW / 2, -drawH / 2,
                drawW, drawH);

            // vrátíme transformaci zpátky
            g.ResetTransform();
        }
    }
}
