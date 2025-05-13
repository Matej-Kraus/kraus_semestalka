using System;
using System.Drawing;
using System.Windows.Forms;
using kraus_semestalka.Data.Models;

namespace kraus_semestalka.Components
{
    public class DriveDataDetailView : Panel
    {
        private Label labelSpeed;
        private Label labelRoll;
        private Label labelAccel;

        public DriveDataDetailView()
        {
            this.Dock = DockStyle.Top;
            this.Height = 150;
            this.Padding = new Padding(10);

            // Vytvoření tří popisků
            labelSpeed = CreateLabel("Rychlost:");
            labelRoll = CreateLabel("Náklon:");
            labelAccel = CreateLabel("Zrychlení:");

            // Přidání do panelu (poslední přidaný je nahoře)
            this.Controls.Add(labelAccel);
            this.Controls.Add(labelRoll);
            this.Controls.Add(labelSpeed);
        }

        private Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 10)
            };
        }

        /// <summary>
        /// Aktualizuje popisky podle dat z vybraného DriveData bodu.
        /// </summary>
        public void UpdateWith(DriveData point)
        {
            // Ochrana proti chybě
            if (labelSpeed == null || labelRoll == null || labelAccel == null)
            {
                MessageBox.Show("Chyba: Komponenta není správně inicializována.");
                return;
            }

            labelSpeed.Text = $"Rychlost: {point.SpeedRec:0.0} m/s";
            labelRoll.Text = $"Náklon: {point.Roll:0.0}°";
            labelAccel.Text = $"Zrychlení: {point.Ax:0.00} m/s²";
        }
    }
}
