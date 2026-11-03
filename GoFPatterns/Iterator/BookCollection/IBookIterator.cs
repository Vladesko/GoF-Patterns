namespace GoFPatterns.Iterator.BookCollection
{
    // Iterator – klassische GoF-Schnittstelle zum Durchlaufen einer Sammlung.
    public interface IBookIterator
    {
        bool HasNext();
        Book Next();
        void Reset();
    }
}
