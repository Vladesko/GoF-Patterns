using GoFPatterns.FactoryMethod.Fitnes.Factories;
using GoFPatterns.FactoryMethod.NotifySystem.Factories;

namespace GoFPatterns.FactoryMethod
{
    public static class DemondtrsteFactoryMethod
    {
        public static void DemonstrateFitnes()
        {
            //Puoi scegliere tra diversi tipi di abbonamento usando la factory appropriata.
            var gymPoolFactory = new GymPoolMembershipFactory();
            var gymPoolMembership = gymPoolFactory.CreateMembership();

            Console.WriteLine($"Abbonamento palestra + piscina: {gymPoolMembership.Name}, {gymPoolMembership.Description}, Prezzo: {gymPoolMembership.Price}");
            var personalTrainingFactory = new PersonalTrainingMembershipFactory();

            var personalTrainingMembership = personalTrainingFactory.CreateMembership();
            Console.WriteLine($"Abbonamento con personal trainer: {personalTrainingMembership.Name}, {personalTrainingMembership.Description}, Prezzo: {personalTrainingMembership.Price}");
        }

        public static void DemonstrateNotify()
        {
            var umailFactory = new UmailNotificator();
            umailFactory.Notify("Questa è una notifica email.");

            var smsFactory = new SmsNotificator();
            smsFactory.Notify("Questa è una notifica SMS.");

            var pushFactory = new PushNotificator();
            pushFactory.Notify("Questa è una notifica push.");
        }
    }
}
