using System.Collections;
using System.Net;
using System.Text;

namespace Igloocoder.MF.LogEntries
{
    public class LogEntries
    {
        private static string _token;
        private static LogEntries _instance;
        private readonly IPHostEntry _ipHostEntry;

        private readonly Hashtable _logLevelText = new Hashtable
        {
            {LogLevel.Debug, "Debug"},
            {LogLevel.Info, "Info"},
            {LogLevel.Warning, "Warning"},
            {LogLevel.Error, "Error"},
            {LogLevel.Fatal, "Fatal"}
        };

        private LogEntries(string token)
        {
            _token = token;
            _ipHostEntry = Dns.GetHostEntry("data.logentries.com");
        }

        public static void Initialize(string token)
        {
            _instance = new LogEntries(token);
        }

        public static LogEntries Instance()
        {
            return _instance;
        }

        public void For(LogLevel level, string logText)
        {
            using (var client = new TcpClient.TcpClient(_ipHostEntry.AddressList[0], 80))
            {
                var message = new StringBuilder();
                message.Append(_token).Append(" ");
                message.Append(_logLevelText[level]).Append(" ");
                message.Append(logText);

                client.Send(message.ToString());
            }
        }
    }
}