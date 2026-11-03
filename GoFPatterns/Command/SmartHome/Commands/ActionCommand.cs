namespace GoFPatterns.Command.SmartHome.Commands
{
    // Comando concreto basado en un delegado. En C# un comando sencillo puede ser simplemente una lambda,
    // así que no necesitamos una clase aparte para cada acción de cada dispositivo.
    public class ActionCommand(string description, Action action) : IHomeCommand
    {
        public string Description { get; } = description;

        public void Execute() => action();
    }
}
