using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp.Models;
using ConsoleApp.Extensions;

namespace ConsoleApp.Services
{
    // Класс для хранения результатов расчета
    public class DepositCalculationResult
    {
        public string Name { get; set; }
        public decimal NominalRate { get; set; }
        public decimal EarAnnual { get; set; }
        public decimal EarQuarterly { get; set; }
        public decimal EarMonthly { get; set; }
        public decimal EarContinuous { get; set; }
        public decimal CustomPeriodEar { get; set; }
    }

    public class DepositCalculator
    {
        // 1. LINQ to Objects: Проекция (Select)
        public static List<DepositCalculationResult> CalculateRates(List<FinancialDeposit> deposits)
        {
            var results = deposits
                .Select(dep => new DepositCalculationResult
                {
                    Name = dep.Name,
                    NominalRate = dep.NominalRate,
                    EarAnnual = dep.CalculateEAR(1),
                    EarQuarterly = dep.CalculateEAR(4),
                    EarMonthly = dep.CalculateEAR(12),
                    EarContinuous = dep.CalculateContinuousEAR()
                })
                .ToList();

            return results;
        }

        // 2. LINQ to Objects: Агрегация (Average) и Группировка (GroupBy) - для второго отчета
        public static string GenerateSummary(List<DepositCalculationResult> results)
        {
            var summary = new StringBuilder();
            summary.AppendLine("--- Сводный Отчет по Депозитам ---");
            summary.AppendLine($"Всего обработано: {results.Count} депозитов.");

            // Агрегат
            decimal avgRate = results.Average(r => r.NominalRate);
            summary.AppendLine($"Средняя номинальная ставка: {avgRate.ToPercentageString(4)}");

            // Группировка
            var groups = results.GroupBy(r => r.NominalRate > 0.08M ? "Высокодоходные (> 8%)" : "Стандартные (<= 8%)");

            foreach (var group in groups.OrderBy(g => g.Key))
            {
                summary.AppendLine($"\n{group.Key} (Найдено: {group.Count()}):");
                foreach (var item in group)
                {
                    summary.AppendLine($"  - {item.Name} ({item.NominalRate.ToPercentageString(2)}) -> EAR (мес.): {item.EarMonthly.ToPercentageString(2)}");
                }
            }

            return summary.ToString();
        }
    }
}