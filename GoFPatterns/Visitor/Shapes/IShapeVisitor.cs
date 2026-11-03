using GoFPatterns.Visitor.Shapes.VisitShapes;

namespace GoFPatterns.Visitor.Shapes
{
    // Interfaz del visitante que define las operaciones para cada tipo de figura
    public interface IShapeVisitor
    {
        void VisitCircle(Circle circle);
        void VisitRectangle(Rectangle rectangle);
    }
}
