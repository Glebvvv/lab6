using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp.Services
{
    public class AppLogger
    {
        // Требование: хотя бы один делегат
        public delegate void LogAction(string message, string level);

        private readonly string _logFilePath;

        public AppLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
            // Очищаем старый лог при запуске
            if (File.Exists(_logFilePath))
            {
                File.Delete(_logFilePath);
            }
        }

        // Метод, соответствующий сигнатуре делегата, для логирования в файл
        public void LogToFile(string message, string level)
        {
            try
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level.ToUpper()}]: {message}";
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"КРИТИЧЕСКАЯ ОШИБКА ЛОГЕРА: {ex.Message}");
            }
        }

        // Метод для логирования в консоль
        public void LogToConsole(string message, string level)
        {
            if (level.Equals("ERROR", StringComparison.OrdinalIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (level.Equals("INFO", StringComparison.OrdinalIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine($"[{level.ToUpper()}]: {message}");
            Console.ResetColor();
        }
    }
}