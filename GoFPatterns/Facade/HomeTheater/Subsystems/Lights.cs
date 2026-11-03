namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Subsystem class.
    public class Lights
    {
        public void Dim(int percent) => Console.WriteLine($"  Lights: dimmed to {percent}%");
        public void On() => Console.WriteLine("  Lights: on");
    }
}
