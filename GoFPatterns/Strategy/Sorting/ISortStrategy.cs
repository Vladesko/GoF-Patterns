namespace GoFPatterns.Strategy.Sorting
{
    // Стратегия - алгоритм сортировки.
    public interface ISortStrategy
    {
        string Name { get; }
        void Sort(int[] items);
    }
}
