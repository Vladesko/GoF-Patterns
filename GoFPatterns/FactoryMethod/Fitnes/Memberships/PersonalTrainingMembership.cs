namespace GoFPatterns.FactoryMethod.Fitnes.Memberships
{
    internal class PersonalTrainingMembership : IMembership
    {
        public string Name => "Personal Training Membership";
        public string Description => "Access to personal training sessions";
        public decimal Price => 100m;
    }
}
