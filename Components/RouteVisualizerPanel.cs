using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using kraus_semestalka.Data.Models;

namespace kraus_semestalka.Components
{
    public class RouteVisualizerPanel : Panel
    {
        public event Action<DriveData>? PointSelected;

        private readonly ToolTip hoverTip = new ToolTip();
        private readonly List<PointF> transformedPoints = new List<PointF>();
        private readonly List<DriveData> transformedData = new List<DriveData>();
        private List<DriveData> previousData = new List<DriveData>();
        private DriveData? lastSelectedPoint;

        public List<DriveData> DriveDataPoints { get; set; } = new List<DriveData>();
        public bool ShowTurnsMode { get; set; } = true;

        // barvy pro zatáčky
        public Color ColorCurveLeft { get; set; } = Color.Blue;
        public Color ColorCurveRight { get; set; } = Color.Red;
        public float RollThreshold { get; set; } = 5f;

        // barvy pro rychlost (gradient)
        public Color SpeedMinColor { get; set; } = Color.LightBlue;
        public Color SpeedMaxColor { get; set; } = Color.DarkRed;

        // barvy pro akceleraci
        public Color ColorAccelPositive { get; set; } = Color.Green;
        public Color ColorAccelNegative { get; set; } = Color.Orange;
        public Color ColorNeutral { get; set; } = Color.Gray;
        public float AccelTolerance { get; set; } = 0.02f;

        private float minSpeed, maxSpeed;

        public RouteVisualizerPanel()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);
            UpdateStyles();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (DriveDataPoints == null || DriveDataPoints.Count < 2)
                return;

            // přepočítání bodů a rychlostního rozsahu
            if (!DriveDataPoints.SequenceEqual(previousData))
            {
                ComputeTransformedPoints();
                previousData = new List<DriveData>(DriveDataPoints);
                minSpeed = DriveDataPoints.Min(d => (float)d.SpeedRec);
                maxSpeed = DriveDataPoints.Max(d => (float)d.SpeedRec);
            }

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var penLeft = new Pen(ColorCurveLeft, 2);
            using var penRight = new Pen(ColorCurveRight, 2);
            using var penNeu = new Pen(ColorNeutral, 2);

            PointF? last = null;
            for (int i = 0; i < transformedPoints.Count; i++)
            {
                var pt = transformedPoints[i];
                var dd = transformedData[i];

                if (last != null)
                {
                    float roll = (float)dd.Roll;
                    float speed = (float)dd.SpeedRec;
                    float accel = (float)dd.Ax;

                    if (ShowTurnsMode)
                    {
                        // režim zatáček
                        if (Math.Abs(roll) >= RollThreshold)
                            g.DrawLine(roll < 0 ? penLeft : penRight, last.Value, pt);
                        else
                            g.DrawLine(penNeu, last.Value, pt);

                        
                    }
                    else
                    {
                        // režim rychlosti/akcelerace
                        float ratio = maxSpeed > minSpeed
                            ? (speed - minSpeed) / (maxSpeed - minSpeed)
                            : 0f;
                        using var penSpeed = new Pen(
                            LerpColor(SpeedMinColor, SpeedMaxColor, ratio), 2);
                        g.DrawLine(penSpeed, last.Value, pt);

                        Color dotColor = accel > AccelTolerance
                            ? ColorAccelPositive
                            : accel < -AccelTolerance
                                ? ColorAccelNegative
                                : ColorNeutral;
                        using var brush = new SolidBrush(dotColor);
                        g.FillEllipse(brush, pt.X - 4, pt.Y - 4, 8, 8);
                    }
                }

                last = pt;
            }

            // vykreslení začátku a konce jízdy
            if (transformedPoints.Count > 1)
            {
                var first = transformedPoints[0];
                var lastp = transformedPoints[^1];
                g.FillEllipse(Brushes.Green, first.X - 6, first.Y - 6, 12, 12);
                g.FillEllipse(Brushes.Red, lastp.X - 6, lastp.Y - 6, 12, 12);
            }

            // zvýraznění hover bodu
            if (lastSelectedPoint != null)
            {
                int idx = transformedData.IndexOf(lastSelectedPoint);
                if (idx >= 0)
                {
                    var hp = transformedPoints[idx];
                    g.FillEllipse(Brushes.Magenta, hp.X - 5, hp.Y - 5, 10, 10);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            float threshold = 10f;
            DriveData? nearest = null;
            PointF? np = null;

            foreach (var (pt, dd) in transformedPoints.Zip(transformedData, (pt, dd) => (pt, dd)))
            {
                float dx = pt.X - e.X, dy = pt.Y - e.Y;
                float d = MathF.Sqrt(dx * dx + dy * dy);
                if (d < threshold)
                {
                    threshold = d;
                    nearest = dd;
                    np = pt;
                }
            }

            if (nearest != lastSelectedPoint)
            {
                lastSelectedPoint = nearest;
                if (nearest != null)
                {
                    PointSelected?.Invoke(nearest);
                    hoverTip.Show(
                        $"Rychl.: {nearest.SpeedRec:0.0} m/s\n" +
                        $"Roll:   {nearest.Roll:0.0}°\n" +
                        $"Akc.:   {nearest.Ax:0.00} m/s²",
                        this, (int)(np!.Value.X + 15), (int)(np!.Value.Y + 15), 1500);
                }
                else
                {
                    hoverTip.Hide(this);
                }
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            lastSelectedPoint = null;
            hoverTip.Hide(this);
            Invalidate();
        }

        private void ComputeTransformedPoints()
        {
            transformedPoints.Clear();
            transformedData.Clear();

            const float pad = 20f;
            float minX = (float)DriveDataPoints.Min(d => d.LonRec.GetValueOrDefault());
            float maxX = (float)DriveDataPoints.Max(d => d.LonRec.GetValueOrDefault());
            float minY = (float)DriveDataPoints.Min(d => d.LatRec.GetValueOrDefault());
            float maxY = (float)DriveDataPoints.Max(d => d.LatRec.GetValueOrDefault());
            float rangeX = maxX - minX + 0.0001f;
            float rangeY = maxY - minY + 0.0001f;

            float scale = Math.Min((Width - 2 * pad) / rangeX, (Height - 2 * pad) / rangeY);
            float offsetX = (Width - rangeX * scale) / 2f;
            float offsetY = (Height - rangeY * scale) / 2f;

            foreach (var d in DriveDataPoints)
            {
                if (!d.LatRec.HasValue || !d.LonRec.HasValue) continue;
                float x = (float)((d.LonRec.GetValueOrDefault() - minX) * scale + offsetX);
                float y = (float)((d.LatRec.GetValueOrDefault() - minY) * scale + offsetY);
                transformedPoints.Add(new PointF(x, Height - y));
                transformedData.Add(d);
            }
        }

        private Color LerpColor(Color a, Color b, float t)
        {
            t = Math.Clamp(t, 0f, 1f);
            return Color.FromArgb(
                (int)(a.A + (b.A - a.A) * t),
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t)
            );
        }
    }
}
