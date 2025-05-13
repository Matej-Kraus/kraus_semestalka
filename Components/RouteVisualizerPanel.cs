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
        private List<PointF> transformedPoints = new();
        private List<DriveData> transformedData = new();
        private DriveData? lastSelectedPoint;

        public List<DriveData> DriveDataPoints { get; set; } = new();

        // Režim zobrazení (true = zatáčky, false = akcelerace)
        private bool showTurnsMode = true;
        public bool ShowTurnsMode
        {
            get => showTurnsMode;
            set
            {
                if (showTurnsMode != value)
                {
                    showTurnsMode = value;
                    Invalidate();
                }
            }
        }

        // barvy pro zatáčky
        public Color ColorCurveLeft { get; set; } = Color.Blue;
        public Color ColorCurveRight { get; set; } = Color.Red;
        public float RollThreshold { get; set; } = 8f;

        // barvy pro akceleraci
        public Color ColorAccelPositive { get; set; } = Color.Green;
        public Color ColorAccelNegative { get; set; } = Color.Orange;
        public Color ColorAccelNeutral { get; set; } = Color.Gray;
        public float AccelTolerance { get; set; } = 0.02f;

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
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (DriveDataPoints == null || DriveDataPoints.Count < 2)
                return;

            ComputeTransformedPoints();

            using var penLeft = new Pen(ColorCurveLeft, 2);
            using var penRight = new Pen(ColorCurveRight, 2);
            using var penNeu = new Pen(ColorAccelNeutral, 2);
            using var penAccPos = new Pen(ColorAccelPositive, 2);
            using var penAccNeg = new Pen(ColorAccelNegative, 2);

            PointF? last = null;
            for (int i = 0; i < transformedPoints.Count; i++)
            {
                var pt = transformedPoints[i];
                var dd = transformedData[i];

                if (last != null)
                {
                    float roll = (float)dd.Roll;
                    float accel = (float)dd.Ax;

                    if (ShowTurnsMode)
                    {
                        // režim zatáčky
                        if (Math.Abs(roll) >= RollThreshold)
                            g.DrawLine(roll < 0 ? penLeft : penRight, last.Value, pt);
                        else
                            g.DrawLine(penNeu, last.Value, pt);
                    }
                    else
                    {
                        // režim akcelerace
                        if (accel > AccelTolerance)
                            g.DrawLine(penAccPos, last.Value, pt);
                        else if (accel < -AccelTolerance)
                            g.DrawLine(penAccNeg, last.Value, pt);
                        else
                            g.DrawLine(penNeu, last.Value, pt);
                    }
                }
                last = pt;
            }

            // hover highlight
            if (lastSelectedPoint != null)
            {
                int idx = transformedData.IndexOf(lastSelectedPoint);
                if (idx >= 0)
                {
                    var hp = transformedPoints[idx];
                    g.FillEllipse(Brushes.Red, hp.X - 5, hp.Y - 5, 10, 10);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            float threshold = 10f;
            DriveData? nearest = null;

            for (int i = 0; i < transformedPoints.Count; i++)
            {
                float dx = transformedPoints[i].X - e.X;
                float dy = transformedPoints[i].Y - e.Y;
                float d = MathF.Sqrt(dx * dx + dy * dy);
                if (d < threshold)
                {
                    threshold = d;
                    nearest = transformedData[i];
                }
            }

            if (nearest != lastSelectedPoint)
            {
                lastSelectedPoint = nearest;
                if (nearest != null)
                {
                    PointSelected?.Invoke(nearest);
                    hoverTip.Show(
                        $"Rychlost: {nearest.SpeedRec:0.0} m/s\n" +
                        $"Náklon:   {nearest.Roll:0.0}°\n" +
                        $"Akcel.:   {nearest.Ax:0.00} m/s²",
                        this, e.Location.X + 15, e.Location.Y + 15, 1500);
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

            float scale = Math.Min((Width - 2 * pad) / rangeX,
                                   (Height - 2 * pad) / rangeY);
            float offsetX = (Width - rangeX * scale) / 2f;
            float offsetY = (Height - rangeY * scale) / 2f;

            foreach (var d in DriveDataPoints)
            {
                if (!d.LatRec.HasValue || !d.LonRec.HasValue) continue;
                float x = ((float)d.LonRec.GetValueOrDefault() - minX) * scale + offsetX;
                float y = ((float)d.LatRec.GetValueOrDefault() - minY) * scale + offsetY;
                transformedPoints.Add(new PointF(x, Height - y));
                transformedData.Add(d);
                transformedData.Add(d);
                //hovno
            }
        }
    }
}
