using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WeatherData.Business;
using System.Diagnostics;

namespace WeatherData.Business
{
    public class LightManager
    {
        private string _currentState = "OFF";
        private DateTime? _sunset;

        public void Run() 
        {
            Console.WriteLine("Startar Lamphanteraren");
            
            var timer = new Timer();
            timer.Interval = 60000;
            //timer.Interval = 10000;

            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_currentState == "OFF") 
            {
                if (!_sunset.HasValue || _sunset.Value.Date != DateTime.Now.Date)
                {
                    _currentState = "OFF";
                    _sunset = WeatherProvider.GetSunsetTime();

                    Console.WriteLine(string.Format("{0}: Solnedgång: {1}", DateTime.Now.ToString("yyyy-MM-dd"), _sunset.Value.ToString("HH:mm")));
                }

                var timeLeft =_sunset.Value - DateTime.Now;

                if (timeLeft.TotalMinutes <= 60 && DateTime.Now < _sunset)
                {
                    Console.WriteLine(string.Format("{0}: Skickar startsignal till alla lampor {1}", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("HH:mm")));
                    var processStartInfo = new ProcessStartInfo(@"C:\Program Files (x86)\Telldus\tdtool.exe", "--on lampor");
                    processStartInfo.CreateNoWindow = true;
                    
                    var process = Process.Start(processStartInfo);
                    
                    _currentState = "ON";
                }
            }
            else
            {
                //During regular weekdays
                if (DateTime.Now.DayOfWeek != DayOfWeek.Friday && DateTime.Now.DayOfWeek != DayOfWeek.Saturday)
                {
                    if (DateTime.Now.Hour >= 22)
                    {
                        Console.WriteLine(string.Format("{0}: Skickar släcksignal till alla lampor {1}", DateTime.Now.ToString("yyyy-MM-dd"), _sunset.Value.ToString("HH:mm")));
                        var processStartInfo = new ProcessStartInfo(@"C:\Program Files (x86)\Telldus\tdtool.exe", "--off lampor");
                        processStartInfo.CreateNoWindow = true;
                        var process = Process.Start(processStartInfo);
                        process.WaitForExit();

                        var hallwayProcessInfo = new ProcessStartInfo(@"C:\Program Files (x86)\Telldus\tdtool.exe", "--on Hall");
                        hallwayProcessInfo.CreateNoWindow = true;
                        Process.Start(hallwayProcessInfo);

                        _currentState = "OFF";
                        _sunset = null;
                    }
                }
                else if (DateTime.Now.Hour >= 3) // During weekends
                {
                    Console.WriteLine(string.Format("{0}: Skickar släcksignal till alla lampor {1}", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("HH:mm")));
                    var processStartInfo = new ProcessStartInfo(@"C:\Program Files (x86)\Telldus\tdtool.exe", "--off lampor");
                    processStartInfo.CreateNoWindow = true;
                    var process = Process.Start(processStartInfo);

                    _currentState = "OFF";
                    _sunset = null;
                }

                //Always turn off bedroom lamps if time has passed 21:30
                if (DateTime.Now.Hour >= 21 && DateTime.Now.Minute >= 30)
                {
                    Console.WriteLine(string.Format("{0}: Skickar släcksignal till sovrummet {1}", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("HH:mm")));
                    var processStartInfo = new ProcessStartInfo(@"C:\Program Files (x86)\Telldus\tdtool.exe", "--off Sovrum");
                    processStartInfo.CreateNoWindow = true;
                    var process = Process.Start(processStartInfo);
                }
            }
        }
    }
}
