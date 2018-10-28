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
using Accord;
using Microsoft.Win32;
using System.IO;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;

namespace Diploma
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        DistributionInstance currentDis;
        // sequental, для посл. анализа
        public SeriesCollection SCollection { get; set; }
        public MainWindow()
		{
			InitializeComponent();
			//Accord.Statistics.Distributions.Univariate.GammaDistribution gd1 = new Accord.Statistics.Distributions.Univariate.GammaDistribution(1,1);
			//double dd1 = gd1.Generate();
            var mapper = Mappers.Xy<System.Windows.Point>().X(v => v.X).Y(v => v.Y);
            SCollection = new SeriesCollection(mapper);
        }

        private void OpenGeneratorHandler(object sender, RoutedEventArgs e)
        {
            var gg = new GammaGeneration();
            gg.Show();
        }


        private void LoadGDFromXmlHadler(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd1 = new OpenFileDialog();
            ofd1.Filter = "XML Document|*.xml";
            ofd1.Title = "Open an XML Document";
            ofd1.ShowDialog();
            if (ofd1.FileName != "")
            {
                currentDis = new DistributionInstance();
                string str = File.ReadAllText(ofd1.FileName);
                currentDis.InitXml(str);
            }
            // test1();
            //test2theta();
        }

        private void test2theta()
        {
            double alpha = 0.1;
            double beta = 0.01;
            double c0;
            double c1; 


            while (SCollection.Count > 0)
            {
                SCollection.RemoveAt(SCollection.Count - 1);
            }

            var topPointsLine = AddNewSeriesCollection("Top", Brushes.Red);
            var botPointsLine = AddNewSeriesCollection("Bot", Brushes.Blue);
            var checkingsPointsLine = AddNewSeriesCollection("values", Brushes.Aqua);
            DataContext = this;

            for (int i = 0; i < currentDis.arr.Count; i++)
            {
                List<double> checkedList = currentDis.arr.GetRange(0, i + 1);
                double likehoodDivision = SPRT.LikehoodKnownTheta(checkedList);
                //c0 = SPRT.LowMarginKnownTheta(currentDis.DParams["k"] + 0.9, currentDis.DParams["k"], currentDis.DParams["theta"], alpha, beta, i + 1);
                //c1 = SPRT.TopMarginKnownTheta(currentDis.DParams["k"] + 0.9, currentDis.DParams["k"], currentDis.DParams["theta"], alpha, beta, i + 1);
                c0 = SPRT.LowMarginKnownTheta(double.Parse(textBox.Text, System.Globalization.CultureInfo.InvariantCulture), double.Parse(textBox_Copy.Text, System.Globalization.CultureInfo.InvariantCulture), currentDis.DParams["theta"], alpha, beta, i + 1);
                c1 = SPRT.TopMarginKnownTheta(double.Parse(textBox.Text, System.Globalization.CultureInfo.InvariantCulture), double.Parse(textBox_Copy.Text, System.Globalization.CultureInfo.InvariantCulture), currentDis.DParams["theta"], alpha, beta, i + 1);

                topPointsLine.Values.Add(new System.Windows.Point(i, Math.Round(c1,3)));
                botPointsLine.Values.Add(new System.Windows.Point(i, Math.Round(c0, 3)));
                checkingsPointsLine.Values.Add(new System.Windows.Point(i, Math.Round(likehoodDivision, 3)));

                if (likehoodDivision < c0)
                {
                    //MessageBox.Show("h0 - " + i.ToString());
                    //break;
                }

                if (likehoodDivision > c1)
                {
                    //MessageBox.Show("h1 - " + i.ToString());
                    //break;
                }
            }
        }
        private void test2K()
        {
            double alpha = 0.1;
            double beta = 0.05;
            double c0;
            double c1;


            while (SCollection.Count > 0)
            {
                SCollection.RemoveAt(SCollection.Count - 1);
                
            }

            var topPointsLine = AddNewSeriesCollection("брак.значение", Brushes.Red);
            var botPointsLine = AddNewSeriesCollection("пр-е значение", Brushes.Blue);
            var checkingsPointsLine = AddNewSeriesCollection("наблюдения", Brushes.Purple);
            DataContext = this;

            for (int i = 0; i < currentDis.arr.Count; i++)
            {
                List<double> checkedList = currentDis.arr.GetRange(0, i + 1);
                double likehoodDivision = SPRT.LikehoodKnownK(checkedList);
                //c0 = SPRT.LowMarginKnownTheta(currentDis.DParams["k"] + 0.9, currentDis.DParams["k"], currentDis.DParams["theta"], alpha, beta, i + 1);
                //c1 = SPRT.TopMarginKnownTheta(currentDis.DParams["k"] + 0.9, currentDis.DParams["k"], currentDis.DParams["theta"], alpha, beta, i + 1);
                c0 = SPRT.LowMarginKnownK(double.Parse(textBox.Text, System.Globalization.CultureInfo.InvariantCulture), double.Parse(textBox_Copy.Text, System.Globalization.CultureInfo.InvariantCulture), currentDis.DParams["k"], alpha, beta, i + 1);
                c1 = SPRT.TopMarginKnownK(double.Parse(textBox.Text, System.Globalization.CultureInfo.InvariantCulture), double.Parse(textBox_Copy.Text, System.Globalization.CultureInfo.InvariantCulture), currentDis.DParams["k"], alpha, beta, i + 1);
                //if (c0 > c1)
               // {
                 //   var tt = c0;
                //    c0 = c1;
               //     c1 = tt;
                //}

                topPointsLine.Values.Add(new System.Windows.Point(i, Math.Round(c1, 3)));
                botPointsLine.Values.Add(new System.Windows.Point(i, Math.Round(c0, 3)));
                checkingsPointsLine.Values.Add(new System.Windows.Point(i, Math.Round(likehoodDivision, 3)));

                if (likehoodDivision < c0)
                {
                    MessageBox.Show("h0 - " + i.ToString());
                    break;
                }

                if (likehoodDivision > c1)
                {
                    MessageBox.Show("h1 - " + i.ToString());
                    break;
                }
            }
        }

        private void test1()
        {
            double alpha = 0.1;
            double beta = 0.01;
            double c0 = beta / (1 - alpha);
            c0 = Math.Log(c0);
            double c1 = (1 - beta) / alpha;
            c1 = Math.Log(c1);


            for (int i = 0; i < currentDis.arr.Count; i++)
            {
                List<double> checkedList = currentDis.arr.GetRange(0, i + 1);
                double checkedForThe0 = SPRT.GammaLikelihood(checkedList, currentDis.DParams["k"]+0.9, currentDis.DParams["theta"]);
                double checkedForThe1 = SPRT.GammaLikelihood(checkedList, currentDis.DParams["k"], currentDis.DParams["theta"]);
                double likehoodDivision = checkedForThe1 / checkedForThe0;
                likehoodDivision = Math.Log(likehoodDivision);
                if (likehoodDivision < c0)
                {
                    MessageBox.Show("h0 - " + i.ToString());
                    break;
                }

                if (likehoodDivision > c1)
                {
                    MessageBox.Show("h1 - " + i.ToString());
                    break;
                }
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //test1();
            if (currentDis == null)
            {
                MessageBox.Show("Файл подгрузи, а?");
                return;
            }
            test2theta();
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

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void checkK_Click(object sender, RoutedEventArgs e)
        {
            test2K();
        }
    }
}
