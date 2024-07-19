using DrawingWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace DrawingWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int sizeInt = 3;
        List<ColorBrush> colorBrushes = new List<ColorBrush>()
        {
            new ColorBrush()
            {
                Name = "Красный",
                Brush = Brushes.Red
            },
            new ColorBrush()
            {
                Name = "Зеленый",
                Brush = Brushes.Green
            },
            new ColorBrush()
            {
                Name = "Желтый",
                Brush = Brushes.Yellow
            },
            new ColorBrush()
            {
                Name = "Синий",
                Brush = Brushes.Blue
            },
            new ColorBrush()
            {
                Name = "Белый",
                Brush = Brushes.White
            },
            new ColorBrush()
            {
                Name = "Черный",
                Brush = Brushes.Black
            },
        };

        public MainWindow()
        {
            InitializeComponent();
            TextSize.Text = "3";

            ComboColors.ItemsSource = colorBrushes;
            ComboColors.SelectedIndex = 0;
        }

        bool isDrawing = false;
        PathFigure currentFigure;
        private void DrawingTarget_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(DrawingTarget);
            isDrawing = true;
            StartFigure(e.GetPosition(DrawingTarget));
        }

        private void StartFigure(Point point)
        {
            var color = (ComboColors.SelectedItem as ColorBrush).Brush;
            currentFigure = new PathFigure() { StartPoint = point };
            var currentPath =
                new Path()
                {
                    Stroke = color,
                    StrokeThickness = sizeInt,
                    Data = new PathGeometry() { Figures = { currentFigure } }
                };
            DrawingTarget.Children.Add(currentPath);
        }

        private void DrawingTarget_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AddFigurePoint(e.GetPosition(DrawingTarget));
            currentFigure = null;
            isDrawing = false;
            Mouse.Capture(null);
        }

        private void AddFigurePoint(Point point)
        {
            if (currentFigure != null)
                currentFigure.Segments.Add(new LineSegment(point, isStroked: true));
        }

        private void DrawingTarget_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing)
                return;
            AddFigurePoint(e.GetPosition(DrawingTarget));
        }

        private void TextSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Regex.IsMatch(TextSize.Text, "^[0-9]+$"))
            {
                TextSize.Background = Brushes.Red;
                return;
            }
            TextSize.Background = Brushes.White;
            try
            {
                sizeInt = Int32.Parse(TextSize.Text);
            }
            catch
            {
                TextSize.Background = Brushes.Red;
            }
        }
    }
}
