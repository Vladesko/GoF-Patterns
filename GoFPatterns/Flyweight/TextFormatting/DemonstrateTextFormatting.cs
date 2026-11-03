namespace GoFPatterns.Flyweight.TextFormatting
{
    public class DemonstrateTextFormatting
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Flyweight: Formattazione del testo ===\n");

            var document = new TextDocument();
            document.Append("I design pattern", "Arial", 18, bold: true);
            document.Append(" sono soluzioni tipiche a problemi comuni. ", "Arial", 12);
            document.Append("Flyweight", "Arial", 12, bold: true, color: "blu");
            document.Append(" risparmia memoria condividendo lo stato. ", "Arial", 12);
            document.Append("Nota:", "Arial", 12, bold: true, color: "blu");
            document.Append(" gli stili vengono riutilizzati.", "Arial", 12);

            Console.WriteLine(document.Render());
            Console.WriteLine($"\nCaratteri: {document.CharactersCount}, oggetti stile nel pool: {document.StylesCount}");
            Console.WriteLine($"Oggetti stile distinti referenziati dai caratteri: {document.CountDistinctStyleObjects()}");

            // .NET usa la stessa idea per le stringhe - lo string interning.
            var first = string.Intern(new string(['f', 'l', 'y']));
            var second = string.Intern(new string(['f', 'l', 'y']));
            Console.WriteLine($"\nLe stringhe internate sono lo stesso oggetto: {ReferenceEquals(first, second)}");
        }
    }
}
