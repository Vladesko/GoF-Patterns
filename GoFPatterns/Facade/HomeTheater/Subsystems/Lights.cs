namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Clase del subsistema.
    public class Lights
    {
        public void Dim(int percent) => Console.WriteLine($"  Luces: atenuadas al {percent}%");
        public void On() => Console.WriteLine("  Luces: encendidas");
    }
}
