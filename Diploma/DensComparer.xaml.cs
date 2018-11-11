using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
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
using System.Windows.Shapes;

namespace Diploma
{
    /// <summary>
    /// Interaction logic for DensComparer.xaml
    /// </summary>
    public partial class DensComparer : Window
    {
        public SeriesCollection SCollection { get; set; }
        public DensComparer(string firstname, List<Point> firstdens, string secondname, List<Point> seconddens)
        {
            InitializeComponent();
            var mapper = Mappers.Xy<System.Windows.Point>().X(v => v.X).Y(v => v.Y);
            SCollection = new SeriesCollection(mapper);
            var firstPointsLine = AddNewSeriesCollection(firstname, Brushes.Red);
            var secondPointsLine = AddNewSeriesCollection(secondname, Brushes.Blue);
            for (int i=0;i<firstdens.Count;i++)
            {
                firstPointsLine.Values.Add(firstdens[i]);
            }
            for (int i = 0; i < seconddens.Count; i++)
            {
                secondPointsLine.Values.Add(seconddens[i]);
            }
            DataContext = this;
        }
        public LineSeries AddNewSeriesCollection(string title, System.Windows.Media.Brush color)
        {
            var nls = new LineSeries
            {
                Title = title,
                //Values = new LiveCharts.ChartValues<float>(values),
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                PointGeometrySize = 5,
                PointForeground = color,
                Stroke = color
            };
            nls.Values = new ChartValues<System.Windows.Point>();
            SCollection.Add(nls);
            return nls;
        }
    }
}
