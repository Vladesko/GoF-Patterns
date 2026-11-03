namespace GoFPatterns.Command.SmartHome
{
    // Команда - действие, которое можно назначить на любую кнопку пульта.
    public interface IHomeCommand
    {
        string Description { get; }
        void Execute();
    }
}
