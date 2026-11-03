namespace GoFPatterns.FactoryMethod.Fitnes.Memberships
{
    public class GymPoolMembership : IMembership
    {
        public string Name => "Abbonamento palestra + piscina";
        public string Description => "Accesso alla palestra e alla piscina";
        public decimal Price => 75m;
    }
}
