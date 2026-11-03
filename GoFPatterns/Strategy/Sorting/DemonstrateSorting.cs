using GoFPatterns.Strategy.Sorting.Strategies;

namespace GoFPatterns.Strategy.Sorting
{
    public class DemonstrateSorting
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Strategy: Sortieralgorithmen ===\n");

            var random = new Random(1);
            var source = Enumerable.Range(0, 5_000).Select(_ => random.Next(100_000)).ToArray();

            ISortStrategy[] strategies = [new BubbleSortStrategy(), new QuickSortStrategy(), new BuiltInSortStrategy()];
            var sorter = new DataSorter();

            Console.WriteLine($"Sortieren von {source.Length:N0} Zahlen:");
            foreach (var strategy in strategies)
            {
                var copy = (int[])source.Clone();
                var time = sorter.Sort(copy, strategy);
                var isSorted = copy.Zip(copy.Skip(1)).All(p => p.First <= p.Second);
                Console.WriteLine($"  {strategy.Name,-12} {time.TotalMilliseconds,8:F2} ms, sortiert: {isSorted}");
            }

            int[] small = [5, 3, 9, 1];
            var chosen = DataSorter.ChooseFor(small);
            sorter.Sort(small, chosen);
            Console.WriteLine($"\nKleines Array -> {chosen.Name}: [{string.Join(", ", small)}]");

            // In C# kann eine einfache Strategie ein Delegat sein – z. B. Comparison<T> für die Sortierreihenfolge.
            string[] names = ["Olga", "Ivan", "Anastasia", "Petr"];
            Comparison<string> byLength = (a, b) => a.Length.CompareTo(b.Length);
            Array.Sort(names, byLength);
            Console.WriteLine($"Delegat als Strategie (nach Länge): {string.Join(", ", names)}");
        }
    }
}
