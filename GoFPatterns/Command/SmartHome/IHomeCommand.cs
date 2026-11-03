namespace GoFPatterns.Command.SmartHome
{
    // Command - action which can be bound to any button of the remote.
    public interface IHomeCommand
    {
        string Description { get; }
        void Execute();
    }
}
