namespace GoFPatterns.Iterator.BookCollection.Iterators
{
    // Konkreter Iterator – ein anderer Durchlaufalgorithmus für dieselbe Sammlung.
    public class ReverseBookIterator(Library library) : IBookIterator
    {
        private int _position = library.Count - 1;

        public bool HasNext() => _position >= 0;

        public Book Next() =>
            HasNext() ? library.GetAt(_position--) : throw new InvalidOperationException("Keine weiteren Bücher.");

        public void Reset() => _position = library.Count - 1;
    }
}
