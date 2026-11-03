namespace GoFPatterns.Iterator.BookCollection
{
    public class DemonstrateBookCollection
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Iterator: Büchersammlung ===\n");

            var library = new Library();
            library.Add(new Book("Krieg und Frieden", "Lew Tolstoi", 1869));
            library.Add(new Book("Schuld und Sühne", "Fjodor Dostojewski", 1866));
            library.Add(new Book("Anna Karenina", "Lew Tolstoi", 1878));
            library.Add(new Book("Der Idiot", "Fjodor Dostojewski", 1869));
            library.Add(new Book("Die toten Seelen", "Nikolai Gogol", 1842));

            // Der Client verwendet für jeden Iterator dieselbe Schleife.
            Print("Vorwärts:", library.CreateIterator());
            Print("Rückwärts:", library.CreateReverseIterator());
            Print("Nur Tolstoi:", library.CreateAuthorIterator("Lew Tolstoi"));

            // Der .NET-Weg: IEnumerable<T> + foreach + LINQ.
            Console.WriteLine("foreach + LINQ (Bücher vor 1869, nach Jahr):");
            foreach (var book in library.Where(b => b.Year < 1869).OrderBy(b => b.Year))
            {
                Console.WriteLine($"  {book.Year} {book.Title}");
            }
        }

        private static void Print(string caption, IBookIterator iterator)
        {
            Console.WriteLine(caption);
            while (iterator.HasNext())
            {
                var book = iterator.Next();
                Console.WriteLine($"  {book.Title} - {book.Author}");
            }
            Console.WriteLine();
        }
    }
}
