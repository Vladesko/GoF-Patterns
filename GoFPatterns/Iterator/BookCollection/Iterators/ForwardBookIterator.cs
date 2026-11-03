namespace GoFPatterns.Iterator.BookCollection.Iterators
{
    // Конкретный итератор - идёт от первой книги к последней.
    // Хранит собственную позицию, поэтому несколько итераторов могут обходить одну библиотеку независимо.
    public class ForwardBookIterator(Library library) : IBookIterator
    {
        private int _position;

        public bool HasNext() => _position < library.Count;

        public Book Next() =>
            HasNext() ? library.GetAt(_position++) : throw new InvalidOperationException("Книг больше нет.");

        public void Reset() => _position = 0;
    }
}
