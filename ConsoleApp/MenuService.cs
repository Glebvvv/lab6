using ConsoleApp.Extensions;
using ConsoleApp.Models;
using ConsoleApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Services
{
    public class MenuService
    {
        private List<FinancialDeposit> _deposits;
        private List<DepositCalculationResult> _calculationResults;
        private readonly AppLogger.LogAction _logger;
        private readonly string _inputXml;
        private readonly string _reportAprEar;
        private readonly string _reportSummary;
        private readonly string _errorLog;

        public MenuService(AppLogger.LogAction logger, string inputXml, string reportAprEar, string reportSummary, string errorLog)
        {
            _logger = logger;
            _inputXml = inputXml;
            _reportAprEar = reportAprEar;
            _reportSummary = reportSummary;
            _errorLog = errorLog;
            _deposits = new List<FinancialDeposit>();
            _calculationResults = new List<DepositCalculationResult>();
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                DisplayHeader();

                Console.WriteLine(" ГЛАВНОЕ МЕНЮ");
                Console.WriteLine("═".PadRight(50, '═'));
                Console.WriteLine("1.  Загрузить данные из XML");
                Console.WriteLine("2.  Выполнить расчеты");
                Console.WriteLine("3.  Просмотреть результаты");
                Console.WriteLine("4.  Сохранить отчеты");
                Console.WriteLine("5.  Анализ и статистика");
                Console.WriteLine("6.  Теория и формулы");
                Console.WriteLine("0.  Выход");
                Console.WriteLine("═".PadRight(50, '═'));
                Console.Write("Выберите пункт меню: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        LoadDataMenu();
                        break;
                    case "2":
                        CalculateMenu();
                        break;
                    case "3":
                        ViewResultsMenu();
                        break;
                    case "4":
                        SaveReportsMenu();
                        break;
                    case "5":
                        AnalysisMenu();
                        break;
                    case "6":
                        ShowTheory();
                        break;
                    case "0":
                        Console.WriteLine("До свидания!");
                        return;
                    default:
                        ShowError("Неверный пункт меню!");
                        break;
                }

                if (choice != "0")
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        private void LoadDataMenu()
        {
            Console.Clear();
            DisplayHeader();
            Console.WriteLine(" ЗАГРУЗКА ДАННЫХ");
            Console.WriteLine("═".PadRight(50, '═'));

            try
            {
                if (!System.IO.File.Exists(_inputXml))
                {
                    Console.WriteLine("Файл deposits.xml не найден.");
                    Console.WriteLine("Создать пример файла? (y/n)");
                    var create = Console.ReadLine()?.ToLower();

                    if (create == "y" || create == "н")
                    {
                        CreateSampleXmlFile();
                        Console.WriteLine(" Пример файла создан!");
                    }
                    return;
                }

                Console.WriteLine("Загрузка данных...");
                _deposits = XmlProcessor.LoadDeposits(_inputXml, _logger);

                if (_deposits.Count == 0)
                {
                    ShowWarning("Не найдено валидных депозитов для обработки.");
                }
                else
                {
                    ShowSuccess($"Успешно загружено {_deposits.Count} депозитов!");
                    DisplayLoadedDeposits();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при загрузке: {ex.Message}");
            }
        }

        private void CalculateMenu()
        {
            Console.Clear();
            DisplayHeader();
            Console.WriteLine(" ВЫПОЛНЕНИЕ РАСЧЕТОВ");
            Console.WriteLine("═".PadRight(50, '═'));

            if (_deposits.Count == 0)
            {
                ShowWarning("Нет данных для расчетов. Сначала загрузите данные.");
                return;
            }

            Console.WriteLine("Выберите тип расчетов:");
            Console.WriteLine("1.  Полный расчет (APR + все EAR)");
            Console.WriteLine("2.  Быстрый расчет (только основные показатели)");
            Console.WriteLine("3.  Расчет для конкретного периода капитализации");
            Console.Write("Выберите: ");

            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        _calculationResults = DepositCalculator.CalculateRates(_deposits);
                        ShowSuccess("Полный расчет завершен!");
                        break;
                    case "2":
                        _calculationResults = _deposits.Select(dep => new DepositCalculationResult
                        {
                            Name = dep.Name,
                            NominalRate = dep.NominalRate,
                            EarMonthly = dep.CalculateEAR(12),
                            EarContinuous = dep.CalculateContinuousEAR()
                        }).ToList();
                        ShowSuccess("Быстрый расчет завершен!");
                        break;
                    case "3":
                        CustomPeriodCalculation();
                        break;
                    default:
                        ShowError("Неверный выбор!");
                        return;
                }

                DisplayCalculationSummary();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при расчетах: {ex.Message}");
            }
        }

        private void CustomPeriodCalculation()
        {
            Console.Write("Введите количество периодов капитализации в год: ");
            if (int.TryParse(Console.ReadLine(), out int periods) && periods > 0)
            {
                _calculationResults = _deposits.Select(dep => new DepositCalculationResult
                {
                    Name = dep.Name,
                    NominalRate = dep.NominalRate,
                    EarAnnual = dep.CalculateEAR(1),
                    EarQuarterly = dep.CalculateEAR(4),
                    EarMonthly = dep.CalculateEAR(12),
                    EarContinuous = dep.CalculateContinuousEAR(),

                }).ToList();

                ShowSuccess($"Расчет для {periods} периодов завершен!");
            }
            else
            {
                ShowError("Неверное количество периодов!");
            }
        }

        private void ViewResultsMenu()
        {
            if (_calculationResults.Count == 0)
            {
                ShowWarning("Нет результатов для отображения. Сначала выполните расчеты.");
                return;
            }

            while (true)
            {
                Console.Clear();
                DisplayHeader();
                Console.WriteLine("ПРОСМОТР РЕЗУЛЬТАТОВ");
                Console.WriteLine("═".PadRight(50, '═'));
                Console.WriteLine($"Найдено результатов: {_calculationResults.Count}");
                Console.WriteLine();
                Console.WriteLine("1.  Таблица всех результатов");
                Console.WriteLine("2.  Топ-5 по эффективной ставке");
                Console.WriteLine("3.  Поиск по названию");
                Console.WriteLine("4.  Детальный просмотр депозита");
                Console.WriteLine("0.  Назад");
                Console.Write("Выберите: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayAllResultsTable();
                        break;
                    case "2":
                        DisplayTopResults();
                        break;
                    case "3":
                        SearchDeposits();
                        break;
                    case "4":
                        ShowDepositDetails();
                        break;
                    case "0":
                        return;
                    default:
                        ShowError("Неверный пункт меню!");
                        break;
                }

                if (choice != "0")
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        private void SaveReportsMenu()
        {
            Console.Clear();
            DisplayHeader();
            Console.WriteLine(" СОХРАНЕНИЕ ОТЧЕТОВ");
            Console.WriteLine("═".PadRight(50, '═'));

            if (_calculationResults.Count == 0)
            {
                ShowWarning("Нет результатов для сохранения.");
                return;
            }

            Console.WriteLine("Выберите отчеты для сохранения:");
            Console.WriteLine("1.  Основной отчет (APR/EAR)");
            Console.WriteLine("2.  Сводный отчет");
            Console.WriteLine("3.  Все отчеты");
            Console.WriteLine("0.  Назад");
            Console.Write("Выберите: ");

            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        ReportGenerator.WriteAprEarReport(_calculationResults, _reportAprEar);
                        ShowSuccess($"Отчет сохранен: {_reportAprEar}");
                        break;
                    case "2":
                        string summary = DepositCalculator.GenerateSummary(_calculationResults);
                        ReportGenerator.WriteSummaryReport(summary, _reportSummary);
                        ShowSuccess($"Отчет сохранен: {_reportSummary}");
                        break;
                    case "3":
                        ReportGenerator.WriteAprEarReport(_calculationResults, _reportAprEar);
                        string summaryContent = DepositCalculator.GenerateSummary(_calculationResults);
                        ReportGenerator.WriteSummaryReport(summaryContent, _reportSummary);
                        ShowSuccess($"Отчеты сохранены: {_reportAprEar}, {_reportSummary}");
                        break;
                    case "0":
                        return;
                    default:
                        ShowError("Неверный выбор!");
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void AnalysisMenu()
        {
            Console.Clear();
            DisplayHeader();
            Console.WriteLine(" АНАЛИЗ И СТАТИСТИКА");
            Console.WriteLine("═".PadRight(50, '═'));

            if (_calculationResults.Count == 0)
            {
                ShowWarning("Нет данных для анализа.");
                return;
            }

            // Статистика
            var stats = new
            {
                Count = _calculationResults.Count,
                AvgNominal = _calculationResults.Average(r => r.NominalRate),
                MaxNominal = _calculationResults.Max(r => r.NominalRate),
                MinNominal = _calculationResults.Min(r => r.NominalRate),
                AvgEarMonthly = _calculationResults.Average(r => r.EarMonthly),
                BestEarMonthly = _calculationResults.OrderByDescending(r => r.EarMonthly).First()
            };

            Console.WriteLine(" ОСНОВНАЯ СТАТИСТИКА:");
            Console.WriteLine($"   Всего депозитов: {stats.Count}");
            Console.WriteLine($"   Средняя номинальная ставка: {stats.AvgNominal.ToPercentageString(2)}");
            Console.WriteLine($"   Лучшая эффективная ставка: {stats.BestEarMonthly.EarMonthly.ToPercentageString(2)}");
            Console.WriteLine($"   Максимальная номинальная: {stats.MaxNominal.ToPercentageString(2)}");
            Console.WriteLine($"   Минимальная номинальная: {stats.MinNominal.ToPercentageString(2)}");
            Console.WriteLine();

            // Группировка по категориям
            var categories = _calculationResults
                .GroupBy(r => r.NominalRate switch
                {
                    < 0.05m => "Низкая",
                    < 0.08m => "Средняя",
                    < 0.12m => "Высокая",
                    _ => "Премиум"
                })
                .OrderBy(g => g.Key);

            Console.WriteLine(" РАСПРЕДЕЛЕНИЕ ПО КАТЕГОРИЯМ:");
            foreach (var category in categories)
            {
                Console.WriteLine($"   {category.Key}: {category.Count()} депозитов");
            }
        }

        private void ShowTheory()
        {
            Console.Clear();
            DisplayHeader();
            Console.WriteLine(" ТЕОРИЯ И ФОРМУЛЫ");
            Console.WriteLine("═".PadRight(50, '═'));

            Console.WriteLine(" ОСНОВНЫЕ ПОНЯТИЯ:");
            Console.WriteLine("   APR (Annual Percentage Rate) - номинальная годовая ставка");
            Console.WriteLine("   EAR (Effective Annual Rate) - эффективная годовая ставка");
            Console.WriteLine();

            Console.WriteLine(" ФОРМУЛЫ РАСЧЕТА EAR:");
            Console.WriteLine("   Для n периодов капитализации:");
            Console.WriteLine("   EAR = (1 + APR/n)ⁿ - 1");
            Console.WriteLine();
            Console.WriteLine("   Для непрерывной капитализации:");
            Console.WriteLine("   EAR = e^APR - 1");
            Console.WriteLine();

            Console.WriteLine(" ПРИМЕРЫ ПЕРИОДОВ:");
            Console.WriteLine("   Годовая (n=1), Квартальная (n=4), Месячная (n=12)");
            Console.WriteLine("   Непрерывная (n=∞)");
            Console.WriteLine();

            Console.WriteLine(" ПРАКТИЧЕСКОЕ ПРИМЕНЕНИЕ:");
            Console.WriteLine("   EAR позволяет сравнивать депозиты с разной");
            Console.WriteLine("   частотой капитализации на единой основе");
        }

        private void DisplayHeader()
        {
            Console.WriteLine("===  КАЛЬКУЛЯТОР ЭФФЕКТИВНЫХ СТАВОК ДЕПОЗИТОВ ===");
            Console.WriteLine(FinancialDeposit.GetBankDisclaimer());
            Console.WriteLine();
        }

        private void DisplayLoadedDeposits()
        {
            Console.WriteLine("\n ЗАГРУЖЕННЫЕ ДЕПОЗИТЫ:");
            Console.WriteLine("-".PadRight(40, '-'));
            foreach (var deposit in _deposits.Take(10)) // Показываем первые 10
            {
                Console.WriteLine($"   • {deposit.Name}: {deposit.NominalRate.ToPercentageString(2)}");
            }
            if (_deposits.Count > 10)
            {
                Console.WriteLine($"   ... и еще {_deposits.Count - 10} депозитов");
            }
        }

        private void DisplayCalculationSummary()
        {
            Console.WriteLine("\n СВОДКА РАСЧЕТОВ:");
            Console.WriteLine("-".PadRight(50, '-'));

            var bestMonthly = _calculationResults.OrderByDescending(r => r.EarMonthly).First();
            var bestContinuous = _calculationResults.OrderByDescending(r => r.EarContinuous).First();

            Console.WriteLine($"   Лучшая месячная EAR: {bestMonthly.Name} - {bestMonthly.EarMonthly.ToPercentageString(4)}");
            Console.WriteLine($"   Лучшая непрерывная EAR: {bestContinuous.Name} - {bestContinuous.EarContinuous.ToPercentageString(4)}");
            Console.WriteLine($"   Средняя EAR (мес.): {_calculationResults.Average(r => r.EarMonthly).ToPercentageString(4)}");
        }

        private void DisplayAllResultsTable()
        {
            Console.WriteLine("\n ТАБЛИЦА РЕЗУЛЬТАТОВ:");
            Console.WriteLine("=".PadRight(80, '='));
            Console.WriteLine("{0,-20} {1,10} {2,12} {3,12} {4,12} {5,12}",
                "Депозит", "APR", "EAR(год)", "EAR(кварт)", "EAR(мес)", "EAR(непр)");
            Console.WriteLine("-".PadRight(80, '-'));

            foreach (var result in _calculationResults.OrderByDescending(r => r.EarMonthly))
            {
                Console.WriteLine("{0,-20} {1,10} {2,12} {3,12} {4,12} {5,12}",
                    result.Name.Length > 19 ? result.Name.Substring(0, 16) + "..." : result.Name,
                    result.NominalRate.ToPercentageString(2),
                    result.EarAnnual.ToPercentageString(2),
                    result.EarQuarterly.ToPercentageString(2),
                    result.EarMonthly.ToPercentageString(2),
                    result.EarContinuous.ToPercentageString(2));
            }
        }

        private void DisplayTopResults()
        {
            var topResults = _calculationResults
                .OrderByDescending(r => r.EarMonthly)
                .Take(5)
                .ToList();

            Console.WriteLine("\n ТОП-5 ПО ЭФФЕКТИВНОЙ СТАВКЕ:");
            Console.WriteLine("=".PadRight(60, '='));

            for (int i = 0; i < topResults.Count; i++)
            {
                var result = topResults[i];
                string medal = i switch { 0 => "🥇", 1 => "🥈", 2 => "🥉", _ => "  " };
                Console.WriteLine($"{medal} {i + 1}. {result.Name}");
                Console.WriteLine($"     APR: {result.NominalRate.ToPercentageString(2)} → " +
                                $"EAR: {result.EarMonthly.ToPercentageString(4)}");
            }
        }

        private void SearchDeposits()
        {
            Console.Write("\n Введите название для поиска: ");
            string searchTerm = Console.ReadLine()?.ToLower() ?? "";

            var found = _calculationResults
                .Where(r => r.Name.ToLower().Contains(searchTerm))
                .ToList();

            if (found.Any())
            {
                Console.WriteLine($"Найдено {found.Count} депозитов:");
                foreach (var result in found)
                {
                    Console.WriteLine($"   • {result.Name}: APR {result.NominalRate.ToPercentageString(2)} → " +
                                    $"EAR {result.EarMonthly.ToPercentageString(4)}");
                }
            }
            else
            {
                ShowWarning("Депозиты не найдены.");
            }
        }

        private void DisplayRateComparisonChart()
        {
            Console.WriteLine("\n ГРАФИК СРАВНЕНИЯ СТАВОК:");
            Console.WriteLine("APR vs EAR (месячная капитализация)");
            Console.WriteLine();

            var sortedResults = _calculationResults
                .OrderBy(r => r.NominalRate)
                .ToList();

            const int maxBarWidth = 30;

            foreach (var result in sortedResults)
            {
                string name = result.Name.Length > 15 ? result.Name.Substring(0, 12) + "..." : result.Name;
                int aprBarWidth = (int)(result.NominalRate * maxBarWidth * 10);
                int earBarWidth = (int)(result.EarMonthly * maxBarWidth * 10);

                Console.Write($"{name,-15} APR:");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(new string('█', Math.Min(aprBarWidth, maxBarWidth)));
                Console.ResetColor();

                Console.Write($"{"",15} EAR:");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(new string('█', Math.Min(earBarWidth, maxBarWidth)));
                Console.ResetColor();

                Console.WriteLine($"{"",15} {result.NominalRate.ToPercentageString(2)} → {result.EarMonthly.ToPercentageString(2)}");
                Console.WriteLine();
            }
        }

        private void ShowDepositDetails()
        {
            Console.WriteLine("\nВыберите депозит для детального просмотра:");
            for (int i = 0; i < Math.Min(_calculationResults.Count, 10); i++)
            {
                Console.WriteLine($"{i + 1}. {_calculationResults[i].Name}");
            }
            Console.Write("Номер: ");

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= _calculationResults.Count)
            {
                var deposit = _calculationResults[choice - 1];

                Console.WriteLine($"\n ДЕТАЛИ ДЕПОЗИТА: {deposit.Name}");
                Console.WriteLine("=".PadRight(40, '='));
                Console.WriteLine($"Номинальная ставка (APR): {deposit.NominalRate.ToPercentageString(4)}");
                Console.WriteLine($"Эффективная ставка (EAR):");
                Console.WriteLine($"  • Годовая: {deposit.EarAnnual.ToPercentageString(6)}");
                Console.WriteLine($"  • Квартальная: {deposit.EarQuarterly.ToPercentageString(6)}");
                Console.WriteLine($"  • Месячная: {deposit.EarMonthly.ToPercentageString(6)}");
                Console.WriteLine($"  • Непрерывная: {deposit.EarContinuous.ToPercentageString(6)}");

                // Дополнительная информация
                decimal difference = deposit.EarMonthly - deposit.NominalRate;
                Console.WriteLine($"Разница APR/EAR: {difference.ToPercentageString(4)}");
            }
            else
            {
                ShowError("Неверный выбор!");
            }
        }

        // Методы для цветного вывода
        private void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" {message}");
            Console.ResetColor();
        }

        private void ShowWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" {message}");
            Console.ResetColor();
        }

        private void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($" {message}");
            Console.ResetColor();
        }

        private void CreateSampleXmlFile()
        {
            string sampleXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<deposits>
  <deposit>
    <name>Сберегательный счет</name>
    <nominalRate>0.08</nominalRate>
  </deposit>
  <deposit>
    <name>Накопительный Плюс</name>
    <nominalRate>0.105</nominalRate>
  </deposit>
  <deposit>
    <name>Молодежный вклад</name>
    <nominalRate>0.06</nominalRate>
  </deposit>
  <deposit>
    <name>Премиум депозит</name>
    <nominalRate>0.12</nominalRate>
  </deposit>
</deposits>";

            System.IO.File.WriteAllText(_inputXml, sampleXml);
        }
    }
}