using System.Diagnostics;
using GoFPatterns.Strategy.Sorting.Strategies;

namespace GoFPatterns.Strategy.Sorting
{
    // Context - ordina i dati con la strategy indicata oppure sceglie da solo una strategy.
    public class DataSorter
    {
        public TimeSpan Sort(int[] items, ISortStrategy strategy)
        {
            var stopwatch = Stopwatch.StartNew();
            strategy.Sort(items);
            return stopwatch.Elapsed;
        }

        // Il context può scegliere la strategy in base ai dati - il client non deve conoscere gli algoritmi.
        public static ISortStrategy ChooseFor(int[] items) =>
            items.Length < 10 ? new BubbleSortStrategy() : new BuiltInSortStrategy();
    }
}
