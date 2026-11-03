using GoFPatterns.Visitor.Shapes.VisitShapes;

namespace GoFPatterns.Visitor.Shapes
{
    // Besucher-Schnittstelle mit einer Operation pro Formtyp
    public interface IShapeVisitor
    {
        void VisitCircle(Circle circle);
        void VisitRectangle(Rectangle rectangle);
    }
}