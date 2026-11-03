namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Subsystem – Zahlungen.
    public class PaymentGateway
    {
        public bool Charge(string cardNumber, decimal amount)
        {
            // Emulation: Karten, die auf "0000" enden, werden abgelehnt.
            var success = !cardNumber.EndsWith("0000");
            Console.WriteLine($"  Zahlung: {amount:F2} von Karte *{cardNumber[^4..]} abbuchen – {(success ? "ok" : "abgelehnt")}");
            return success;
        }
    }
}
