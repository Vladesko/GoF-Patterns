using GoFPatterns.FactoryMethod.Fitnes.Factories;
using GoFPatterns.FactoryMethod.NotifySystem.Factories;

namespace GoFPatterns.FactoryMethod
{
    public static class DemondtrsteFactoryMethod
    {
        public static void DemonstrateFitnes()
        {
            //You can make choise between different membership types by using the appropriate factory.
            var gymPoolFactory = new GymPoolMembershipFactory();
            var gymPoolMembership = gymPoolFactory.CreateMembership();

            Console.WriteLine($"Gym Pool Membership: {gymPoolMembership.Name}, {gymPoolMembership.Description}, Price: {gymPoolMembership.Price}");
            var personalTrainingFactory = new PersonalTrainingMembershipFactory();

            var personalTrainingMembership = personalTrainingFactory.CreateMembership();
            Console.WriteLine($"Personal Training Membership: {personalTrainingMembership.Name}, {personalTrainingMembership.Description}, Price: {personalTrainingMembership.Price}");
        }

        public static void DemonstrateNotify()
        {
            var umailFactory = new UmailNotificator();
            umailFactory.Notify("This is an email notification.");

            var smsFactory = new SmsNotificator();
            smsFactory.Notify("This is an SMS notification.");

            var pushFactory = new PushNotificator();
            pushFactory.Notify("This is a push notification.");
        }
    }
}
