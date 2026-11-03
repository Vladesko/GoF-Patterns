namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Clase del subsistema: no sabe nada de la fachada.
    public class Projector
    {
        public void On() => Console.WriteLine("  Proyector: encendido");
        public void Off() => Console.WriteLine("  Proyector: apagado");
        public void SetInput(string input) => Console.WriteLine($"  Proyector: entrada {input}");
        public void WideScreenMode() => Console.WriteLine("  Proyector: modo 16:9");
    }
}
