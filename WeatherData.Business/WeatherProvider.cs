using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeatherData.Business
{
    public class WeatherProvider
    {
        public static DateTime GetSunsetTime() 
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.yr.no/place/Sweden/V%C3%A4sterbotten/Skellefte%C3%A5/forecast.xml");
            request.Method = "GET";
            request.Accept = "Accept=application/xml";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                var serializer = new XmlSerializer(typeof(weatherdata));

                 var data = (weatherdata)serializer.Deserialize(reader);

                 return data.sun.set;
            }
        }
    }
}
