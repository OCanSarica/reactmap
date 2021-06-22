using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using core.Logger;

namespace core.Tools
{
    public sealed class LogTool
    {
        private readonly IConfiguration _Configuration;

        private readonly ILogger _Logger;

        public static LogTool Instance => _Instance.Value;

        private static readonly Lazy<LogTool> _Instance =
            new Lazy<LogTool>(() => new LogTool());

        private LogTool()
        {
            _Configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.json", true, true).
                Build();

            var _provider = new LoggerProvider(_Configuration["Logging:Directory"]);

            _Logger = _provider.CreateLogger("");
        }

        public void Error(Exception _ex, string _message = "") => 
            _Logger.LogError(_ex, _message);

        public void Warning(string _message) => 
            _Logger.LogWarning(_message);

        public void Debug(string _message) => 
            _Logger.LogDebug(_message);
    }
}
