using GoFPatterns.FactoryMethod.Fitnes.Factories;
using GoFPatterns.FactoryMethod.NotifySystem.Factories;

namespace GoFPatterns.FactoryMethod
{
    public static class DemondtrsteFactoryMethod
    {
        public static void DemonstrateFitnes()
        {
            // Se puede elegir entre distintos tipos de abono usando la fábrica correspondiente.
            var gymPoolFactory = new GymPoolMembershipFactory();
            var gymPoolMembership = gymPoolFactory.CreateMembership();

            Console.WriteLine($"Abono gimnasio + piscina: {gymPoolMembership.Name}, {gymPoolMembership.Description}, Precio: {gymPoolMembership.Price}");
            var personalTrainingFactory = new PersonalTrainingMembershipFactory();

            var personalTrainingMembership = personalTrainingFactory.CreateMembership();
            Console.WriteLine($"Abono de entrenamiento personal: {personalTrainingMembership.Name}, {personalTrainingMembership.Description}, Precio: {personalTrainingMembership.Price}");
        }

        public static void DemonstrateNotify()
        {
            var umailFactory = new UmailNotificator();
            umailFactory.Notify("Esta es una notificación por correo electrónico.");

            var smsFactory = new SmsNotificator();
            smsFactory.Notify("Esta es una notificación por SMS.");

            var pushFactory = new PushNotificator();
            pushFactory.Notify("Esta es una notificación push.");
        }
    }
}
