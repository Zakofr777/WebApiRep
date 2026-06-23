using System;
using System.IO;

namespace BankingApplication.Services;

public class LoggerService
{
    private  string logFilePath = "app.log";

    public void LogInfo(string message)
    {
        Log("INFO", message);
    }

    public void LogError(string message, Exception? ex = null)
    {
        string fullMessage = ex != null ? $"{message} | Exception: {ex.Message}" : message;
        Log("ERROR", fullMessage);
    }

    private void Log(string level, string message)
    {
        try
        {
            string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
            
            File.AppendAllText(logFilePath, logLine + Environment.NewLine);
        }
        catch
        {
        }
    }
}