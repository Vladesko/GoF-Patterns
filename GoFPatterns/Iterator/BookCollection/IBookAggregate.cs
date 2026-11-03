namespace GoFPatterns.Iterator.BookCollection
{
    // Агрегат - коллекция, которая умеет создавать итераторы для самой себя.
    public interface IBookAggregate
    {
        IBookIterator CreateIterator();
        IBookIterator CreateReverseIterator();
        IBookIterator CreateAuthorIterator(string author);
    }
}
