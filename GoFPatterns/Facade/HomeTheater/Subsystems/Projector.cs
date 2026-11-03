namespace GoFPatterns.Facade.HomeTheater.Subsystems
{
    // Classe del sottosistema - non sa nulla della facade.
    public class Projector
    {
        public void On() => Console.WriteLine("  Proiettore: acceso");
        public void Off() => Console.WriteLine("  Proiettore: spento");
        public void SetInput(string input) => Console.WriteLine($"  Proiettore: ingresso {input}");
        public void WideScreenMode() => Console.WriteLine("  Proiettore: modalità 16:9");
    }
}
