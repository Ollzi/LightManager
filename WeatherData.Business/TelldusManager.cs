using System.Diagnostics;

namespace WeatherData.Business
{
    public class TelldusManager : ITelldus
    {
        public void TurnOff(string section)
        {
            var processStartInfo = new ProcessStartInfo(@"C:\Program Files (x86)\Telldus\tdtool.exe", $"--off {section}")
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            RepeatProcess(10, processStartInfo);
        }

        public void TurnOn(string section)
        {
            var processStartInfo = new ProcessStartInfo(@"C:\Program Files (x86)\Telldus\tdtool.exe", $"--on {section}")
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            RepeatProcess(10, processStartInfo);
        }

        private void RepeatProcess(int numberOfTimes, ProcessStartInfo processStartInfo)
        {
            int count = 0;

            while(count <= numberOfTimes)
            {
                var process = Process.Start(processStartInfo);
                process?.WaitForExit();

                System.Threading.Thread.Sleep(200);
                count++;
            }
        }
    }
}
