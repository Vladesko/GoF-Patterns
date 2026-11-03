namespace GoFPatterns.FactoryMethod.Fitnes.Memberships
{
    internal class PersonalTrainingMembership : IMembership
    {
        public string Name => "Personal-Training-Mitgliedschaft";
        public string Description => "Zugang zu Personal-Training-Einheiten";
        public decimal Price => 100m;
    }
}
