namespace GoFPatterns.Iterator.BookCollection
{
    // Iterator - classic GoF interface for traversing a collection.
    public interface IBookIterator
    {
        bool HasNext();
        Book Next();
        void Reset();
    }
}
