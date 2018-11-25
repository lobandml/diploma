using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma
{
    class KnownKParser
    {
        public double k { get; set; }
        public double theta1 { get; set; }
        public double theta2 { get; set; }
        public double alpha { get; set; }
        public double beta { get; set; }

        public void Parse(MainWindow mainWnd)
        {
            k = mainWnd.k_kInputTextBox.Text.GetNeutralParsed();
            theta1 = mainWnd.k_theta1InputTextBox.Text.GetNeutralParsed();
            theta2 = mainWnd.k_theta2InputTextBox.Text.GetNeutralParsed();
            alpha = mainWnd.k_alphaInputTextBox.Text.GetNeutralParsed();
            beta = mainWnd.k_betaInputTextBox.Text.GetNeutralParsed();
        }
    }
    class KnownThetaParser
    {
        public double k1 { get; set; }
        public double k2 { get; set; }
        public double theta { get; set; }
        public double alpha { get; set; }
        public double beta { get; set; }

        public void Parse(MainWindow mainWnd)
        {
            k1 = mainWnd.theta_k1InputTextBox.Text.GetNeutralParsed();
            k2 = mainWnd.theta_k2InputTextBox.Text.GetNeutralParsed();
            theta = mainWnd.theta_thetaInputTextBox.Text.GetNeutralParsed();
            alpha = mainWnd.theta_alphaInputTextBox.Text.GetNeutralParsed();
            beta = mainWnd.theta_betaInputTextBox.Text.GetNeutralParsed();
        }
    }
    class BothUnknownParser
    {
        public double k1 { get; set; }
        public double k2 { get; set; }
        public double theta1 { get; set; }
        public double theta2 { get; set; }
        public double alpha { get; set; }
        public double beta { get; set; }

        public void Parse(MainWindow mainWnd)
        {
            k1 = mainWnd.both_k1InputTextBox.Text.GetNeutralParsed();
            k2 = mainWnd.both_k2InputTextBox.Text.GetNeutralParsed();
            theta1 = mainWnd.both_theta1InputTextBox.Text.GetNeutralParsed();
            theta2 = mainWnd.both_theta2InputTextBox.Text.GetNeutralParsed();
            alpha = mainWnd.both_alphaInputTextBox.Text.GetNeutralParsed();
            beta = mainWnd.both_betaInputTextBox.Text.GetNeutralParsed();
        }
    }
}
