namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Subsistema: notificaciones.
    public class NotificationService
    {
        public void SendEmail(string email, string text) => Console.WriteLine($"  Correo a {email}: {text}");
    }
}
