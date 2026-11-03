namespace GoFPatterns.Flyweight.TextFormatting
{
    public class DemonstrateTextFormatting
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Легковес: форматирование текста ===\n");

            var document = new TextDocument();
            document.Append("Паттерны проектирования", "Arial", 18, bold: true);
            document.Append(" - это типовые решения часто встречающихся задач. ", "Arial", 12);
            document.Append("Легковес", "Arial", 12, bold: true, color: "blue");
            document.Append(" экономит память за счёт разделения состояния. ", "Arial", 12);
            document.Append("Примечание:", "Arial", 12, bold: true, color: "blue");
            document.Append(" стили используются повторно.", "Arial", 12);

            Console.WriteLine(document.Render());
            Console.WriteLine($"\nСимволов: {document.CharactersCount}, объектов стилей в пуле: {document.StylesCount}");
            Console.WriteLine($"Различных объектов стилей, на которые ссылаются символы: {document.CountDistinctStyleObjects()}");

            // .NET использует ту же идею для строк - интернирование строк.
            var first = string.Intern(new string(['f', 'l', 'y']));
            var second = string.Intern(new string(['f', 'l', 'y']));
            Console.WriteLine($"\nИнтернированные строки - один и тот же объект: {ReferenceEquals(first, second)}");
        }
    }
}
