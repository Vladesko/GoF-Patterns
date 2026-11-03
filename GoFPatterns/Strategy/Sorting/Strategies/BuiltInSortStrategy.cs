namespace GoFPatterns.Strategy.Sorting.Strategies
{
    // Estrategia concreta: adaptador al algoritmo integrado de .NET (introsort).
    public class BuiltInSortStrategy : ISortStrategy
    {
        public string Name => "Array.Sort";

        public void Sort(int[] items) => Array.Sort(items);
    }
}
