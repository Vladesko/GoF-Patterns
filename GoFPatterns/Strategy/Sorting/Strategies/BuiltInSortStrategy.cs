namespace GoFPatterns.Strategy.Sorting.Strategies
{
    // Strategy concreta - adapter verso l'algoritmo integrato di .NET (introsort).
    public class BuiltInSortStrategy : ISortStrategy
    {
        public string Name => "Array.Sort";

        public void Sort(int[] items) => Array.Sort(items);
    }
}
