using GoFPatterns.FactoryMethod.NotifySystem.NotifiationServices;

namespace GoFPatterns.FactoryMethod.NotifySystem.Factories
{
    public class UmailNotificator : NotificationFactory
    {
        protected override INotificationService CreateNotification()
        {
            return new UmailNotification();
        }
    }
}
