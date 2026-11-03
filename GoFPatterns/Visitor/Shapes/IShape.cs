namespace GoFPatterns.Visitor.Shapes
{
    // Die Schnittstelle IShape definiert die Methode Accept, die einen Besucher als Argument erhält.
    // Durch ihre Implementierung können verschiedene Formen Besucher annehmen, die Operationen
    // auf ihnen ausführen, ohne dass die Formklassen selbst geändert werden müssen.
    public interface IShape
    {
        void Accept(IShapeVisitor visitor);
    }
}
