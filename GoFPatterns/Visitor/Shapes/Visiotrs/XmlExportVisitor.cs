using GoFPatterns.Visitor.Shapes.VisitShapes;
using System.Text;

namespace GoFPatterns.Visitor.Shapes.Visiotrs
{
    internal class XmlExportVisitor : IShapeVisitor
    {
        private StringBuilder xml = new StringBuilder();

        public void VisitCircle(Circle circle) =>
            xml.AppendLine($"<Circle radius=\"{circle.Radius}\" />");
        
        public void VisitRectangle(Rectangle rectangle) =>
            xml.AppendLine($"<Rectangle width=\"{rectangle.Width}\" height=\"{rectangle.Height}\" />");

        public string GetXml() => xml.ToString();
    }
}