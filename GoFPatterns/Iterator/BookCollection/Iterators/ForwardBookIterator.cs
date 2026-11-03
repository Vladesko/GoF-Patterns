namespace GoFPatterns.Iterator.BookCollection.Iterators
{
    // Iterator concreto - va dal primo libro all'ultimo.
    // Mantiene una propria posizione, così più iterator possono attraversare la stessa biblioteca in modo indipendente.
    public class ForwardBookIterator(Library library) : IBookIterator
    {
        private int _position;

        public bool HasNext() => _position < library.Count;

        public Book Next() =>
            HasNext() ? library.GetAt(_position++) : throw new InvalidOperationException("Non ci sono altri libri.");

        public void Reset() => _position = 0;
    }
}
