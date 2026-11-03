namespace GoFPatterns.Facade.OrderPlacement.Subsystems
{
    // Подсистема - уведомления.
    public class NotificationService
    {
        public void SendEmail(string email, string text) => Console.WriteLine($"  Письмо на {email}: {text}");
    }
}
