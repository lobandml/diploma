using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma
{
    static class Extentions
    {
        public static double GetNeutralParsed(this string a)
        {
            return double.Parse(a, System.Globalization.CultureInfo.InvariantCulture);
        }
        public static string GetRoundedString(this double d, int digits)
        {
            d = Math.Round(d, digits);
            return d.ToString();
        }
    }
}
