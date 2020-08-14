using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Plot {
    class PlotBuilder { 
        private class Plot {
            public System.Windows.Media.SolidColorBrush brush = SystemColors.HotTrackBrush;
            public Polyline curve;
            public Func<double, double> func;

            public Plot(Func<double, double> f, Polyline curve) {
                this.curve = curve;
                this.func = f;
                this.curve.Stroke = brush;
            }
        }
        Canvas paper = null;
        Line ox = new Line();
        Line oy = new Line();
        //public Func<double, double> func = null;
        List<Plot> plots;
        double _scale;
        public bool ShowAxes { get; set; } = true;
        public double Scale {
            get {
                return _scale;
            }
            set {
                this.PointBuilder.Mul = value;
                _scale = value;
            } 
        }
        public PointBuilder PointBuilder { get; private set; }
        public PlotBuilder(Canvas canvas, int scale) {
            this.PointBuilder = new PointBuilder(canvas, scale);
            this.paper = canvas;
            this.Scale = scale;
            this.plots = new List<Plot>();

        }
        public void DrawPlot(Func<double, double> fn) {
            Plot plt = new Plot(fn, new Polyline());
            plots.Add(plt);
            _Draw(plt, PointBuilder.MinX, PointBuilder.MaxX);
        }
        private void _Draw(Plot p, double begin, double end) {

            if (ShowAxes) {
                DrawAxes();
                ShowAxes = false;
            }
            for (double i = begin; i < end; i = NextX(i)) {
                double y = -p.func.Invoke(i);
                if (double.IsNaN(y))
                    continue;
                p.curve.Points.Add(PointBuilder.GetPoint(i, y));
            }
            paper.Children.Add(p.curve);
        }
        private void DrawAxes() { 
            ox.Stroke = SystemColors.GrayTextBrush;
            oy.Stroke = SystemColors.GrayTextBrush;

            ox.X1 = -20;
            ox.X2 = paper.ActualWidth * 1.2;
            ox.Y1 = ox.Y2 = PointBuilder.Center.Y;

            oy.X1 = oy.X2 = PointBuilder.Center.X;
            oy.Y1 = -20;
            oy.Y2 = paper.ActualHeight * 1.2;

            paper.Children.Add(ox);
            paper.Children.Add(oy);
            DrawMeasure();
        }

        public void DrawMeasure() {
            double nextP = SplitStep();          

            for (double i = 0; i < PointBuilder.MaxX; i += 2*nextP) {
                Line l = new Line() {
                    X1 = PointBuilder.GetX(i),
                    X2 = PointBuilder.GetX(i),
                    Y1 = PointBuilder.Center.Y - 3,
                    Y2 = PointBuilder.Center.Y + 3,
                    Stroke = SystemColors.WindowFrameBrush
                };
                paper.Children.Add(l);

                TextBlock digits = new TextBlock();
                digits.Text = i.ToString();
                digits.FontSize = 9;
                digits.Foreground = SystemColors.WindowFrameBrush;
                Canvas.SetLeft(digits, l.X1 - 5);
                Canvas.SetTop(digits, l.Y1 - 10);
                paper.Children.Add(digits);
            }
            for (double i = 0; i > PointBuilder.MinX; i -= 2*nextP) {
                Line l = new Line() {
                    X1 = PointBuilder.GetX(i),
                    X2 = PointBuilder.GetX(i),
                    Y1 = PointBuilder.Center.Y - 3,
                    Y2 = PointBuilder.Center.Y + 3,
                    Stroke = SystemColors.WindowFrameBrush
                };
                paper.Children.Add(l);

                TextBlock digits = new TextBlock();
                digits.Text = i.ToString();
                digits.FontSize = 9;
                digits.Foreground = SystemColors.WindowFrameBrush;
                Canvas.SetLeft(digits, l.X1 - 5);
                Canvas.SetTop(digits, l.Y1 - 10);
                paper.Children.Add(digits);
            }
            for (double i = 0; i < PointBuilder.MaxY; i += 2 * nextP) {
                Line l = new Line() {
                    X1 = PointBuilder.Center.X - 3,
                    X2 = PointBuilder.Center.X + 3,
                    Y1 = PointBuilder.GetY(i),
                    Y2 = PointBuilder.GetY(i),
                    Stroke = SystemColors.WindowFrameBrush
                };
                paper.Children.Add(l);

                TextBlock digits = new TextBlock();
                digits.Text = i.ToString();
                digits.FontSize = 9;
                digits.Foreground = SystemColors.WindowFrameBrush;
                Canvas.SetLeft(digits, l.X1 + 8);
                Canvas.SetTop(digits, l.Y1 - 7);
                paper.Children.Add(digits);
            }
            for (double i = 0; i > PointBuilder.MinY; i -= 2 * nextP) {
                Line l = new Line() {
                    X1 = PointBuilder.Center.X - 3,
                    X2 = PointBuilder.Center.X + 3,
                    Y1 = PointBuilder.GetY(i),
                    Y2 = PointBuilder.GetY(i),
                    Stroke = SystemColors.WindowFrameBrush
                };
                paper.Children.Add(l);

                TextBlock digits = new TextBlock();
                digits.Text = i.ToString();
                digits.FontSize = 9;
                digits.Foreground = SystemColors.WindowFrameBrush;
                Canvas.SetLeft(digits, l.X1 + 8);
                Canvas.SetTop(digits, l.Y1 - 7);
                paper.Children.Add(digits);
            }
        }
        public double SplitStep() {
            double step = 60.0 / Scale;
            double old = Math.Abs(Math.Pow(10, -6) - step);
            for (int i = -5; i < 3; i++) {
                double distance = Math.Abs(Math.Pow(10, i) - step);
                if (old < distance) {
                    return Math.Pow(10, i - 1);
                }
                old = distance;
            }
            return 60.0 / Scale;
        }
        private double NextX(double x) {
            if (Scale > 2)
                return x + (1.0 / (Scale / 2));
            else if (Scale > 0.5)
                return x + Scale * 0.8;
            else return x + Scale * 1.5;
        }

        public void RedrawWithNewScale(double scale) {
            this.Scale = scale;
            RedrawPlot();
        }
        private void RedrawPlot() {
            paper.Children.Clear();
            plots.ForEach((Plot p) => { p.curve.Points.Clear(); });
            ShowAxes = true;
            foreach (var p in plots) {
                _Draw(p, PointBuilder.MinX, PointBuilder.MaxX);
            }
        }
        private void MoveX(double x) {
            PointBuilder.MoveCenter(x, 0);
            RedrawWithNewScale(Scale);
        }
        private void MoveY(double y) {
            PointBuilder.MoveCenter(0, y);
            RedrawWithNewScale(Scale);
        }
        public void MoveLeft() {
            if ((Scale / 4) > 5) {
                MoveX(-(Scale / 4));
            } else
                MoveX(-5);
        }
        public void MoveRight() {
            if ((Scale / 4) > 5) {
                MoveX(Scale / 4);
            } else
                MoveX(5);
        }
        public void MoveTop() {
            if ((Scale / 4) > 5) {
                MoveY(-(Scale / 4));
            } else
                MoveY(-5);
        }
        public void MoveBot() {
            if ((Scale / 4) > 5) {
                MoveY(Scale / 4);
            } else
                MoveY(5);
        }

    }
}
