using GoFPatterns.Strategy.Sorting.Strategies;

namespace GoFPatterns.Strategy.Sorting
{
    public class DemonstrateSorting
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Strategy: algoritmos de ordenación ===\n");

            var random = new Random(1);
            var source = Enumerable.Range(0, 5_000).Select(_ => random.Next(100_000)).ToArray();

            ISortStrategy[] strategies = [new BubbleSortStrategy(), new QuickSortStrategy(), new BuiltInSortStrategy()];
            var sorter = new DataSorter();

            Console.WriteLine($"Ordenando {source.Length:N0} números:");
            foreach (var strategy in strategies)
            {
                var copy = (int[])source.Clone();
                var time = sorter.Sort(copy, strategy);
                var isSorted = copy.Zip(copy.Skip(1)).All(p => p.First <= p.Second);
                Console.WriteLine($"  {strategy.Name,-16} {time.TotalMilliseconds,8:F2} ms, ordenado: {isSorted}");
            }

            int[] small = [5, 3, 9, 1];
            var chosen = DataSorter.ChooseFor(small);
            sorter.Sort(small, chosen);
            Console.WriteLine($"\nArray pequeño -> {chosen.Name}: [{string.Join(", ", small)}]");

            // En C# una estrategia sencilla puede ser un delegado; por ejemplo, Comparison<T> para el orden de clasificación.
            string[] names = ["Olga", "Ivan", "Anastasia", "Petr"];
            Comparison<string> byLength = (a, b) => a.Length.CompareTo(b.Length);
            Array.Sort(names, byLength);
            Console.WriteLine($"Delegado como estrategia (por longitud): {string.Join(", ", names)}");
        }
    }
}
