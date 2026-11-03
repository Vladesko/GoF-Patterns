namespace GoFPatterns.Iterator.BookCollection.Iterators
{
    // Iterator concreto - restituisce solo i libri dell'autore indicato.
    public class AuthorFilterIterator(Library library, string author) : IBookIterator
    {
        private int _position;

        public bool HasNext()
        {
            // Salta i libri degli altri autori.
            while (_position < library.Count && library.GetAt(_position).Author != author)
            {
                _position++;
            }
            return _position < library.Count;
        }

        public Book Next() =>
            HasNext() ? library.GetAt(_position++) : throw new InvalidOperationException("Non ci sono altri libri.");

        public void Reset() => _position = 0;
    }
}
