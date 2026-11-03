using GoFPatterns.FactoryMethod.Fitnes.Factories;
using GoFPatterns.FactoryMethod.NotifySystem.Factories;

namespace GoFPatterns.FactoryMethod
{
    public static class DemondtrsteFactoryMethod
    {
        public static void DemonstrateFitnes()
        {
            // Можно выбрать нужный тип абонемента, используя соответствующую фабрику.
            var gymPoolFactory = new GymPoolMembershipFactory();
            var gymPoolMembership = gymPoolFactory.CreateMembership();

            Console.WriteLine($"Абонемент «Тренажёрный зал + бассейн»: {gymPoolMembership.Name}, {gymPoolMembership.Description}, цена: {gymPoolMembership.Price}");
            var personalTrainingFactory = new PersonalTrainingMembershipFactory();

            var personalTrainingMembership = personalTrainingFactory.CreateMembership();
            Console.WriteLine($"Абонемент «Персональные тренировки»: {personalTrainingMembership.Name}, {personalTrainingMembership.Description}, цена: {personalTrainingMembership.Price}");
        }

        public static void DemonstrateNotify()
        {
            var umailFactory = new UmailNotificator();
            umailFactory.Notify("Это уведомление по электронной почте.");

            var smsFactory = new SmsNotificator();
            smsFactory.Notify("Это SMS-уведомление.");

            var pushFactory = new PushNotificator();
            pushFactory.Notify("Это push-уведомление.");
        }
    }
}
