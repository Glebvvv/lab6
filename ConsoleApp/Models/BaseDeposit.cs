using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConsoleApp.Extensions; // Подключаем наш метод расширения

namespace ConsoleApp.Models
{
    // Базовый класс
    public class BaseDeposit
    {
        public string Name { get; protected set; }
        public decimal NominalRate { get; protected set; } // Номинальная годовая ставка (напр., 0.08)

        public BaseDeposit(string name, decimal rate)
        {
            Name = name;
            NominalRate = rate;
        }

        // --- Требование: 3+ метода (1-2 virtual) ---

        // Метод 1 (virtual): Расчет EAR (Effective Annual Rate) для n периодов
        public virtual decimal CalculateEAR(int periods)
        {
            if (periods <= 0)
                throw new ArgumentException("Количество периодов должно быть положительным.");

            // Формула: EAR = (1 + r/n)^n - 1
            // Используем double для Math.Pow, затем кастуем обратно в decimal
            double r = (double)NominalRate;
            double n = (double)periods;
            double ear = Math.Pow(1 + r / n, n) - 1;
            return (decimal)ear;
        }

        // Метод 2 (virtual): Расчет EAR при непрерывном начислении
        public virtual decimal CalculateContinuousEAR()
        {
            // Формула: EAR = e^r - 1
            double r = (double)NominalRate;
            double ear = Math.Exp(r) - 1;
            return (decimal)ear;
        }

        // Метод 3 (non-virtual): Простое отображение
        public string GetBaseInfo()
        {
            // Используем метод расширения
            return $"Депозит: {Name}, Номинальная ставка: {NominalRate.ToPercentageString(2)}";
        }
    }
}