namespace GoFPatterns.Command.SmartHome
{
    // Befehl – Aktion, die jeder Taste der Fernbedienung zugewiesen werden kann.
    public interface IHomeCommand
    {
        string Description { get; }
        void Execute();
    }
}
