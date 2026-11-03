namespace GoFPatterns.Iterator.BookCollection.Iterators
{
    // Concrete iterator - returns only books of the given author.
    public class AuthorFilterIterator(Library library, string author) : IBookIterator
    {
        private int _position;

        public bool HasNext()
        {
            // Skip books of other authors.
            while (_position < library.Count && library.GetAt(_position).Author != author)
            {
                _position++;
            }
            return _position < library.Count;
        }

        public Book Next() =>
            HasNext() ? library.GetAt(_position++) : throw new InvalidOperationException("No more books.");

        public void Reset() => _position = 0;
    }
}
