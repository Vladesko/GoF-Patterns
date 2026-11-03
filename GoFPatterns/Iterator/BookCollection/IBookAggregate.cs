namespace GoFPatterns.Iterator.BookCollection
{
    // Aggregat – Sammlung, die Iteratoren für sich selbst erzeugen kann.
    public interface IBookAggregate
    {
        IBookIterator CreateIterator();
        IBookIterator CreateReverseIterator();
        IBookIterator CreateAuthorIterator(string author);
    }
}
