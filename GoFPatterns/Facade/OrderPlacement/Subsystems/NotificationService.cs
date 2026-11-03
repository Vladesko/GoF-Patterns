namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Subsystem - notifications.
    public class NotificationService
    {
        public void SendEmail(string email, string text) => Console.WriteLine($"  Email to {email}: {text}");
    }
}
