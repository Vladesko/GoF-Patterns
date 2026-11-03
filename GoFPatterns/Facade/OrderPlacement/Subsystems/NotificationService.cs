namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Sottosistema - notifiche.
    public class NotificationService
    {
        public void SendEmail(string email, string text) => Console.WriteLine($"  Email a {email}: {text}");
    }
}
