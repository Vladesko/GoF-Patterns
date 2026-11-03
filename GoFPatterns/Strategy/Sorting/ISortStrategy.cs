namespace GoFPatterns.Strategy.Sorting
{
    // Strategy - sorting algorithm.
    public interface ISortStrategy
    {
        string Name { get; }
        void Sort(int[] items);
    }
}
