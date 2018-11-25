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
            Bayes_info.Text = Bayes_info.Text.Replace("\\r\\n", "\r\n");
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
        private void TestKnownBoth_UI(BothUnknownParser data)
        {
            List<ResultsTableRow> resList = new List<ResultsTableRow>();
            double alpha = data.alpha;
            double beta = data.beta;
            double c0;
            double c1;

            bool falseIsTop = true;

            while (SCollection.Count > 0)
            {
                SCollection.RemoveAt(SCollection.Count - 1);
            }
            var topPointsLine = AddNewSeriesCollection("брак.значення", Brushes.Red);
            var botPointsLine = AddNewSeriesCollection("пр-е значення", Brushes.Blue);
            var checkingsPointsLine = AddNewSeriesCollection("спостереження", Brushes.Purple);
            DataContext = this;

            for (int i = 0; i < currentDis.arr.Count; i++)
            {
                ResultsTableRow resListInstance = new ResultsTableRow();
                List<double> checkedList = currentDis.arr.GetRange(0, i + 1);
                double likehoodDivision = SPRT.GammaLikelihood(checkedList, data.k1,data.theta1);
                likehoodDivision /= SPRT.GammaLikelihood(checkedList, data.k2, data.theta2);
                c0 = beta / (1 - alpha);
                c1 = (1 - beta) / alpha;
                if (c1 < c0) falseIsTop = false;
                resListInstance.Num = i.ToString();
                resListInstance.falseMargin = falseIsTop ? c1.GetRoundedString(4) : c0.GetRoundedString(4);
                resListInstance.TrueMargin = falseIsTop ? c0.GetRoundedString(4) : c1.GetRoundedString(4);
                resListInstance.Sum = likehoodDivision.GetRoundedString(4);
                resListInstance.Value = currentDis.arr[i].GetRoundedString(4);
                resList.Add(resListInstance);

                topPointsLine.Values.Add(new System.Windows.Point(i, Math.Round(c1, 3)));
                botPointsLine.Values.Add(new System.Windows.Point(i, Math.Round(c0, 3)));
                checkingsPointsLine.Values.Add(new System.Windows.Point(i, Math.Round(likehoodDivision, 3)));

                var checkC0 = falseIsTop ? c0 : c1;
                var checkC1 = falseIsTop ? c1 : c0;
                if (likehoodDivision < checkC0)
                {
                    string insert = falseIsTop ? "H0" : "H1";
                    MessageBox.Show("Гіпотеза " + insert + "\r\n на кроці " + (i + 1).ToString());
                    break;
                }
                if (likehoodDivision > checkC1)
                {
                    string insert = falseIsTop ? "H1" : "H0";
                    MessageBox.Show("Гіпотеза " + insert + "\r\n на кроці " + (i + 1).ToString());
                    break;
                }
            }
            ResultsTable.ItemsSource = resList;
        }
        private void TestKnownTheta_IU(KnownThetaParser data)
        {
            List<ResultsTableRow> resList = new List<ResultsTableRow>();
            double alpha = data.alpha;
            double beta = data.beta;
            double c0;
            double c1;

            bool falseIsTop = true;

            while (SCollection.Count > 0)
            {
                SCollection.RemoveAt(SCollection.Count - 1);
            }
            var topPointsLine = AddNewSeriesCollection("брак.значення", Brushes.Red);
            var botPointsLine = AddNewSeriesCollection("пр-е значення", Brushes.Blue);
            var checkingsPointsLine = AddNewSeriesCollection("спостереження", Brushes.Purple);
            DataContext = this;

            for (int i = 0; i < currentDis.arr.Count; i++)
            {
                ResultsTableRow resListInstance = new ResultsTableRow();
                List<double> checkedList = currentDis.arr.GetRange(0, i + 1);
                double likehoodDivision = SPRT.LikehoodKnownTheta(checkedList);

                c0 = SPRT.LowMarginKnownTheta(data.k1, data.k2, data.theta, alpha, beta, i + 1);
                c1 = SPRT.TopMarginKnownTheta(data.k1, data.k2, data.theta, alpha, beta, i + 1);
                if (c1 < c0) falseIsTop = false;
                resListInstance.Num = i.ToString();
                resListInstance.falseMargin = falseIsTop ? c1.GetRoundedString(4) : c0.GetRoundedString(4);
                resListInstance.TrueMargin = falseIsTop ? c0.GetRoundedString(4) : c1.GetRoundedString(4);
                resListInstance.Sum = likehoodDivision.GetRoundedString(4);
                resListInstance.Value = currentDis.arr[i].GetRoundedString(4);
                resList.Add(resListInstance);

                topPointsLine.Values.Add(new System.Windows.Point(i, Math.Round(c1,3)));
                botPointsLine.Values.Add(new System.Windows.Point(i, Math.Round(c0, 3)));
                checkingsPointsLine.Values.Add(new System.Windows.Point(i, Math.Round(likehoodDivision, 3)));

                var checkC0 = falseIsTop ? c0 : c1;
                var checkC1 = falseIsTop ? c1 : c0;
                if (likehoodDivision < checkC0)
                {
                    string insert = falseIsTop ? "H0" : "H1";
                    MessageBox.Show("Гіпотеза " + insert + "\r\n на кроці " + (i + 1).ToString());
                    break;
                }
                if (likehoodDivision > checkC1)
                {
                    string insert = falseIsTop ? "H1" : "H0";
                    MessageBox.Show("Гіпотеза " + insert + "\r\n на кроці " + (i + 1).ToString());
                    break;
                }
            }
            ResultsTable.ItemsSource = resList;
        }
        private void TestKnownK_IU(KnownKParser data)
        {
            List<ResultsTableRow> resList = new List<ResultsTableRow>();
            
            double alpha = data.alpha;
            double beta = data.beta;
            double c0;
            double c1;

            bool falseIsTop = true;

            while (SCollection.Count > 0)
            {
                SCollection.RemoveAt(SCollection.Count - 1);
            }
            var topPointsLine = AddNewSeriesCollection("брак.значення", Brushes.Red);
            var botPointsLine = AddNewSeriesCollection("пр-е значення", Brushes.Blue);
            var checkingsPointsLine = AddNewSeriesCollection("спостереження", Brushes.Purple);
            DataContext = this;

            for (int i = 0; i < currentDis.arr.Count; i++)
            {
                ResultsTableRow resListInstance = new ResultsTableRow();
                List<double> checkedList = currentDis.arr.GetRange(0, i + 1);
                double likehoodDivision = SPRT.LikehoodKnownK(checkedList);
                c0 = SPRT.LowMarginKnownK(data.theta1, data.theta2, data.k, alpha, beta, i + 1);
                c1 = SPRT.TopMarginKnownK(data.theta1, data.theta2, data.k, alpha, beta, i + 1);
                if (c1 < c0) falseIsTop = false;
                resListInstance.Num = i.ToString();
                resListInstance.falseMargin = falseIsTop ? c1.GetRoundedString(4) : c0.GetRoundedString(4);
                resListInstance.TrueMargin = falseIsTop ? c0.GetRoundedString(4) : c1.GetRoundedString(4);
                resListInstance.Sum = likehoodDivision.GetRoundedString(4);
                resListInstance.Value = currentDis.arr[i].GetRoundedString(4);
                resList.Add(resListInstance);
                

                topPointsLine.Values.Add(new System.Windows.Point(i, Math.Round(c1, 3)));
                botPointsLine.Values.Add(new System.Windows.Point(i, Math.Round(c0, 3)));
                checkingsPointsLine.Values.Add(new System.Windows.Point(i, Math.Round(likehoodDivision, 3)));

                var checkC0 = falseIsTop ? c0 : c1;
                var checkC1 = falseIsTop ? c1 : c0;
                if (likehoodDivision < checkC0)
                {
                    string insert = falseIsTop ? "H0" : "H1";
                    MessageBox.Show("Гіпотеза " + insert + "\r\n на кроці " + (i + 1).ToString());
                    break;
                }
                if (likehoodDivision > checkC1)
                {
                    string insert = falseIsTop ? "H1" : "H0";
                    MessageBox.Show("Гіпотеза " + insert + "\r\n на кроці " + (i + 1).ToString());
                    break;
                }
            }
            ResultsTable.ItemsSource = resList;
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


        private void k_DoNTimes(object sender, RoutedEventArgs e)
        {

        }

        private void theta_DoNTimes(object sender, RoutedEventArgs e)
        {

        }

        private void theta_DoMain(object sender, RoutedEventArgs e)
        {
            if (currentDis == null)
            {
                MessageBox.Show("Завантажте файл");
                return;
            }
            KnownThetaParser data = new KnownThetaParser();
            data.Parse(this);
            TestKnownTheta_IU(data);
        }

        private void k_DoMain(object sender, RoutedEventArgs e)
        {
            if (currentDis == null)
            {
                MessageBox.Show("Завантажте файл");
                return;
            }
            KnownKParser data = new KnownKParser();
            data.Parse(this);
            TestKnownK_IU(data);
        }


        private void both_DoMain(object sender, RoutedEventArgs e)
        {
            if (currentDis == null)
            {
                MessageBox.Show("Завантажте файл");
                return;
            }
            BothUnknownParser data = new BothUnknownParser();
            data.Parse(this);
            TestKnownBoth_UI(data);
        }

        private void both_DoNTimes(object sender, RoutedEventArgs e)
        {

        }

        private void ShowGDInfo(object sender, RoutedEventArgs e)
        {
            if (currentDis == null)
            {
                MessageBox.Show("Завантажте файл");
                return;
            }
            string results = string.Empty;
            results += "Об`єм вибірки: " + currentDis.arr.Count.ToString();
            if(currentDis.DParams.ContainsKey("k")) results += "\r\n k = " + currentDis.DParams["k"].ToString();
            if (currentDis.DParams.ContainsKey("theta")) results += "\r\n theta = " + currentDis.DParams["k"].ToString();
            MessageBox.Show(results);
        }

        private void OpenLastChart_both(object sender, RoutedEventArgs e)
        {
            if (currentDis == null)
            {
                MessageBox.Show("Завантажте файл");
                return;
            }
            BothUnknownParser data = new BothUnknownParser();
            data.Parse(this);
            List<System.Windows.Point> lp1 = new List<System.Windows.Point>();
            List<System.Windows.Point> lp2 = new List<System.Windows.Point>();
            for (double i = 0; i < 20; i += 0.1)
            {
                lp1.Add(new System.Windows.Point(i, GammaDistribution.calcDens(data.k1, data.theta1, i)));
                lp2.Add(new System.Windows.Point(i, GammaDistribution.calcDens(data.k2, data.theta2, i)));
            }
            DensComparer dc = new DensComparer("H0", lp1, "H1", lp2);
            dc.Show();
        }

        private void OpenLastChart_theta(object sender, RoutedEventArgs e)
        {
            if (currentDis == null)
            {
                MessageBox.Show("Завантажте файл");
                return;
            }
            KnownThetaParser data = new KnownThetaParser();
            data.Parse(this);
            List<System.Windows.Point> lp1 = new List<System.Windows.Point>();
            List<System.Windows.Point> lp2 = new List<System.Windows.Point>();
            for (double i = 0; i < 20; i += 0.1)
            {
                lp1.Add(new System.Windows.Point(i, GammaDistribution.calcDens(data.k1, data.theta, i)));
                lp2.Add(new System.Windows.Point(i, GammaDistribution.calcDens(data.k2, data.theta, i)));
            }
            DensComparer dc = new DensComparer("H0", lp1, "H1", lp2);
            dc.Show();

        }
        private void OpenLastChart_k(object sender, RoutedEventArgs e)
        {
            if (currentDis == null)
            {
                MessageBox.Show("Завантажте файл");
                return;
            }
            KnownKParser data = new KnownKParser();
            data.Parse(this);
            List<System.Windows.Point> lp1 = new List<System.Windows.Point>();
            List<System.Windows.Point> lp2 = new List<System.Windows.Point>();
            for (double i = 0; i < 20; i += 0.1)
            {
                lp1.Add(new System.Windows.Point(i, GammaDistribution.calcDens(data.k, data.theta1, i)));
                lp2.Add(new System.Windows.Point(i, GammaDistribution.calcDens(data.k, data.theta2, i)));
            }
            DensComparer dc = new DensComparer("H0", lp1, "H1", lp2);
            dc.Show();
        }
    }
}
