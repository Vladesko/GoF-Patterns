namespace GoFPatterns.Iterator.BookCollection
{
    // Iterator - interfaccia GoF classica per attraversare una collezione.
    public interface IBookIterator
    {
        bool HasNext();
        Book Next();
        void Reset();
    }
}
