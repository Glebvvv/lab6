using ConsoleApp.Services;

namespace ConsoleApp
{
    class Program
    {
        private const string InputXml = "deposits.xml";
        private const string ReportAprEar = "apr_ear.txt";
        private const string ReportSummary = "summary_report.txt";
        private const string ErrorLog = "error_log.txt";

        static void Main(string[] args)
        {
            AppLogger logger = new AppLogger(ErrorLog);
            AppLogger.LogAction logAction = logger.LogToConsole;
            logAction += logger.LogToFile;

            try
            {
                MenuService menuService = new MenuService(logAction, InputXml, ReportAprEar, ReportSummary, ErrorLog);
                menuService.ShowMainMenu();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}