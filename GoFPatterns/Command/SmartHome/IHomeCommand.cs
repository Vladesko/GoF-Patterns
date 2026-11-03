namespace GoFPatterns.Command.SmartHome
{
    // Comando: acción que se puede asignar a cualquier botón del mando.
    public interface IHomeCommand
    {
        string Description { get; }
        void Execute();
    }
}
