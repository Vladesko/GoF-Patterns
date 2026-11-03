namespace GoFPatterns.Flyweight.TextFormatting
{
    public class DemonstrateTextFormatting
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Flyweight: formato de texto ===\n");

            var document = new TextDocument();
            document.Append("Los patrones de diseño", "Arial", 18, bold: true);
            document.Append(" son soluciones típicas a problemas comunes. ", "Arial", 12);
            document.Append("Flyweight", "Arial", 12, bold: true, color: "azul");
            document.Append(" ahorra memoria compartiendo el estado. ", "Arial", 12);
            document.Append("Nota:", "Arial", 12, bold: true, color: "azul");
            document.Append(" los estilos se reutilizan.", "Arial", 12);

            Console.WriteLine(document.Render());
            Console.WriteLine($"\nCaracteres: {document.CharactersCount}, objetos de estilo en el pool: {document.StylesCount}");
            Console.WriteLine($"Objetos de estilo distintos referenciados por los caracteres: {document.CountDistinctStyleObjects()}");

            // .NET usa la misma idea para las cadenas: el internado de cadenas (string interning).
            var first = string.Intern(new string(['f', 'l', 'y']));
            var second = string.Intern(new string(['f', 'l', 'y']));
            Console.WriteLine($"\nLas cadenas internadas son el mismo objeto: {ReferenceEquals(first, second)}");
        }
    }
}
