namespace GoFPatterns.FactoryMethod.Fitnes.Memberships
{
    internal class PersonalTrainingMembership : IMembership
    {
        public string Name => "Abbonamento con personal trainer";
        public string Description => "Accesso alle sessioni di allenamento personalizzato";
        public decimal Price => 100m;
    }
}
