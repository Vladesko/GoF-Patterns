namespace GoFPatterns.FactoryMethod.Fitnes.Memberships
{
    public class GymPoolMembership : IMembership
    {
        public string Name => "Abono de gimnasio + piscina";
        public string Description => "Acceso al gimnasio y a la piscina";
        public decimal Price => 75m;
    }
}
