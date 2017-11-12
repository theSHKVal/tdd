using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using TagCloudLibrary;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace TagCloudVisualisation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public ObservableCollection<RectangleItem> Cloud { get; set; }
        private ITagCloudLayouter cloudLayouter;

        public MainWindow()
        {
            Cloud = new ObservableCollection<RectangleItem>();          
            InitializeComponent();
        }

        private void NewCloudButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cloud.Clear();
                var center = new Point(int.Parse(CenterXTextBox.Text), int.Parse(CenterYTextBox.Text));
                cloudLayouter = new CircularCloudLayouter(center);
                Cloud.Add(new RectangleItem
                {
                    Width = 2,
                    Height = 2,
                    X = center.X,
                    Y = center.Y
                });
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (cloudLayouter == null)
            {
                MessageBox.Show("Create cloud first", "Error");
                return;
            }

            try
            {
                var nextRectangle = cloudLayouter.PutNextRectangle(new Size(int.Parse(WidthTextBox.Text), int.Parse(HeightTextBox.Text)));
                Cloud.Add(new RectangleItem
                {
                    Height = nextRectangle.Height,
                    Width = nextRectangle.Width,
                    X = nextRectangle.X,
                    Y = nextRectangle.Y
                });
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
        }

        private void ExampleButton_Click(object sender, RoutedEventArgs e)
        {
            var sizes = new List<Size>
            {
                new Size(100, 70), //first
                new Size(60, 30), new Size(70, 40), new Size(80, 40), new Size(65, 30), new Size(85, 25), //top, right
                new Size(60, 40), new Size(80, 30), new Size(75, 25), new Size(80, 30), new Size(90, 50), //bottom, left
                new Size(100, 30), new Size(80, 20), new Size(50, 35),  new Size(60, 20), //top
                new Size(50, 15), new Size(80, 30), new Size(70, 20), new Size(90, 25), new Size(60, 60), new Size(70, 30), //right
                new Size(120, 35), new Size(90, 40), new Size(80, 20), //bottom
                new Size(70, 30), new Size(90, 30), new Size(120, 40), new Size(100, 50), new Size(70, 20) //left
            };
            cloudLayouter = new CircularCloudLayouter(new Point(400, 200));

            foreach (var size in sizes)
            {
                var  rectangle = cloudLayouter.PutNextRectangle(size);
                Cloud.Add(new RectangleItem
                {
                    Height = rectangle.Height,
                    Width = rectangle.Width,
                    X = rectangle.X,
                    Y = rectangle.Y
                });
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Cloud.Clear();
        }   
    }

    public class RectangleItem
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
