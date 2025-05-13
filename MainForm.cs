using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using kraus_semestalka.Components;
using kraus_semestalka.Data;
using kraus_semestalka.Data.Models;
using kraus_semestalka.Properties;

namespace kraus_semestalka
{
    public partial class MainForm : Form
    {
        private SplitContainer splitLeftRight, splitCenterRight;
        private ComboBox comboRecordings;
        private ListBox listRecordings;
        private GroupBox groupModes;
        private RadioButton radioTurns, radioSpeed;
        private Button btnSettings;
        private RouteVisualizerPanel panelVisualizer;
        private DriveDataDetailView detailView;
        private MotorcycleView motorcycleView;
        private DriveData? currentHoveredPoint;

        public MainForm()
        {
            InitializeComponent();

            Text = "Vizualizace jízdy";
            MinimumSize = new Size(1280, 720);
            StartPosition = FormStartPosition.CenterScreen;

            InitLayout();
            InitMockData();
            ApplySettings();
        }

        private void InitLayout()
        {
            comboRecordings = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10),
                Height = 30
            };
            comboRecordings.SelectedIndexChanged += ComboRecordings_SelectedIndexChanged;

            splitLeftRight = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 30
            };
            Controls.Add(splitLeftRight);

            splitCenterRight = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 850
            };
            splitLeftRight.Panel2.Controls.Add(splitCenterRight);

            listRecordings = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10)
            };

            groupModes = new GroupBox
            {
                Text = "Režim vykreslení",
                Dock = DockStyle.Top,
                Height = 80,
                Padding = new Padding(10)
            };
            radioTurns = new RadioButton
            {
                Text = "Zatáčky",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 9),
                Checked = true
            };
            radioSpeed = new RadioButton
            {
                Text = "Rychlost / Akcelerace",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 9)
            };
            radioTurns.CheckedChanged += (s, e) => SwitchMode();
            radioSpeed.CheckedChanged += (s, e) => SwitchMode();
            groupModes.Controls.Add(radioSpeed);
            groupModes.Controls.Add(radioTurns);

            btnSettings = new Button
            {
                Text = "Nastavení",
                Dock = DockStyle.Bottom,
                Height = 30,
                Font = new Font("Segoe UI", 9)
            };
            btnSettings.Click += (s, e) => {
                using var dlg = new SettingsForm();
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    ApplySettings();
            };

            splitLeftRight.Panel1.Controls.Add(listRecordings);
            splitLeftRight.Panel1.Controls.Add(comboRecordings);
            splitLeftRight.Panel1.Controls.Add(groupModes);
            splitLeftRight.Panel1.Controls.Add(btnSettings);

            detailView = new DriveDataDetailView
            {
                Dock = DockStyle.Top
            };
            splitCenterRight.Panel2.Controls.Add(detailView);

            motorcycleView = new MotorcycleView
            {
                Dock = DockStyle.Fill,
                BikeImage = LoadBikeImage()
            };
            splitCenterRight.Panel2.Controls.Add(motorcycleView);

            panelVisualizer = new RouteVisualizerPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            panelVisualizer.PointSelected += OnPointSelected;
            splitCenterRight.Panel1.Controls.Add(panelVisualizer);
        }

        private Image LoadBikeImage()
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagine", "bike.png");
            if (!File.Exists(file))
                throw new FileNotFoundException($"Motorka nenalezena: {file}");
            return Image.FromFile(file);
        }

        private void InitMockData()
        {
            comboRecordings.Items.Clear();
            foreach (var r in DataService.GetRecordings())
            {
                string label = !string.IsNullOrEmpty(r.UIID)
                    ? r.UIID : $"ID {r.Id}";
                comboRecordings.Items.Add(new ComboBoxItem
                {
                    Text = $"{label} – {r.StartDateTime:dd.MM.yyyy} – {r.SensorsDeviceName}",
                    Value = r.Id
                });
            }
            if (comboRecordings.Items.Count > 0)
                comboRecordings.SelectedIndex = 0;
        }

        private void SwitchMode()
        {
            panelVisualizer.ShowTurnsMode = radioTurns.Checked;
            if (Settings.Default.AutoRedraw)
                panelVisualizer.Invalidate();
        }

        private void ComboRecordings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboRecordings.SelectedItem is ComboBoxItem sel)
            {
                var data = DataService.GetDriveDataByRecordingId(sel.Value);
                panelVisualizer.DriveDataPoints = data;
                panelVisualizer.ShowTurnsMode = radioTurns.Checked;
                if (Settings.Default.AutoRedraw)
                    panelVisualizer.Invalidate();
            }
        }

        private void OnPointSelected(DriveData pt)
        {
            detailView.UpdateWith(pt);
            currentHoveredPoint = pt;
            motorcycleView.Roll = (float)pt.Roll;
        }

        private void ApplySettings()
        {
            var s = Settings.Default;
            panelVisualizer.ColorCurveLeft = s.ColorCurveLeft;
            panelVisualizer.ColorCurveRight = s.ColorCurveRight;
            panelVisualizer.ColorAccelPositive = s.ColorAccelPositive;
            panelVisualizer.ColorAccelNegative = s.ColorAccelNegative;
            panelVisualizer.AccelTolerance = s.AccelTolerance;
            panelVisualizer.Invalidate();
        }
    }

    public class ComboBoxItem
    {
        public string Text { get; set; } = "";
        public int Value { get; set; }
        public override string ToString() => Text;
    }
}
