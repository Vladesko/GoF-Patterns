namespace GoFPatterns.Command.SmartHome
{
    // Command - azione che può essere associata a qualsiasi pulsante del telecomando.
    public interface IHomeCommand
    {
        string Description { get; }
        void Execute();
    }
}
