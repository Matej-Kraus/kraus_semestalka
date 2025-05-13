using System;
using System.Drawing;
using System.Windows.Forms;
using kraus_semestalka.Properties;

namespace kraus_semestalka.Components
{
    public class SettingsForm : Form
    {
        private CheckBox chkAutoRedraw;
        private Button btnCurveLeftColor, btnCurveRightColor;
        private Button btnAccelPosColor, btnAccelNegColor;
        private NumericUpDown numAccelTol;
        private Label lblAccelTol;
        private Button btnResetDefaults, btnOK, btnCancel;

        public SettingsForm()
        {
            Text = "Nastavení aplikace";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            Width = 440;
            Height = 360;

            InitializeComponents();
            LoadSettings();
        }

        private void InitializeComponents()
        {
            chkAutoRedraw = new CheckBox
            {
                Text = "Automatické překreslení po výběru jízdy",
                AutoSize = true,
                Top = 20,
                Left = 20
            };

            btnCurveLeftColor = new Button
            {
                Text = "Barva zatáček vlevo",
                Width = 150,
                Top = 60,
                Left = 20
            };
            btnCurveLeftColor.Click += (s, e) =>
                PickColor(btnCurveLeftColor, Settings.Default.ColorCurveLeft);

            btnCurveRightColor = new Button
            {
                Text = "Barva zatáček vpravo",
                Width = 150,
                Top = 100,
                Left = 20
            };
            btnCurveRightColor.Click += (s, e) =>
                PickColor(btnCurveRightColor, Settings.Default.ColorCurveRight);

            btnAccelPosColor = new Button
            {
                Text = "Barva akcelerace +",
                Width = 150,
                Top = 140,
                Left = 20
            };
            btnAccelPosColor.Click += (s, e) =>
                PickColor(btnAccelPosColor, Settings.Default.ColorAccelPositive);

            btnAccelNegColor = new Button
            {
                Text = "Barva akcelerace -",
                Width = 150,
                Top = 180,
                Left = 20
            };
            btnAccelNegColor.Click += (s, e) =>
                PickColor(btnAccelNegColor, Settings.Default.ColorAccelNegative);

            lblAccelTol = new Label
            {
                Text = "Tolerance akcelerace:",
                AutoSize = true,
                Top = 220,
                Left = 20
            };

            numAccelTol = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 5,
                DecimalPlaces = 3,
                Increment = 0.01M,
                Top = 220,
                Left = 150,
                Width = 80
            };

            btnResetDefaults = new Button
            {
                Text = "Obnovit výchozí",
                Width = 120,
                Top = 260,
                Left = 20
            };
            btnResetDefaults.Click += (s, e) => {
                Settings.Default.Reset();
                LoadSettings();
            };

            btnOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Width = 80,
                Top = 260,
                Left = 260
            };
            btnOK.Click += (s, e) => {
                SaveSettings();
                Close();
            };

            btnCancel = new Button
            {
                Text = "Storno",
                DialogResult = DialogResult.Cancel,
                Width = 80,
                Top = 260,
                Left = 350
            };

            Controls.AddRange(new Control[]{
                chkAutoRedraw,
                btnCurveLeftColor, btnCurveRightColor,
                btnAccelPosColor,  btnAccelNegColor,
                lblAccelTol,       numAccelTol,
                btnResetDefaults,  btnOK, btnCancel
            });
        }

        private void PickColor(Button btn, Color initial)
        {
            using var dlg = new ColorDialog { Color = initial };
            if (dlg.ShowDialog(this) == DialogResult.OK)
                btn.BackColor = dlg.Color;
        }

        private void LoadSettings()
        {
            chkAutoRedraw.Checked = Settings.Default.AutoRedraw;
            btnCurveLeftColor.BackColor = Settings.Default.ColorCurveLeft;
            btnCurveRightColor.BackColor = Settings.Default.ColorCurveRight;
            btnAccelPosColor.BackColor = Settings.Default.ColorAccelPositive;
            btnAccelNegColor.BackColor = Settings.Default.ColorAccelNegative;
            numAccelTol.Value = (decimal)Settings.Default.AccelTolerance;
        }

        private void SaveSettings()
        {
            Settings.Default.AutoRedraw = chkAutoRedraw.Checked;
            Settings.Default.ColorCurveLeft = btnCurveLeftColor.BackColor;
            Settings.Default.ColorCurveRight = btnCurveRightColor.BackColor;
            Settings.Default.ColorAccelPositive = btnAccelPosColor.BackColor;
            Settings.Default.ColorAccelNegative = btnAccelNegColor.BackColor;
            Settings.Default.AccelTolerance = (float)numAccelTol.Value;
            Settings.Default.Save();
        }
    }
}
