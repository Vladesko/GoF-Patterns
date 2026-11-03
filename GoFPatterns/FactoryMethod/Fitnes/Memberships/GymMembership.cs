namespace GoFPatterns.FactoryMethod.Fitnes.Memberships
{
    public class GymMembership : IMembership
    {
        public string Name => "Gym Membership";
        public string Description => "Access to gym facilities";
        public decimal Price => 50m;
    }
}
