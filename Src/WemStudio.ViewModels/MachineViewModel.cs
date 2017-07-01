using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using Caliburn.Micro;
using WemManagementStudio.Data;
using WemManagementStudio.Utilities;
using WemManagementStudio.Utilities.Wmi;

namespace WemManagementStudio.ViewModels
{
    public sealed class MachineViewModel : Screen
    {
        private readonly Machine _machine;
        private readonly IRepository<Machine, long> _machines;
       
        private bool _canDeploy;

        private readonly IDictionary<MachineType, Func<MachineViewModel, Task>> _deployManager;

        public MachineViewModel(Machine machine, IRepository<Machine, long> machines)
        {
            _machine = machine;
            _machines = machines;
          
            _canDeploy = true;

            _deployManager = new Dictionary<MachineType, Func<MachineViewModel, Task>>
            {
                {MachineType.Agent, DeployAgent},
                {MachineType.Broker, DeployBroker},
                {MachineType.Console, DeployConsole}
            };

            Log = new BindableCollection<LogViewModel>();
        }

        public long Id => _machine.Id;

        public string MachineName => _machine.Name;

        public string Host => _machine.Host;

        public string User => _machine.User;

        public string Pass => _machine.Pass;

        public string Path => _machine.Path;

        public MachineType MachineType => _machine.MachineType;

        public BindableCollection<LogViewModel> Log { get; }

        public async Task Deploy()
        {
            CanDeploy = false;

            try
            {
                await _deployManager[MachineType](this);
            }
            catch (Exception e)
            {
                Log.Add(new LogViewModel(LogLevel.Error, e.ToString()));
            }
            finally
            {
                CanDeploy = true;
            }
        }

        public bool CanDeploy
        {
            get { return _canDeploy; }

            set
            {
                _canDeploy = value;

                NotifyOfPropertyChange(() => CanDeploy);
            }
        }

        // TODO: Refactor. Not a good place. Thinking about global MachineViewModel Collection and Event Aggregator
        public void Remove()
        {
            _machines.Remove(_machine);
        }

        private static async Task DeployAgent(MachineViewModel viewModel)
        {
            viewModel.Log.Add(new LogViewModel(LogLevel.Information, $"Logining to {viewModel.Host}"));

            using (new Impersonation(viewModel.User.Split('\\')[0], viewModel.User.Split('\\')[1], viewModel.Pass))
            {
                viewModel.Log.Add(new LogViewModel(LogLevel.Information, "Stopping Agent Service"));
                var service = new ServiceController("Norskale Agent Host Service", viewModel.Host);
                await Task.Factory.StartNew(
                    () =>
                    {
                        if (service.Status != ServiceControllerStatus.Stopped)
                        {
                            service.Stop();
                            service.WaitForStatus(ServiceControllerStatus.Stopped);
                        }
                    });

                viewModel.Log.Add(new LogViewModel(LogLevel.Information, "Killing Agent UI"));
                await WmiProcess.KillAsync("VUEMUIAgent.exe", viewModel.Host, viewModel.User, viewModel.Pass);

                viewModel.Log.Add(new LogViewModel(LogLevel.Information, "Copy binaries"));
                await Task.Factory.StartNew(
                    () => DirectoryCopy(viewModel.Path,
                        $@"\\{viewModel.Host}\C$\Program Files (x86)\Norskale\Norskale Agent Host\", true));

                viewModel.Log.Add(new LogViewModel(LogLevel.Information, "Starting Service"));
                await Task.Factory.StartNew(
                    () =>
                    {
                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running);
                    });
              
                viewModel.Log.Add(new LogViewModel(LogLevel.Information, "Done"));
            }
        }

        private static async Task DeployBroker(MachineViewModel viewModel)
        {
            viewModel.Log.Add(new LogViewModel(LogLevel.Information, $"Logining to {viewModel.Host}"));

            using (new Impersonation(viewModel.User.Split('\\')[0], viewModel.User.Split('\\')[1], viewModel.Pass))
            {
                viewModel.Log.Add(new LogViewModel(LogLevel.Information, "Stopping Broker Service"));
                var service = new ServiceController("Norskale Infrastructure Service", viewModel.Host);
                await Task.Factory.StartNew(
                    () =>
                    {
                        if (service.Status != ServiceControllerStatus.Stopped)
                        {
                            service.Stop();
                            service.WaitForStatus(ServiceControllerStatus.Stopped);
                        }
                    });

                viewModel.Log.Add(new LogViewModel(LogLevel.Information, "Killing Broker tools"));
                var configToolTask =  WmiProcess.KillAsync("Norskale Broker Service Configuration Utility.exe", viewModel.Host, viewModel.User, viewModel.Pass);
                var dbToolTask = WmiProcess.KillAsync("Norskale Database Management Utility.exe", viewModel.Host, viewModel.User, viewModel.Pass);
                await Task.WhenAll(configToolTask, dbToolTask);

                viewModel.Log.Add(new LogViewModel(LogLevel.Information, "Copy binaries"));
                await Task.Factory.StartNew(
                    () => DirectoryCopy(viewModel.Path,
                        $@"\\{viewModel.Host}\C$\Program Files (x86)\Norskale\Norskale Infrastructure Services\", true));

                viewModel.Log.Add(new LogViewModel(LogLevel.Information, "Starting Service"));
                await Task.Factory.StartNew(
                    () =>
                    {
                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running);
                    });

                viewModel.Log.Add(new LogViewModel(LogLevel.Information, "Done"));
            }
        }

        private static async Task DeployConsole(MachineViewModel viewModel)
        {
            viewModel.Log.Add(new LogViewModel(LogLevel.Information, $"Logining to {viewModel.Host}"));

            using (new Impersonation(viewModel.User.Split('\\')[0], viewModel.User.Split('\\')[1], viewModel.Pass))
            {
                viewModel.Log.Add(new LogViewModel(LogLevel.Information, "Killing Admin Console"));
                await WmiProcess.KillAsync("Norskale Administration Console.exe", viewModel.Host, viewModel.User, viewModel.Pass);

                viewModel.Log.Add(new LogViewModel(LogLevel.Information, "Copy binaries"));
                await Task.Factory.StartNew(
                    () => DirectoryCopy(viewModel.Path,
                        $@"\\{viewModel.Host}\C$\Program Files (x86)\Norskale\Norskale Administration Console\", true));

                viewModel.Log.Add(new LogViewModel(LogLevel.Information, "Done"));
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            Parallel.ForEach(SkipFiles(dir.GetFiles()), new ParallelOptions {MaxDegreeOfParallelism = 10}, file =>
            {
                CopyFile(file, destDirName, 5);
            });

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        private static void CopyFile(FileInfo file, string destDirName, int retries)
        {
            var temppath = System.IO.Path.Combine(destDirName, file.Name);

            try
            {
                file.CopyTo(temppath, true);
            }
            catch
            {
                retries--;

                if (retries > 0)
                {
                    CopyFile(file, destDirName, retries);
                }
                else
                {
                    throw;
                }
            }
        }

        private static IEnumerable<FileInfo> SkipFiles(IEnumerable<FileInfo> files)
        {
            return files
                .Where(x => !x.Name.StartsWith("DevExpress."))
                .Where(x => !x.Name.StartsWith("Microsoft."))
                .Where(x => !x.Name.EndsWith(".pdb"));

        }
    }
}
