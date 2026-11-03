namespace GoFPatterns.Strategy.Sorting.Strategies
{
    // Concrete strategy - adapter to the built-in .NET algorithm (introsort).
    public class BuiltInSortStrategy : ISortStrategy
    {
        public string Name => "Array.Sort";

        public void Sort(int[] items) => Array.Sort(items);
    }
}
