namespace GoFPatterns.Iterator.BookCollection.Iterators
{
    // Konkreter Iterator – liefert nur die Bücher des angegebenen Autors.
    public class AuthorFilterIterator(Library library, string author) : IBookIterator
    {
        private int _position;

        public bool HasNext()
        {
            // Bücher anderer Autoren überspringen.
            while (_position < library.Count && library.GetAt(_position).Author != author)
            {
                _position++;
            }
            return _position < library.Count;
        }

        public Book Next() =>
            HasNext() ? library.GetAt(_position++) : throw new InvalidOperationException("Keine weiteren Bücher.");

        public void Reset() => _position = 0;
    }
}
