namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Subsystem - payments.
    public class PaymentGateway
    {
        public bool Charge(string cardNumber, decimal amount)
        {
            // Emulation: cards ending with "0000" are declined.
            var success = !cardNumber.EndsWith("0000");
            Console.WriteLine($"  Payment: charge {amount:F2} from card *{cardNumber[^4..]} - {(success ? "ok" : "declined")}");
            return success;
        }
    }
}
