namespace GoFPatterns.Visitor.Shapes
{
    // L'interfaccia IShape definisce il metodo Accept, che riceve un visitor come argomento.
    // Implementare questa interfaccia permette alle varie forme di accettare visitor che eseguono operazioni
    // su di esse senza modificare le classi delle forme.
    public interface IShape
    {
        void Accept(IShapeVisitor visitor);
    }
}
