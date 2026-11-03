namespace GoFPatterns.Iterator.BookCollection.Iterators
{
    // Конкретный итератор - другой алгоритм обхода той же коллекции.
    public class ReverseBookIterator(Library library) : IBookIterator
    {
        private int _position = library.Count - 1;

        public bool HasNext() => _position >= 0;

        public Book Next() =>
            HasNext() ? library.GetAt(_position--) : throw new InvalidOperationException("Книг больше нет.");

        public void Reset() => _position = library.Count - 1;
    }
}
