namespace GoFPatterns.Strategy.Sorting.Strategies
{
    // Konkrete Strategie – Adapter zum eingebauten .NET-Algorithmus (Introsort).
    public class BuiltInSortStrategy : ISortStrategy
    {
        public string Name => "Array.Sort";

        public void Sort(int[] items) => Array.Sort(items);
    }
}
