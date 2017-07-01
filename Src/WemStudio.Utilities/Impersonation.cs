using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;

namespace WemStudio.Utilities
{
    public sealed class Impersonation : IDisposable
    {
        const int LOGON32_PROVIDER_DEFAULT = 3;
        const int LOGON32_LOGON_INTERACTIVE = 9;

        private ImpersonationHandle _tokenHandle;
        private WindowsImpersonationContext _windowsImpersonationContext;

        public Impersonation(string domain, string userName, string password)
        {
            DoImpersonate(domain, userName, password);
        }

        [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Justification = "Need for native code!")]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private void DoImpersonate(string domain, string username, string password)
        {
            if (!NativeMethods.LogonUser(username, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out _tokenHandle))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            _windowsImpersonationContext = new WindowsIdentity(_tokenHandle.DangerousGetHandle()).Impersonate();
        }

        public void Dispose()
        {
            _windowsImpersonationContext?.Undo();
            _tokenHandle?.Dispose();
        }
    }
}