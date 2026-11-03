namespace GoFPatterns.Iterator.BookCollection
{
    // Iterador: interfaz GoF clásica para recorrer una colección.
    public interface IBookIterator
    {
        bool HasNext();
        Book Next();
        void Reset();
    }
}
