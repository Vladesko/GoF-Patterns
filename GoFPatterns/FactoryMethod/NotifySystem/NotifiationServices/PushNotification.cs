
namespace GoFPatterns.FactoryMethod.NotifySystem.NotifiationServices
{
    public class PushNotification : INotificationService
    {
        public void Notify(string message)
        {
            Console.WriteLine($"Notificación push: {message}");
        }
    }
}
