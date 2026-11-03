using GoFPatterns.FactoryMethod.NotifySystem.NotifiationServices;

namespace GoFPatterns.FactoryMethod.NotifySystem.Factories
{
    internal class PushNotificator : NotificationFactory
    {
        protected override INotificationService CreateNotification()
        {
            return new PushNotification();
        }    
    }
}
