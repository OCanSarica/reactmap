using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using core.Models;

namespace core.Logger
{
    internal sealed class Logger : ILogger
    {
        private readonly string _Path;

        public Logger(string _path) => _Path = _path;

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public async void Log<TState>(
            LogLevel _logLevel, EventId _eventId,
            TState _state, Exception _exception,
            Func<TState, Exception, string> _formatter)
        {
            try
            {
                if (_logLevel == LogLevel.Information)
                {
                    return;
                }

                using var _streamWriter = new StreamWriter(_Path, true);

                await _streamWriter.WriteLineAsync(
                    $"{DateTime.Now.ToLongTimeString()} => " +
                    $"log level : {_logLevel.ToString()} | " +
                    $"event id : {_eventId.Id} | event name : {_eventId.Name} | " +
                    $"message : {_formatter(_state, null)} | " +
                    $"exception: {_exception?.ToString()}");

                _streamWriter.Close();

                await _streamWriter.DisposeAsync();
            }
            catch (Exception _ex)
            {
            }
        }
    }

    public class LoggerProvider : ILoggerProvider
    {
        private readonly string _Path;

        public LoggerProvider(string _directory)
        {
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }

            var _now = DateTime.Now;

            var _fileName = $"{_now.Year}_{_now.Month}_{_now.Day}.log";

            _Path = Path.Combine(_directory, _fileName);
        }

        public ILogger CreateLogger(string categoryName) => new Logger(_Path);

        public void Dispose() => GC.SuppressFinalize(true);
    }
}
