namespace GoFPatterns.Iterator.BookCollection.Iterators
{
    // Concrete iterator - goes from the first book to the last.
    // Keeps its own position, so several iterators can traverse the same library independently.
    public class ForwardBookIterator(Library library) : IBookIterator
    {
        private int _position;

        public bool HasNext() => _position < library.Count;

        public Book Next() =>
            HasNext() ? library.GetAt(_position++) : throw new InvalidOperationException("No more books.");

        public void Reset() => _position = 0;
    }
}
