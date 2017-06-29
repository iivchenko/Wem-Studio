using System.Threading.Tasks;

namespace WemManagementStudio.Utilities.Wmi
{
    public sealed partial class WmiService
    {
        public static Task StartAsync(string service, string host, string user, string pass)
        {
            return new WmiService(service, host, user, pass).StartAsync();
        }

        public static Task StopAsync(string service, string host, string user, string pass)
        {
            return new WmiService(service, host, user, pass).StopAsync();
        }
    }
}
