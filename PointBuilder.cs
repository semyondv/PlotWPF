using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Plot {
    class PointBuilder {
        Canvas canvas = null;
        public Point Center { get; set; }
        public Point OrigCenter { get; set; }
        public double Mul { get; set; }
        public PointBuilder(Canvas canvas) :
            this(canvas, 40) {
        }
        public PointBuilder(Canvas canvas, int mul) {
            this.canvas = canvas;
            this.Mul = mul;

            Center = new Point(canvas.Width / 2, canvas.Height / 2);
            OrigCenter = Center;
        }
        public Point GetPoint(double x, double y) => new Point(GetX(x), GetY(y));
        public double GetX(double x) => Center.X + x * Mul;//x * Mul + canvas.Width / 2;
        public double GetY(double y) => Center.Y + y * Mul;//y * Mul + canvas.Height / 2;
        public double MaxX => (canvas.ActualWidth - Center.X) * 1.1 / Mul;
        public double MinX => -Center.X * 1.1 / Mul;
        public double MaxY => (canvas.ActualHeight - Center.Y) * 1.1 / Mul;
        public double MinY => -Center.Y * 1.1 / Mul;
        public void MoveCenter(double x, double y) {
            Center = new Point(Center.X + x, Center.Y + y);
        }

    }
}
