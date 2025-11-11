using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ConsoleApp.Extensions
{
    public static class DecimalExtensions
    {
        // Метод расширения для форматирования ставки в проценты
        public static string ToPercentageString(this decimal value, int precision = 2)
        {
            // Умножаем на 100 и форматируем
            decimal percentage = value * 100;
            return percentage.ToString($"F{precision}", CultureInfo.InvariantCulture) + "%";
        }
    }
}