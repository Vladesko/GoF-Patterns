namespace GoFPatterns.Iterator.BookCollection
{
    public class DemonstrateBookCollection
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Iterator: Collezione di libri ===\n");

            var library = new Library();
            library.Add(new Book("Guerra e pace", "Lev Tolstoj", 1869));
            library.Add(new Book("Delitto e castigo", "Fëdor Dostoevskij", 1866));
            library.Add(new Book("Anna Karenina", "Lev Tolstoj", 1878));
            library.Add(new Book("L'idiota", "Fëdor Dostoevskij", 1869));
            library.Add(new Book("Le anime morte", "Nikolaj Gogol'", 1842));

            // Il client usa lo stesso ciclo per qualsiasi iterator.
            Print("In avanti:", library.CreateIterator());
            Print("All'indietro:", library.CreateReverseIterator());
            Print("Solo Tolstoj:", library.CreateAuthorIterator("Lev Tolstoj"));

            // Il modo .NET: IEnumerable<T> + foreach + LINQ.
            Console.WriteLine("foreach + LINQ (libri prima del 1869, per anno):");
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
