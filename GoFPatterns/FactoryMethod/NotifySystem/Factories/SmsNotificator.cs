using GoFPatterns.FactoryMethod.NotifySystem.NotifiationServices;

namespace GoFPatterns.FactoryMethod.NotifySystem.Factories
{
    internal class SmsNotificator : NotificationFactory
    {
        protected override INotificationService CreateNotification()
        {
            return new SmsNotification();
        }   
    }
}
