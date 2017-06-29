using System;
using System.Management;

namespace WemManagementStudio.Utilities.Wmi
{
    public abstract class WmiBase
    {
        private readonly string _host;
        private readonly string _user;
        private readonly string _pass; 

        protected WmiBase(string host, string user, string pass)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentNullException(nameof(host));
            }

            if (!string.IsNullOrWhiteSpace(user) && string.IsNullOrWhiteSpace(pass))
            {
                throw new ArgumentException("There no user for provided password!");
            }

            if (!string.IsNullOrWhiteSpace(pass) && string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentException("There no password for provided user!");
            }

            _host = host;
            _user = user;
            _pass = pass;
        }

        protected ManagementScope CreateScope()
        {
            return new ManagementScope($@"\\{_host}\root\cimv2")
            {
                Options =
                    string.IsNullOrWhiteSpace(_user)
                        ? new ConnectionOptions()
                        : new ConnectionOptions
                        {
                            Username = _user,
                            Password = _pass,
                            Impersonation = ImpersonationLevel.Impersonate
                        }
            };
        }
    }
}
