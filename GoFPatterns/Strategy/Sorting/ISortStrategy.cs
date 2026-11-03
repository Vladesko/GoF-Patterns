namespace GoFPatterns.Strategy.Sorting
{
    // Estrategia: algoritmo de ordenación.
    public interface ISortStrategy
    {
        string Name { get; }
        void Sort(int[] items);
    }
}
