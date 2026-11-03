using System.Diagnostics;
using GoFPatterns.Strategy.Sorting.Strategies;

namespace GoFPatterns.Strategy.Sorting
{
    // Context - sorts data with the given strategy or chooses a strategy by itself.
    public class DataSorter
    {
        public TimeSpan Sort(int[] items, ISortStrategy strategy)
        {
            var stopwatch = Stopwatch.StartNew();
            strategy.Sort(items);
            return stopwatch.Elapsed;
        }

        // Context can pick strategy depending on data - client doesn't have to know algorithms.
        public static ISortStrategy ChooseFor(int[] items) =>
            items.Length < 10 ? new BubbleSortStrategy() : new BuiltInSortStrategy();
    }
}
