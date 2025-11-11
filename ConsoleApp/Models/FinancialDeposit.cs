using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Models
{
    public partial class FinancialDeposit
    {
        // Const и Static
        public const string BankName = "Contoso Bank"; // const
        public static string GetBankDisclaimer() // static метод
        {
            return $"Все расчеты предоставлены {BankName}.";
        }

        // Переопределение virtual-метода
        public override decimal CalculateEAR(int periods)
        {
            // Можно добавить свою логику, но для примера просто вызовем базовую
            // Console.WriteLine($"Расчет для FinancialDeposit (n={periods})...");
            return base.CalculateEAR(periods);
        }

        // Переопределение ToString
        public override string ToString()
        {
            return $"[Финансовый] {base.GetBaseInfo()} (ID: {_depositId})";
        }
    }
    public partial class FinancialDeposit : BaseDeposit
    {
        // Поля (const, readonly, static)
        private readonly Guid _depositId; // readonly поле
        public static readonly decimal MinAllowedRate = 0.0001M; // static readonly

        // Свойство
        public Guid DepositId => _depositId;

        // Конструктор
        public FinancialDeposit(string name, decimal rate) : base(name, rate)
        {
            // Валидация по требованию (нулевая/отрицательная ставка)
            if (rate < MinAllowedRate)
            {
                throw new ArgumentException($"Ставка для '{name}' слишком низкая ({rate}).");
            }
            _depositId = Guid.NewGuid();
        }
    }
}