namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Subsystem – Benachrichtigungen.
    public class NotificationService
    {
        public void SendEmail(string email, string text) => Console.WriteLine($"  E-Mail an {email}: {text}");
    }
}
