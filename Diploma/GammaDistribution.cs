using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma
{
    class GammaDistribution
    {
        public static double calcDens(double k, double theta, double input)
        {
            double result = 0;
            if (input < 0) return 0;
            result = Math.Pow(input, k - 1);
            result *= Math.Pow(Math.E, -input / theta);
            result /= (Math.Pow(theta, k) * Accord.Math.Gamma.Function(k));
            return result;
        }
    }
}
