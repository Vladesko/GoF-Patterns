namespace GoFPatterns.Iterator.BookCollection
{
    // Aggregate - collezione che sa creare gli iterator per se stessa.
    public interface IBookAggregate
    {
        IBookIterator CreateIterator();
        IBookIterator CreateReverseIterator();
        IBookIterator CreateAuthorIterator(string author);
    }
}
