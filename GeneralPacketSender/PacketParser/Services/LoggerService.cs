using CommunityToolkit.Mvvm.ComponentModel;
using PacketSender.Core;
using System;
using System.Collections.ObjectModel;

namespace PacketParser.Services
{

    internal partial class LoggerService : ObservableObject, ILogger
    {
        public ObservableCollection<Log> Logs { get; private set; }
        public LoggerService()
        {
            Logs = new();
        }
        public void Debug(string message)
        {
            Log(message, LogLevel.Debug);
        }

        public void Error(string message)
        {
            Log(message, LogLevel.Error);
        }

        public void Error(Exception exception)
        {
            Log(exception.Message, LogLevel.Error);
        }

        public void Info(string message)
        {
            Log(message, LogLevel.Info);
        }

        public void Warning(string message)
        {
            Log(message, LogLevel.Warning);
        }

        public void Log(string message, LogLevel logLevel = LogLevel.Info)
        {
            Logs.Add(new Log(DateTime.Now.TimeOfDay, logLevel, message));
            OnPropertyChanged(nameof(Log));
        }
    }
}
