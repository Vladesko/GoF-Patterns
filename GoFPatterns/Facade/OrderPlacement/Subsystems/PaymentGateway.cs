namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Подсистема - платежи.
    public class PaymentGateway
    {
        public bool Charge(string cardNumber, decimal amount)
        {
            // Эмуляция: карты, оканчивающиеся на "0000", отклоняются.
            var success = !cardNumber.EndsWith("0000");
            Console.WriteLine($"  Оплата: списание {amount:F2} с карты *{cardNumber[^4..]} - {(success ? "успешно" : "отклонено")}");
            return success;
        }
    }
}
