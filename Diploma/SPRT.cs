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

        public static double LikehoodKnownK(List<double> input)
        {
            double result = 0;
            for (int i =0;i<input.Count;i++)
            {
                result += input[i];
            }
            return result;
        }
        public static double TopMarginKnownK(double theta0, double theta1, double k, double alpha, double beta, double n)
        {
            double result = 0;
            result = Math.Log((1 - beta) / alpha) + k * n * Math.Log(theta1) - k * n * Math.Log(theta0);
            result = result / ((1 / theta0) - (1 / theta1));
            return result;
        }
        public static double LowMarginKnownK(double theta0, double theta1, double k, double alpha, double beta, double n)
        {
            double result = 0;
            result = Math.Log(beta / (1 - alpha)) + k * n * Math.Log(theta1) - k * n * Math.Log(theta0);
            result = result / ((1 / theta0) - (1 / theta1));
            return result;
        }
        public static double LikehoodKnownTheta(List<double> input)
        {
            double result = 0;
            for (int i = 0; i < input.Count; i++)
            {
                result += Math.Log(input[i]);
            }
            return result;
        }
        //public static double TopMarginKnownTheta(double k0, double k1, double theta, double alpha, double beta, double n)
        //{
        //    double result = 0;
        //    result = ((1 - beta) / alpha) * Math.Pow(theta, k1) * Math.Pow(Accord.Math.Gamma.Function(k1), n) / (Math.Pow(theta, k0) * Math.Pow(Accord.Math.Gamma.Function(k0), n));
        //    result = Math.Log(result);
        //    result = result / (k1 - k0);
        //    return result;
        //}
        //public static double LowMarginKnownTheta(double k0, double k1, double theta, double alpha, double beta, double n)
        //{
        //    double result = 0;
        //    result = result = beta / (1-alpha) * Math.Pow(theta, k1) * Math.Pow(Accord.Math.Gamma.Function(k1), n) / (Math.Pow(theta, k0) / Math.Pow(Accord.Math.Gamma.Function(k0), n));
        //    result = Math.Log(result);
        //    result = result / (k1 - k0);
        //    return result;
        //}
        public static double TopMarginKnownTheta(double k0, double k1, double theta, double alpha, double beta, double n)
        {
            double result = 0;
            result = ((1 - beta) / alpha) * Math.Pow(theta, k1) * Math.Pow(Accord.Math.Gamma.Function(k1), n) / (Math.Pow(theta, k0) * Math.Pow(Accord.Math.Gamma.Function(k0), n));
            result = Math.Log(result);
            result = result / (k1 - k0);
            return result;
        }
        public static double LowMarginKnownTheta(double k0, double k1, double theta, double alpha, double beta, double n)
        {
            double result = 0;
            result = result = beta / (1 - alpha) * Math.Pow(theta, k1) * Math.Pow(Accord.Math.Gamma.Function(k1), n) / (Math.Pow(theta, k0) / Math.Pow(Accord.Math.Gamma.Function(k0), n));
            result = Math.Log(result);
            result = result / (k1 - k0);
            return result;
        }
    }
}
