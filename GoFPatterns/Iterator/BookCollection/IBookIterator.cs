namespace GoFPatterns.Iterator.BookCollection
{
    // Итератор - классический интерфейс GoF для обхода коллекции.
    public interface IBookIterator
    {
        bool HasNext();
        Book Next();
        void Reset();
    }
}
