
namespace GoFPatterns.FactoryMethod.NotifySystem.NotifiationServices
{
    public class PushNotification : INotificationService
    {
        public void Notify(string message)
        {
            Console.WriteLine($"Push notification: {message}");
        }
    }
}
