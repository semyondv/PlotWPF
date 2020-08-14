using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio.Dsp;

namespace Plot {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
        }
        PlotBuilder plotBuilder = null;
        Canvas canvas = null;
        private void Canvas_Loaded(object sender, RoutedEventArgs e) {
            canvas = sender as Canvas;
            plotBuilder = new PlotBuilder(canvas, 20);
            plotBuilder.DrawPlot((x) => Math.Sqrt(x));
            plotBuilder.DrawPlot((x) => x * x * x);
            //FuncProcessor processor = new FuncProcessor();

        }
        private void Window_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Down) {
                plotBuilder.MoveTop();
            } else if (e.Key == Key.Up) {
                plotBuilder.MoveBot();
            } else if (e.Key == Key.Left) {
                plotBuilder.MoveRight();
            } else if (e.Key == Key.Right) {
                plotBuilder.MoveLeft();
            }
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e) {
            double newScale = e.Delta > 0 ? plotBuilder.Scale * 1.05 : plotBuilder.Scale * 0.95;
            //this.Title = plotBuilder.PointBuilder.MinX.ToString() + "___" + plotBuilder.PointBuilder.MaxX.ToString();
            this.Title = plotBuilder.Scale.ToString();
            //if (newScale > 10)
            plotBuilder.RedrawWithNewScale(newScale);
        }
        private void Window_MouseMove(object sender, MouseEventArgs e) {
            //Point pos;
            if (e.LeftButton == MouseButtonState.Pressed) {
                double newX = -plotBuilder.PointBuilder.Center.X + e.GetPosition(this).X,
                    newY = -plotBuilder.PointBuilder.Center.Y + e.GetPosition(this).Y;
                plotBuilder.PointBuilder.MoveCenter(newX, newY);
                plotBuilder.RedrawWithNewScale(plotBuilder.Scale);
            }
        }
    }
}
