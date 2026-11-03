namespace GoFPatterns.Iterator.BookCollection.Iterators
{
    // Konkreter Iterator – läuft vom ersten bis zum letzten Buch.
    // Er hält seine eigene Position, sodass mehrere Iteratoren dieselbe Bibliothek unabhängig durchlaufen können.
    public class ForwardBookIterator(Library library) : IBookIterator
    {
        private int _position;

        public bool HasNext() => _position < library.Count;

        public Book Next() =>
            HasNext() ? library.GetAt(_position++) : throw new InvalidOperationException("Keine weiteren Bücher.");

        public void Reset() => _position = 0;
    }
}
