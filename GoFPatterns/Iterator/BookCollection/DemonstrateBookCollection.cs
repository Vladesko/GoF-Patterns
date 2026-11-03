namespace GoFPatterns.Iterator.BookCollection
{
    public class DemonstrateBookCollection
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Iterator: Book collection ===\n");

            var library = new Library();
            library.Add(new Book("War and Peace", "Leo Tolstoy", 1869));
            library.Add(new Book("Crime and Punishment", "Fyodor Dostoevsky", 1866));
            library.Add(new Book("Anna Karenina", "Leo Tolstoy", 1878));
            library.Add(new Book("The Idiot", "Fyodor Dostoevsky", 1869));
            library.Add(new Book("Dead Souls", "Nikolai Gogol", 1842));

            // Client uses the same loop for any iterator.
            Print("Forward:", library.CreateIterator());
            Print("Reverse:", library.CreateReverseIterator());
            Print("Only Tolstoy:", library.CreateAuthorIterator("Leo Tolstoy"));

            // .NET way: IEnumerable<T> + foreach + LINQ.
            Console.WriteLine("foreach + LINQ (books before 1869, by year):");
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
