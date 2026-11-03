namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Subsystem class - knows nothing about facade.
    public class Projector
    {
        public void On() => Console.WriteLine("  Projector: on");
        public void Off() => Console.WriteLine("  Projector: off");
        public void SetInput(string input) => Console.WriteLine($"  Projector: input {input}");
        public void WideScreenMode() => Console.WriteLine("  Projector: 16:9 mode");
    }
}
