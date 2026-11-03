namespace GoFPatterns.Iterator.BookCollection.Iterators
{
    // Concrete iterator - another traversal algorithm for the same collection.
    public class ReverseBookIterator(Library library) : IBookIterator
    {
        private int _position = library.Count - 1;

        public bool HasNext() => _position >= 0;

        public Book Next() =>
            HasNext() ? library.GetAt(_position--) : throw new InvalidOperationException("No more books.");

        public void Reset() => _position = library.Count - 1;
    }
}
