namespace GoFPatterns.FactoryMethod.NotifySystem.Factories
{
    public abstract class NotificationFactory
    {
        protected abstract INotificationService CreateNotification();

        public void Notify(string message)
        {
            var notificationService = CreateNotification();
            notificationService.Notify(message);
        }
    }
}
