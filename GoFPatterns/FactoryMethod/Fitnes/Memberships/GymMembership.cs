namespace GoFPatterns.FactoryMethod.Fitnes.Memberships
{
    public class GymMembership : IMembership
    {
        public string Name => "Абонемент в тренажёрный зал";
        public string Description => "Доступ в тренажёрный зал";
        public decimal Price => 50m;
    }
}
