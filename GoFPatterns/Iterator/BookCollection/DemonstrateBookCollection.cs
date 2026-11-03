namespace GoFPatterns.Iterator.BookCollection
{
    public class DemonstrateBookCollection
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Iterator: colección de libros ===\n");

            var library = new Library();
            library.Add(new Book("Guerra y paz", "León Tolstói", 1869));
            library.Add(new Book("Crimen y castigo", "Fiódor Dostoievski", 1866));
            library.Add(new Book("Anna Karénina", "León Tolstói", 1878));
            library.Add(new Book("El idiota", "Fiódor Dostoievski", 1869));
            library.Add(new Book("Almas muertas", "Nikolái Gógol", 1842));

            // El cliente usa el mismo bucle para cualquier iterador.
            Print("Hacia delante:", library.CreateIterator());
            Print("En orden inverso:", library.CreateReverseIterator());
            Print("Solo Tolstói:", library.CreateAuthorIterator("León Tolstói"));

            // Al estilo .NET: IEnumerable<T> + foreach + LINQ.
            Console.WriteLine("foreach + LINQ (libros anteriores a 1869, por año):");
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
