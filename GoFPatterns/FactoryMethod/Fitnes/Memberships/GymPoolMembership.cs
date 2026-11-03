namespace GoFPatterns.FactoryMethod.Fitnes.Memberships
{
    public class GymPoolMembership : IMembership
    {
        public string Name => "Абонемент «Тренажёрный зал + бассейн»";
        public string Description => "Доступ в тренажёрный зал и бассейн";
        public decimal Price => 75m;
    }
}
