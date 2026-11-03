namespace GoFPatterns.FactoryMethod.Fitnes.Memberships
{
    public class GymMembership : IMembership
    {
        public string Name => "Fitnessstudio-Mitgliedschaft";
        public string Description => "Zugang zum Fitnessstudio";
        public decimal Price => 50m;
    }
}
