using GoFPatterns.FactoryMethod.Fitnes.Factories;
using GoFPatterns.FactoryMethod.NotifySystem.Factories;

namespace GoFPatterns.FactoryMethod
{
    public static class DemondtrsteFactoryMethod
    {
        public static void DemonstrateFitnes()
        {
            // Durch Wahl der passenden Fabrik kann man zwischen verschiedenen Mitgliedschaftsarten wählen.
            var gymPoolFactory = new GymPoolMembershipFactory();
            var gymPoolMembership = gymPoolFactory.CreateMembership();

            Console.WriteLine($"Mitgliedschaft Fitness + Schwimmbad: {gymPoolMembership.Name}, {gymPoolMembership.Description}, Preis: {gymPoolMembership.Price}");
            var personalTrainingFactory = new PersonalTrainingMembershipFactory();

            var personalTrainingMembership = personalTrainingFactory.CreateMembership();
            Console.WriteLine($"Mitgliedschaft Personal Training: {personalTrainingMembership.Name}, {personalTrainingMembership.Description}, Preis: {personalTrainingMembership.Price}");
        }

        public static void DemonstrateNotify()
        {
            var umailFactory = new UmailNotificator();
            umailFactory.Notify("Dies ist eine E-Mail-Benachrichtigung.");

            var smsFactory = new SmsNotificator();
            smsFactory.Notify("Dies ist eine SMS-Benachrichtigung.");

            var pushFactory = new PushNotificator();
            pushFactory.Notify("Dies ist eine Push-Benachrichtigung.");
        }
    }
}
