using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Diploma
{
    class DistributionInstance
    {
        public Dictionary<string,double> DParams;
        public List<double> arr;
        public DistributionInstance(double[] inputarr)
        {
            DParams = new Dictionary<string, double>();
            arr = inputarr.ToList();
        }
        public DistributionInstance()
        {
            DParams = new Dictionary<string, double>();
            arr = new List<double>();
        }
        public XmlDocument ConvertToXml()
        {
            XmlDocument doc = new XmlDocument();
            var root = doc.CreateElement("Instance");
            doc.AppendChild(root);
            var list = DParams.ToList();
            for (int i=0; i< list.Count;i++)
            {
                var cel = doc.CreateElement("Param");
                cel.SetAttribute("name", list[i].Key);
                cel.InnerText = list[i].Value.ToString(CultureInfo.InvariantCulture);
                root.AppendChild(cel);
            }
            for (int i = 0; i < arr.Count; i++)
            {
                var cel = doc.CreateElement("Item");
                cel.InnerText = arr[i].ToString(CultureInfo.InvariantCulture);
                root.AppendChild(cel);
            }
            return doc;
        }
        public void InitXml(string doc)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(doc);
            InitXml(xdoc);
        }
        public void InitXml(XmlDocument doc)
        {
            var instance = doc.SelectSingleNode("Instance");
            var SParams = instance.SelectNodes("Param");
            var SItems = instance.SelectNodes("Item");
            DParams.Clear();
            arr.Clear();
            for (int i=0;i<SParams.Count;i++)
            {
                var parElement = (XmlElement)SParams[i];
                DParams.Add(parElement.GetAttribute("name"), Double.Parse(parElement.InnerText, CultureInfo.InvariantCulture));
            }
            for (int i = 0; i < SItems.Count; i++)
            {
                var itemElement = (XmlElement)SItems[i];
                arr.Add(Double.Parse(itemElement.InnerText, CultureInfo.InvariantCulture));
            }
        }
    }
}
