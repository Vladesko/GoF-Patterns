namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Klasse des Subsystems – weiß nichts von der Fassade.
    public class Projector
    {
        public void On() => Console.WriteLine("  Projektor: an");
        public void Off() => Console.WriteLine("  Projektor: aus");
        public void SetInput(string input) => Console.WriteLine($"  Projektor: Eingang {input}");
        public void WideScreenMode() => Console.WriteLine("  Projektor: 16:9-Modus");
    }
}
