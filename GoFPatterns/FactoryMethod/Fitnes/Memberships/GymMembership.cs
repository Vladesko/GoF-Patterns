namespace GoFPatterns.FactoryMethod.Fitnes.Memberships
{
    public class GymMembership : IMembership
    {
        public string Name => "Abono de gimnasio";
        public string Description => "Acceso a las instalaciones del gimnasio";
        public decimal Price => 50m;
    }
}
