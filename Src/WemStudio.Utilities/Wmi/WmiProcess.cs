using System;
using System.Management;
using System.Threading.Tasks;

namespace WemStudio.Utilities.Wmi
{
    public sealed partial class WmiProcess : WmiBase
    {
        private readonly string _process;

        public WmiProcess(string process, string host)
            : base(host, null, null)
        {
            _process = process;
        }

        public WmiProcess(string process, string host, string user, string pass)
            : base(host, user, pass)
        {
            _process = process;
        }

        public void Start()
        {
            StartInternal();
        }

        public void Kill()
        {
            KillInternal();
        }

        public Task StartAsync()
        {
            return Task.Factory.StartNew(StartInternal);
        }

        public Task KillAsync()
        {
            return Task.Factory.StartNew(KillInternal);
        }

        private void StartInternal()
        {
            throw new NotSupportedException();
        }

        private void KillInternal()
        {
            var query = new SelectQuery($@"select * from Win32_process where name = '{_process}'");

            using (var searcher = new ManagementObjectSearcher(CreateScope(), query))
            {
                foreach (ManagementObject process in searcher.Get())
                {
                    process.InvokeMethod("Terminate", null);
                }
            }
        }
    }
}
