namespace GoFPatterns.FactoryMethod.NotifySystem.NotifiationServices
{
    public class SmsNotification : INotificationService
    {
        public void Notify(string message)
        {
            Console.WriteLine($"SMS-уведомление: {message}");
        }
    }
}
