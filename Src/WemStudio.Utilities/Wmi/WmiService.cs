using System;
using System.Management;
using System.Threading.Tasks;

namespace WemStudio.Utilities.Wmi
{
    public sealed partial class WmiService : WmiBase
    {
        private readonly string _service;

        public WmiService(string service)
            : this(service, "localhost", null, null)
        {
        }

        public WmiService(string service, string host)
            : this (service, host, null, null)
        {
        }

        public WmiService(string service, string host, string user, string pass)
            : base(host, user, pass)
        {
            if (string.IsNullOrWhiteSpace(service))
            {
                throw new ArgumentNullException(nameof(service));
            }

            _service = service;
        }

        public void Start()
        {
            StartInternal();
        }

        public Task StartAsync()
        {
            return Task.Factory.StartNew(StartInternal);
        }

        public void Stop()
        {
            StopInternal();
        }

        public Task StopAsync()
        {
            return Task.Factory.StartNew(StopInternal);
        }

        private void StartInternal()
        {
            var query = new SelectQuery($"select * from Win32_Service where name = '{_service}'");

            using (var searcher = new ManagementObjectSearcher(CreateScope(), query))
            {
                foreach (ManagementObject service in searcher.Get())
                {
                    if (service["Started"].Equals(false))
                    {
                        service.InvokeMethod("StartService", null);
                    }
                }
            }
        }

        public void StopInternal()
        {
            var query = new SelectQuery($"select * from Win32_Service where name = '{_service}'");

            using (var searcher = new ManagementObjectSearcher(CreateScope(), query))
            {
                foreach (ManagementObject service in searcher.Get())
                {
                    if (service["Started"].Equals(true))
                    {
                        service.InvokeMethod("StopService", null);
                    }
                }
            }
        }
    }
}
