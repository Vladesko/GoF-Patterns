namespace GoFPatterns.Strategy.Sorting.Strategies
{
    // Конкретная стратегия - адаптер к встроенному алгоритму .NET (introsort).
    public class BuiltInSortStrategy : ISortStrategy
    {
        public string Name => "Array.Sort";

        public void Sort(int[] items) => Array.Sort(items);
    }
}
