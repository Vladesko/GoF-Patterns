namespace GoFPatterns.Visitor.Shapes
{
    public class ShapeDemostrateVisitor
    {
        public void Demonstrate()
        {
            var shapes = new List<IShape>
            {
                new VisitShapes.Circle { Radius = 5 },
                new VisitShapes.Rectangle { Width = 4, Height = 6 }
            };
            var areaVisitor = new Visiotrs.AreaCalculatorVisitor();
            var xmlVisitor = new Visiotrs.XmlExportVisitor();
            foreach (var shape in shapes)
            {
                shape.Accept(areaVisitor);
                shape.Accept(xmlVisitor);
            }
            Console.WriteLine($"Total Area: {areaVisitor.TotalArea}");
            Console.WriteLine("XML Export:");
            Console.WriteLine(xmlVisitor.GetXml());
        }
    }
}
