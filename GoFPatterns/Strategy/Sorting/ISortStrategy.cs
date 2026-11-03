namespace GoFPatterns.Strategy.Sorting
{
    // Strategie – Sortieralgorithmus.
    public interface ISortStrategy
    {
        string Name { get; }
        void Sort(int[] items);
    }
}
