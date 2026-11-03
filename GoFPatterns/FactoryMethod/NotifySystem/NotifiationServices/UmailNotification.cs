namespace GoFPatterns.FactoryMethod.NotifySystem.NotifiationServices
{
    public class UmailNotification : INotificationService
    {
        public void Notify(string message)
        {
            Console.WriteLine($"Umail-Benachrichtigung: {message}");
        }
    }
}
