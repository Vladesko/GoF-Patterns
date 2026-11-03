namespace GoFPatterns.FactoryMethod.Fitnes.Memberships
{
    internal class PersonalTrainingMembership : IMembership
    {
        public string Name => "Abono de entrenamiento personal";
        public string Description => "Acceso a sesiones de entrenamiento personal";
        public decimal Price => 100m;
    }
}
