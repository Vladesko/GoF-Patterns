namespace GoFPatterns.Flyweight.TextFormatting
{
    public class DemonstrateTextFormatting
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Flyweight: Text formatting ===\n");

            var document = new TextDocument();
            document.Append("Design Patterns", "Arial", 18, bold: true);
            document.Append(" are typical solutions to common problems. ", "Arial", 12);
            document.Append("Flyweight", "Arial", 12, bold: true, color: "blue");
            document.Append(" saves memory by sharing state. ", "Arial", 12);
            document.Append("Note:", "Arial", 12, bold: true, color: "blue");
            document.Append(" styles are reused.", "Arial", 12);

            Console.WriteLine(document.Render());
            Console.WriteLine($"\nCharacters: {document.CharactersCount}, style objects in pool: {document.StylesCount}");
            Console.WriteLine($"Distinct style objects referenced by characters: {document.CountDistinctStyleObjects()}");

            // .NET uses the same idea for strings - string interning.
            var first = string.Intern(new string(['f', 'l', 'y']));
            var second = string.Intern(new string(['f', 'l', 'y']));
            Console.WriteLine($"\nInterned strings are the same object: {ReferenceEquals(first, second)}");
        }
    }
}
