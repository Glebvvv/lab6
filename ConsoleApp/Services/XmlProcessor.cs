using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ConsoleApp.Models;

namespace ConsoleApp.Services
{
    public class XmlProcessor
    {
        // Чтение XML (LINQ to XML)
        public static List<FinancialDeposit> LoadDeposits(string xmlPath, AppLogger.LogAction logger)
        {
            try
            {
                XDocument doc = XDocument.Load(xmlPath);

                var deposits = doc.Root.Elements("deposit")
                    .Select(elem =>
                    {
                        try
                        {
                            string name = elem.Element("name")?.Value ?? "N/A";
                            decimal rate = (decimal?)elem.Element("nominalRate") ?? 0;

                            // Требование: обработка нулевых/отрицательных ставок
                            if (rate <= 0)
                            {
                                logger($"Пропуск депозита '{name}'. Ставка не положительная: {rate}", "WARN");
                                return null; // Будет отфильтрован
                            }

                            return new FinancialDeposit(name, rate);
                        }
                        catch (Exception ex)
                        {
                            logger($"Ошибка парсинга элемента deposit: {ex.Message}. Элемент пропущен.", "ERROR");
                            return null;
                        }
                    })
                    .Where(d => d != null) // Убираем невалидные/пропущенные
                    .ToList();

                logger($"Успешно загружено {deposits.Count} депозитов из {xmlPath}.", "INFO");
                return deposits;
            }
            catch (Exception ex)
            {
                logger($"Критическая ошибка при чтении {xmlPath}: {ex.Message}", "ERROR");
                throw; // Передаем ошибку выше, чтобы остановить выполнение
            }
        }
    }
}