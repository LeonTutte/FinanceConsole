using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace FinanceConsoleLibrary.Modules.Static;

/// <summary>
///     Universal module to access the log
/// </summary>
public static class LogModule
{
    private static readonly Logger _fileLogger = new LoggerConfiguration()
        .WriteTo.File($"{ConfigurationModule.GetConfiguration()["Logging"]["LogFolder"]}/log_.txt",
            rollOnFileSizeLimit: true,
            fileSizeLimitBytes: int.Parse(ConfigurationModule.GetConfiguration()["Logging"]["MaxFileSize"]),
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: int.Parse(ConfigurationModule.GetConfiguration()["Logging"]["FileCountLimit"]),
            restrictedToMinimumLevel: LogEventLevel.Verbose)
        .CreateLogger();

    private static readonly Logger _logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger();

    /// <summary>
    ///     Write a Message with Level "Information" to the Log.
    /// </summary>
    /// <param name="message">Your Message for the Log.</param>
    /// <returns></returns>
    public static void WriteInformation(string message)
    {
        _logger.Information(message);
    }

    /// <summary>
    ///     Write a Message with Level "Error" to the Log.
    /// </summary>
    /// <param name="message">Your Message for the Log.</param>
    /// <param name="exception">Your exception for additional file logging</param>
    /// <returns></returns>
    public static void WriteError(string message, Exception? exception = null)
    {
        _logger.Error(message);

        if (exception != null)
        {
            _fileLogger.Error(exception.Message);
            _fileLogger.Error(exception.InnerException!.Message);
        }
    }

    /// <summary>
    ///     Write a Message with Level "Debug" to the Log.
    /// </summary>
    /// <param name="message">Your Message for the Log.</param>
    /// <returns></returns>
    public static void WriteDebug(string message)
    {
        _logger.Debug(message);
    }
}