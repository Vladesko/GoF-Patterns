namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Classe del sottosistema.
    public class Lights
    {
        public void Dim(int percent) => Console.WriteLine($"  Luci: abbassate al {percent}%");
        public void On() => Console.WriteLine("  Luci: accese");
    }
}
