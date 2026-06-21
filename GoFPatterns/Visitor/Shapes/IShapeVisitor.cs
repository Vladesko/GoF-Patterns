using GoFPatterns.Visitor.Shapes.VisitShapes;

namespace GoFPatterns.Visitor.Shapes
{
    // Visitor interface defining operations for each shape type
    public interface IShapeVisitor
    {
        void VisitCircle(Circle circle);
        void VisitRectangle(Rectangle rectangle);
    }
}