namespace GoFPatterns.Visitor.Shapes.VisitShapes
{
    public class Rectangle : IShape
    {
        public required double Width { get; init; }
        public required double Height { get; init; }
        public void Accept(IShapeVisitor visitor) => 
            visitor.VisitRectangle(this);
        
    }
}
