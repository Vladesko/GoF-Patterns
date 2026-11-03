namespace GoFPatterns.Flyweight.TextFormatting
{
    public class DemonstrateTextFormatting
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Flyweight: Textformatierung ===\n");

            var document = new TextDocument();
            document.Append("Entwurfsmuster", "Arial", 18, bold: true);
            document.Append(" sind typische Lösungen für häufige Probleme. ", "Arial", 12);
            document.Append("Flyweight", "Arial", 12, bold: true, color: "blau");
            document.Append(" spart Speicher durch gemeinsam genutzten Zustand. ", "Arial", 12);
            document.Append("Hinweis:", "Arial", 12, bold: true, color: "blau");
            document.Append(" Stile werden wiederverwendet.", "Arial", 12);

            Console.WriteLine(document.Render());
            Console.WriteLine($"\nZeichen: {document.CharactersCount}, Stilobjekte im Pool: {document.StylesCount}");
            Console.WriteLine($"Verschiedene Stilobjekte, auf die Zeichen verweisen: {document.CountDistinctStyleObjects()}");

            // .NET nutzt dieselbe Idee für Zeichenketten – String-Interning.
            var first = string.Intern(new string(['f', 'l', 'y']));
            var second = string.Intern(new string(['f', 'l', 'y']));
            Console.WriteLine($"\nInternierte Strings sind dasselbe Objekt: {ReferenceEquals(first, second)}");
        }
    }
}
