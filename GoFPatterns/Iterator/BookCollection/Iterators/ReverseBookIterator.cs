namespace GoFPatterns.Iterator.BookCollection.Iterators
{
    // Iterator concreto - un altro algoritmo di attraversamento per la stessa collezione.
    public class ReverseBookIterator(Library library) : IBookIterator
    {
        private int _position = library.Count - 1;

        public bool HasNext() => _position >= 0;

        public Book Next() =>
            HasNext() ? library.GetAt(_position--) : throw new InvalidOperationException("Non ci sono altri libri.");

        public void Reset() => _position = library.Count - 1;
    }
}
