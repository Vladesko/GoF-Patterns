namespace GoFPatterns.Iterator.BookCollection
{
    // Aggregate - collection which can create iterators for itself.
    public interface IBookAggregate
    {
        IBookIterator CreateIterator();
        IBookIterator CreateReverseIterator();
        IBookIterator CreateAuthorIterator(string author);
    }
}
