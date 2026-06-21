namespace GoFPatterns.Visitor.Shapes.VisitShapes
{
    public class Circle : IShape
    {
        public required double Radius { get; init; }

        public void Accept(IShapeVisitor visitor) =>   
            visitor.VisitCircle(this);
    }
}
