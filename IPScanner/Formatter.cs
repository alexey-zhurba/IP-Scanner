using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPScanner
{
    class Formatter
    {
        public static IPEndPoint Parse(string ipEndPoint)
        {
            string[] temp = new string[2];
            if (!ipEndPoint.Contains(':'))
            {
                temp[0] = ipEndPoint;
                temp[1] = "80";
            }
            else
                temp = ipEndPoint.Split(':');

            return new IPEndPoint(IPAddress.Parse(temp[0]), int.Parse(temp[1]));
        }
        public static IPEndPoint Format(string ipEndPoint, int number)
        {
            ipEndPoint = ipEndPoint.Replace("X", "" + number); //ersetze X durch Zahl
            return Parse(ipEndPoint);
        }
        public static List<IPEndPoint> Format(string ipEndpoint, int range1, int range2)
        {
            List<IPEndPoint> list = new List<IPEndPoint>();
            for (int i = range1; i <= range2; i++)
            {
                list.Add(Format(ipEndpoint, i));
            }
            return list;
        }
    }
}
