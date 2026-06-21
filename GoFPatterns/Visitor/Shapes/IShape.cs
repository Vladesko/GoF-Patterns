namespace GoFPatterns.Visitor.Shapes
{
    // The IShape interface defines the Accept method that takes a visitor as an argument.
    // Implementing this interface allows different shapes to accept visitors that can perform operations
    // on them without modifying the shape classes themselves.
    public interface IShape
    {
        void Accept(IShapeVisitor visitor);
    }
}
