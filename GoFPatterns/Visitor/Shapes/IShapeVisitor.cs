using GoFPatterns.Visitor.Shapes.VisitShapes;

namespace GoFPatterns.Visitor.Shapes
{
    // Interfaccia Visitor che definisce le operazioni per ogni tipo di forma
    public interface IShapeVisitor
    {
        void VisitCircle(Circle circle);
        void VisitRectangle(Rectangle rectangle);
    }
}