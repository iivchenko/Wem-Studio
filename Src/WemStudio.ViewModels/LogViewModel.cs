namespace WemManagementStudio.ViewModels
{
    public sealed class LogViewModel
    {
        public LogViewModel(LogLevel level, string message)
        {
            Level = level;
            Message = message;
        }

        public LogLevel Level { get;set; }

        public string Message { get; set; }
    }

    public enum LogLevel
    {
        Error,
        Warning,
        Information,
        Debug,
        Trace
    }
}
