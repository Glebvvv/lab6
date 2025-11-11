using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ConsoleApp.Extensions;

namespace ConsoleApp.Services
{
    public class ReportGenerator
    {
        // Отчет 1 (Требуемый 'apr_ear.txt')
        public static void WriteAprEarReport(List<DepositCalculationResult> results, string filePath)
        {
            var sb = new StringBuilder();
            // Заголовок
            sb.AppendLine("DepositName;NominalRate (APR);EAR_Annual (n=1);EAR_Quarterly (n=4);EAR_Monthly (n=12);EAR_Continuous (n=inf)");

            foreach (var res in results)
            {
                // Используем ToPercentageString для красоты
                sb.AppendFormat("{0};{1};{2};{3};{4};{5}{6}",
                    res.Name,
                    res.NominalRate.ToPercentageString(4),
                    res.EarAnnual.ToPercentageString(4),
                    res.EarQuarterly.ToPercentageString(4),
                    res.EarMonthly.ToPercentageString(4),
                    res.EarContinuous.ToPercentageString(4),
                    Environment.NewLine
                );
            }

            File.WriteAllText(filePath, sb.ToString());
        }

        // Отчет 2 (Требование "минимум 2 отчета")
        public static void WriteSummaryReport(string summaryContent, string filePath)
        {
            File.WriteAllText(filePath, summaryContent);
        }
    }
}