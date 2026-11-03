namespace GoFPatterns.FactoryMethod.Fitnes.Memberships
{
    public class GymPoolMembership : IMembership
    {
        public string Name => "Mitgliedschaft Fitnessstudio + Schwimmbad";
        public string Description => "Zugang zu Fitnessstudio und Schwimmbad";
        public decimal Price => 75m;
    }
}
