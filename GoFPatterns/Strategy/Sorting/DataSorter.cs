using System.Diagnostics;
using GoFPatterns.Strategy.Sorting.Strategies;

namespace GoFPatterns.Strategy.Sorting
{
    // Контекст - сортирует данные заданной стратегией или сам выбирает стратегию.
    public class DataSorter
    {
        public TimeSpan Sort(int[] items, ISortStrategy strategy)
        {
            var stopwatch = Stopwatch.StartNew();
            strategy.Sort(items);
            return stopwatch.Elapsed;
        }

        // Контекст может выбрать стратегию в зависимости от данных - клиенту не нужно знать алгоритмы.
        public static ISortStrategy ChooseFor(int[] items) =>
            items.Length < 10 ? new BubbleSortStrategy() : new BuiltInSortStrategy();
    }
}
