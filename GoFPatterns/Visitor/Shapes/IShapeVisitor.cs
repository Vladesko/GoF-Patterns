using GoFPatterns.Visitor.Shapes.VisitShapes;

namespace GoFPatterns.Visitor.Shapes
{
    // Интерфейс посетителя, определяющий операции для каждого типа фигуры
    public interface IShapeVisitor
    {
        void VisitCircle(Circle circle);
        void VisitRectangle(Rectangle rectangle);
    }
}
