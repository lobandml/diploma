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

namespace Diploma
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        DistributionInstance currentDis;
        public MainWindow()
		{
			InitializeComponent();
			Accord.Statistics.Distributions.Univariate.GammaDistribution gd1 = new Accord.Statistics.Distributions.Univariate.GammaDistribution(1,1);
			double dd1 = gd1.Generate();
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
            double alpha = 0.1;
            double beta = 0.01;
            double c0 = beta / (1 - alpha);
            c0 = Math.Log(c0);
            double c1 = (1 - beta) / alpha;
            c1 = Math.Log(c1);
            for (int i = 0; i < currentDis.arr.Count; i++)
            {
                List<double> checkedList = currentDis.arr.GetRange(0, i + 1);
                double checkedForThe0 = SPRT.GammaLikelihood(checkedList, currentDis.DParams["k"] + 0.4, currentDis.DParams["theta"]);
                double checkedForThe1 = SPRT.GammaLikelihood(checkedList, currentDis.DParams["k"], currentDis.DParams["theta"]);
                double likehoodDivision = checkedForThe1 / checkedForThe0;
                likehoodDivision = Math.Log(likehoodDivision);
                if (likehoodDivision < c0)
                {
                    MessageBox.Show("h0" + i.ToString());
                    break;
                }

                if (likehoodDivision > c1)
                {
                    MessageBox.Show("h1" + i.ToString());
                    break;
                }
            }
        }
    }
}
