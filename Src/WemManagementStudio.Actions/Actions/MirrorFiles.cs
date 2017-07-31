using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WemManagementStudio.Actions.Actions
{
    public class MirrorFiles : IOperatopImplementation
    {
        public MirrorFiles(string targetPath, string destinationPath)
        {
            TargetPath = targetPath ?? throw new ArgumentNullException(nameof(targetPath));
            DestinationPath = destinationPath ?? throw new ArgumentNullException(nameof(destinationPath));
        }

        public MirrorFiles(string targetPath, string destinationPath, string[] includeMasks) : this(targetPath, destinationPath)
        {
            if (includeMasks == null) throw new ArgumentNullException(nameof(includeMasks));
            if (includeMasks.Length == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(includeMasks));

            IncludeMask = includeMasks;
        }

        /// <summary>
        /// Directory fon current machine.
        /// </summary>
        private string TargetPath { get; }

        /// <summary>
        /// Directory on the remote machine.
        /// </summary>
        private string DestinationPath { get; }

        private string[] IncludeMask { get; }

        public Task<bool> Execute()
        {
            return Task<bool>.Factory.StartNew(() =>
            {
                var files = IncludeMask.SelectMany(
                    mask => Directory.GetFiles(TargetPath, mask, SearchOption.AllDirectories));

                Console.Out.WriteLine("");

                foreach (var path in files)
                {
                    var remoteFileName = CreateRemoveFileName(path, DestinationPath);

                    if (remoteFileName != null)
                    {
                        if (CheckFileExistRemote(remoteFileName))
                        {
                            if (CopyFile(path, remoteFileName))
                            {
                                Console.Out.WriteLine("Error while copying " + remoteFileName);
                            }
                        }
                    }
                }
                
                return false;
            });
        }

        private bool CopyFile(string targetFilePath, string destinationFilePath)
        {
            if (string.IsNullOrEmpty(targetFilePath))
                throw new ArgumentException("Value cannot be null or empty.", nameof(targetFilePath));
            if (string.IsNullOrEmpty(destinationFilePath))
                throw new ArgumentException("Value cannot be null or empty.", nameof(destinationFilePath));

            if (!File.Exists(targetFilePath)) return false;

            return CopyFileInternal(targetFilePath, destinationFilePath);
        }

        /// <summary>
        /// Copies file to the remote PC.
        /// </summary>
        /// <param name="targetFilePath">Represents the full file name on the current machine.</param>
        /// <param name="destinationFilePath">Fully qualified file path on the remote PC.</param>
        /// <returns></returns>
        private bool CopyFileInternal(string targetFilePath, string destinationFilePath)
        {
            try
            {
                File.Copy(targetFilePath, destinationFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        private bool CheckFileExistRemote(string remoteFileName)
        {
            if (string.IsNullOrEmpty(remoteFileName))
                throw new ArgumentException("Value cannot be null or empty.", nameof(remoteFileName));

            try
            {
                return File.Exists(remoteFileName);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private string CreateRemoveFileName(string localFullFileName, string remoteDirectory)
        {
            var fileName = Path.GetFileName(localFullFileName);

            if (fileName != null) return Path.Combine(remoteDirectory, fileName);

            return null;
        }
    }
}