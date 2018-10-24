using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma
{
    static class SPRT
    {
        public static double GammaLikelihood(List<double> input, double k, double theta)
        {
            double result = 1;
            for (int i=0; i<input.Count;i++)
            {
                result *= GammaDistribution.calcDens(k, theta, input[i]);
            }
            return result;
        }
    }
}
