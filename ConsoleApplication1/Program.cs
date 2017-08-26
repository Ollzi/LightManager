using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherData.Business;


namespace ConsoleApplication1
{
    class Program
    {
        

        static void Main(string[] args)
        {
            var lightManager = new LightManager();
            lightManager.Run();

            Console.ReadLine();
        }
    }
}
