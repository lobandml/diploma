using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
	/// Interaction logic for GammaGeneration.xaml
	/// </summary>
	public partial class GammaGeneration : Window
	{
		double[] currGammaArray;
        DistributionInstance currentDis;
		public GammaGeneration()
		{
			InitializeComponent();
		}

		private void Ribbon_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			double theta_param = double.Parse(param1tb.Text);
			double k = double.Parse(param2tb.Text);
			int count = int.Parse(counttb.Text);
			Accord.Statistics.Distributions.Univariate.GammaDistribution gd1 = new Accord.Statistics.Distributions.Univariate.GammaDistribution(theta_param, k);
			double[] dd1 = gd1.Generate(count);
			currGammaArray = dd1;
			UpdateList(currentDis);
            DistributionInstance dis = new DistributionInstance(currGammaArray);
            dis.DParams.Add("theta", theta_param);
            dis.DParams.Add("k", k);
            currentDis = dis;
        }
		private void UpdateList(DistributionInstance input)
		{
			listBox.Items.Clear();
			for (int i=0;i<input.arr.Count;i++)
			{
				listBox.Items.Add(input.arr[i]);
			}
		}
        private void SaveGenerated()
        {  
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML Document|*.xml";
            saveFileDialog1.Title = "Save an XML Document";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                currentDis.ConvertToXml().Save(saveFileDialog1.FileName);
            }
        }
        private void OpenGenerated()
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
            UpdateList(currentDis);
        }

        private void SaveGenButtonHandler(object sender, RoutedEventArgs e)
        {
            SaveGenerated();
        }

        private void OpenGenButtonHandler(object sender, RoutedEventArgs e)
        {
            OpenGenerated();
        }

        private void graphButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
