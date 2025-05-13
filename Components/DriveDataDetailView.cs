using System;
using System.Drawing;
using System.Windows.Forms;
using kraus_semestalka.Data.Models;

namespace kraus_semestalka.Components
{
    public class DriveDataDetailView : Panel
    {
        private readonly Label labelSpeed;
        private readonly Label labelRoll;
        private readonly Label labelAccel;

        public DriveDataDetailView()
        {
            Dock = DockStyle.Top;
            Height = 100;
            Padding = new Padding(10);

            labelSpeed = CreateLabel("Rychlost:");
            labelRoll = CreateLabel("Náklon:");
            labelAccel = CreateLabel("Akcelerace:");

            Controls.Add(labelAccel);
            Controls.Add(labelRoll);
            Controls.Add(labelSpeed);
        }

        private Label CreateLabel(string text) => new()
        {
            Text = text,
            Dock = DockStyle.Top,
            Font = new Font("Segoe UI", 10)
        };

        public void UpdateWith(DriveData pt)
        {
            labelSpeed.Text = $"Rychlost: {pt.SpeedRec:0.0} m/s";
            labelRoll.Text = $"Náklon:   {pt.Roll:0.0}°";
            labelAccel.Text = $"Akcelerace: {pt.Ax:0.00} m/s²";
        }
    }
}
