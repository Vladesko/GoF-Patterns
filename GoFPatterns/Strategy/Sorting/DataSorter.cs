using System.Diagnostics;
using GoFPatterns.Strategy.Sorting.Strategies;

namespace GoFPatterns.Strategy.Sorting
{
    // Kontext – sortiert Daten mit der angegebenen Strategie oder wählt selbst eine Strategie.
    public class DataSorter
    {
        public TimeSpan Sort(int[] items, ISortStrategy strategy)
        {
            var stopwatch = Stopwatch.StartNew();
            strategy.Sort(items);
            return stopwatch.Elapsed;
        }

        // Der Kontext kann die Strategie abhängig von den Daten wählen – der Client muss die Algorithmen nicht kennen.
        public static ISortStrategy ChooseFor(int[] items) =>
            items.Length < 10 ? new BubbleSortStrategy() : new BuiltInSortStrategy();
    }
}
