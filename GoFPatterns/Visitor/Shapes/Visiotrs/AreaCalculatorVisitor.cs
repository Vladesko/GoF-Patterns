using GoFPatterns.Visitor.Shapes.VisitShapes;

namespace GoFPatterns.Visitor.Shapes.Visiotrs
{
    internal class AreaCalculatorVisitor : IShapeVisitor
    {
        public double TotalArea { get; private set; }

        public void VisitCircle(Circle circle)
        {
            TotalArea += Math.PI * circle.Radius * circle.Radius;
        }

        public void VisitRectangle(Rectangle rectangle)
        {
            TotalArea += rectangle.Width * rectangle.Height;
        }
    }
}

