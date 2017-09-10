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
        private readonly ITelldus _telldus;
        private string _currentState = "OFF";
        private DateTime? _sunset;
        private DateTime? _unitTestDateTime;

        public LightManager()
            :this(new TelldusManager())
        {
        }

        public LightManager(ITelldus telldus)
        {
            _telldus = telldus;
        }

        public void Run() 
        {
            Console.WriteLine("Startar Lamphanteraren");
            
            var timer = new Timer();
            timer.Interval = 60000;
            //timer.Interval = 10000;

            timer.Elapsed += timer_Elapsed;
            timer.Start();
            TimerElapsed();
        }

        public void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimerElapsed();
        }

        public void TimerElapsed()
        {
            if (_currentState == "OFF")
            {
                if (!_sunset.HasValue || _sunset.Value.Date != DateTimeNow.Date)
                {
                    _currentState = "OFF";
                    _sunset = WeatherProvider.GetSunsetTime();

                    Console.WriteLine($"{DateTimeNow:yyyy-MM-dd}: Solnedgång: {_sunset.Value:HH:mm}");
                }

                var timeLeft = _sunset.Value - DateTimeNow;

                if (timeLeft.TotalMinutes <= 50 && DateTimeNow < _sunset)
                {
                    Console.WriteLine($"{DateTimeNow:yyyy-MM-dd}: Skickar startsignal till alla lampor {DateTimeNow:HH:mm}");
                    _telldus.TurnOn("lampor");
                    _currentState = "ON";
                }
            }
            else
            {
                //During regular weekdays
                if (DateTimeNow.DayOfWeek != DayOfWeek.Friday && DateTimeNow.DayOfWeek != DayOfWeek.Saturday)
                {
                    if (DateTimeNow.Hour >= 22)
                    {
                        Console.WriteLine($"{DateTimeNow:yyyy-MM-dd}: Skickar släcksignal till alla lampor {DateTimeNow:HH:mm}");

                        _telldus.TurnOff("lampor");
                        _telldus.TurnOn("Hall");

                        _currentState = "OFF";
                        _sunset = null;
                    }
                }

                //Always turn off bedroom lamps if time has passed 21:30
                if (DateTimeNow.Hour >= 21 && DateTimeNow.Minute >= 30)
                {
                    Console.WriteLine($"{DateTimeNow:yyyy-MM-dd}: Skickar släcksignal till sovrummet {DateTimeNow:HH:mm}");
                    _telldus.TurnOff("Sovrum");
                }
            }

            if (DateTimeNow.Hour >= 3 && DateTimeNow .Hour < 4) // Always shut down everything when the clock has passed 03:00
            {
                Console.WriteLine($"{DateTimeNow:yyyy-MM-dd}: Skickar släcksignal till alla lampor {DateTimeNow:HH:mm}");
                _telldus.TurnOff("lampor");

                _currentState = "OFF";
                _sunset = null;
            }
        }

        public DateTime DateTimeNow
        {
            get
            {
                if (_unitTestDateTime.HasValue)
                {
                    return _unitTestDateTime.Value;
                }

                return DateTime.Now;
            }
            set
            {
                _unitTestDateTime = value;
            }
        }
    }
}
