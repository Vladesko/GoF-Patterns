namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Subsistema: pagos.
    public class PaymentGateway
    {
        public bool Charge(string cardNumber, decimal amount)
        {
            // Emulación: las tarjetas que terminan en "0000" se rechazan.
            var success = !cardNumber.EndsWith("0000");
            Console.WriteLine($"  Pago: cargo de {amount:F2} a la tarjeta *{cardNumber[^4..]} - {(success ? "ok" : "rechazado")}");
            return success;
        }
    }
}
