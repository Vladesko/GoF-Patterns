namespace GoFPatterns.Iterator.BookCollection
{
    public class DemonstrateBookCollection
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Итератор: коллекция книг ===\n");

            var library = new Library();
            library.Add(new Book("Война и мир", "Лев Толстой", 1869));
            library.Add(new Book("Преступление и наказание", "Фёдор Достоевский", 1866));
            library.Add(new Book("Анна Каренина", "Лев Толстой", 1878));
            library.Add(new Book("Идиот", "Фёдор Достоевский", 1869));
            library.Add(new Book("Мёртвые души", "Николай Гоголь", 1842));

            // Клиент использует один и тот же цикл для любого итератора.
            Print("Прямой порядок:", library.CreateIterator());
            Print("Обратный порядок:", library.CreateReverseIterator());
            Print("Только Толстой:", library.CreateAuthorIterator("Лев Толстой"));

            // Способ .NET: IEnumerable<T> + foreach + LINQ.
            Console.WriteLine("foreach + LINQ (книги до 1869 года, по году):");
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
