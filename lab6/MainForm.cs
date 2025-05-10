using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    public partial class MainForm : Form
    {
        private List<Point> points = new List<Point>();
        private int selectedPointIndex = -1;
        private bool isDragging = false;
        private Color polylineColor = Color.Black;
        private Random rand = new Random();

        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.KeyPreview = true;
        }

        private void InitializeComponent()
        {
            this.Text = "Ломаная линия";
            this.Width = 800;
            this.Height = 600;

            Button btnGenerate = new Button() { Text = "Сгенерировать", Left = 10, Top = 10, Width = 120 };
            NumericUpDown numPoints = new NumericUpDown() { Left = 140, Top = 10, Width = 60, Minimum = 2, Maximum = 100, Value = 5 };
            Button btnColor = new Button() { Text = "Цвет", Left = 210, Top = 10, Width = 80 };

            btnGenerate.Click += (s, e) => {
                points.Clear();
                int count = (int)numPoints.Value;
                for (int i = 0; i < count; i++)
                {
                    points.Add(new Point(rand.Next(Width - 40) + 20, rand.Next(Height - 80) + 60));
                }
                selectedPointIndex = -1;
                Invalidate();
            };

            btnColor.Click += (s, e) => {
                using (ColorDialog dlg = new ColorDialog())
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        polylineColor = dlg.Color;
                        Invalidate();
                    }
                }
            };

            this.Controls.Add(btnGenerate);
            this.Controls.Add(numPoints);
            this.Controls.Add(btnColor);

            this.Paint += MainForm_Paint;
            this.MouseDown += MainForm_MouseDown;
            this.MouseMove += MainForm_MouseMove;
            this.MouseUp += MainForm_MouseUp;
            this.KeyDown += MainForm_KeyDown;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            if (points.Count < 2) return;

            using (Pen pen = new Pen(polylineColor, 2))
            {
                e.Graphics.DrawLines(pen, points.ToArray());
            }

            for (int i = 0; i < points.Count; i++)
            {
                Brush brush = (i == selectedPointIndex) ? Brushes.Red : Brushes.Blue;
                e.Graphics.FillEllipse(brush, points[i].X - 5, points[i].Y - 5, 10, 10);
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (Distance(e.Location, points[i]) < 10)
                {
                    selectedPointIndex = i;
                    isDragging = true;
                    Invalidate();
                    return;
                }
            }

            selectedPointIndex = -1;
            Invalidate();
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedPointIndex != -1)
            {
                points[selectedPointIndex] = e.Location;
                Invalidate();
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && selectedPointIndex != -1)
            {
                points.RemoveAt(selectedPointIndex);
                selectedPointIndex = -1;
                Invalidate();
            }
        }

        private double Distance(Point p1, Point p2)
        {
            int dx = p1.X - p2.X;
            int dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}


