namespace GoFPatterns.FactoryMethod.Fitnes.Memberships
{
    public class GymMembership : IMembership
    {
        public string Name => "Abbonamento palestra";
        public string Description => "Accesso alle strutture della palestra";
        public decimal Price => 50m;
    }
}
