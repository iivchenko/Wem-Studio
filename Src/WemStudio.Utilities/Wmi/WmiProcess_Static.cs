using System.Threading.Tasks;

namespace WemStudio.Utilities.Wmi
{
    public sealed partial class WmiProcess
    {
        public static Task StartAsync(string process, string host, string user, string pass)
        {
            return new WmiProcess(process, host, user, pass).StartAsync();
        }

        public static Task KillAsync(string process, string host, string user, string pass)
        {
            return new WmiProcess(process, host, user, pass).KillAsync();
        }

        public static Task KillAsync(string process, string host)
        {
            return new WmiProcess(process, host).KillAsync();
        }
    }
}
