namespace GoFPatterns.Strategy.Sorting
{
    // Strategy - algoritmo di ordinamento.
    public interface ISortStrategy
    {
        string Name { get; }
        void Sort(int[] items);
    }
}
