namespace GoFPatterns.Iterator.BookCollection
{
    // Agregado: colección que puede crear iteradores para sí misma.
    public interface IBookAggregate
    {
        IBookIterator CreateIterator();
        IBookIterator CreateReverseIterator();
        IBookIterator CreateAuthorIterator(string author);
    }
}
