namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Sottosistema - pagamenti.
    public class PaymentGateway
    {
        public bool Charge(string cardNumber, decimal amount)
        {
            // Emulazione: le carte che terminano con "0000" vengono rifiutate.
            var success = !cardNumber.EndsWith("0000");
            Console.WriteLine($"  Pagamento: addebito di {amount:F2} sulla carta *{cardNumber[^4..]} - {(success ? "ok" : "rifiutato")}");
            return success;
        }
    }
}
