namespace GoFPatterns.FactoryMethod.Fitnes.Memberships
{
    public class GymPoolMembership : IMembership
    {
        public string Name => "Gym + Pool Membership";
        public string Description => "Access to gym and pool facilities";
        public decimal Price => 75m;
    }
}
