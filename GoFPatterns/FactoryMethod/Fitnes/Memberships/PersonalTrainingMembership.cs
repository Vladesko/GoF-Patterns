namespace GoFPatterns.FactoryMethod.Fitnes.Memberships
{
    internal class PersonalTrainingMembership : IMembership
    {
        public string Name => "Абонемент на персональные тренировки";
        public string Description => "Доступ к персональным тренировкам";
        public decimal Price => 100m;
    }
}
