namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Klasse des Subsystems.
    public class Lights
    {
        public void Dim(int percent) => Console.WriteLine($"  Licht: auf {percent} % gedimmt");
        public void On() => Console.WriteLine("  Licht: an");
    }
}
