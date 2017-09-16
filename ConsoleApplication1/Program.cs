using System;
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
