using System.Diagnostics;

namespace WeatherData.Business
{
    public class TelldusManager : ITelldus
    {
        public void TurnOff(string section)
        {
            var processStartInfo = new ProcessStartInfo(@"C:\Program Files (x86)\Telldus\tdtool.exe", $"--off {section}")
            {
                CreateNoWindow = true
            };

            var process = Process.Start(processStartInfo);
            process?.WaitForExit();
        }

        public void TurnOn(string section)
        {
            var processStartInfo = new ProcessStartInfo(@"C:\Program Files (x86)\Telldus\tdtool.exe", $"--on {section}")
            {
                CreateNoWindow = true
            };

            var process = Process.Start(processStartInfo);
            process?.WaitForExit();
        }
    }
}
