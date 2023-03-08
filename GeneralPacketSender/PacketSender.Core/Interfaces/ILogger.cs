using System.Collections.ObjectModel;

namespace PacketSender.Core
{
    public record Log(TimeSpan Time, LogLevel LogLevel, string Message);
    public interface ILogger
    {
        ObservableCollection<Log> Logs { get; }
        void Info(string message);
        void Warning(string message);
        void Error(string message);
        void Debug(string message);
        void Error(Exception exception);
        void Log(string message, LogLevel logLevel = LogLevel.Info);
    }
}
