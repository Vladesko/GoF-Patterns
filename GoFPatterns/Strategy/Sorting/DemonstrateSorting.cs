using GoFPatterns.Strategy.Sorting.Strategies;

namespace GoFPatterns.Strategy.Sorting
{
    public class DemonstrateSorting
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Стратегия: алгоритмы сортировки ===\n");

            var random = new Random(1);
            var source = Enumerable.Range(0, 5_000).Select(_ => random.Next(100_000)).ToArray();

            ISortStrategy[] strategies = [new BubbleSortStrategy(), new QuickSortStrategy(), new BuiltInSortStrategy()];
            var sorter = new DataSorter();

            Console.WriteLine($"Сортировка {source.Length:N0} чисел:");
            foreach (var strategy in strategies)
            {
                var copy = (int[])source.Clone();
                var time = sorter.Sort(copy, strategy);
                var isSorted = copy.Zip(copy.Skip(1)).All(p => p.First <= p.Second);
                Console.WriteLine($"  {strategy.Name,-12} {time.TotalMilliseconds,8:F2} мс, отсортировано: {isSorted}");
            }

            int[] small = [5, 3, 9, 1];
            var chosen = DataSorter.ChooseFor(small);
            sorter.Sort(small, chosen);
            Console.WriteLine($"\nМаленький массив -> {chosen.Name}: [{string.Join(", ", small)}]");

            // В C# простая стратегия может быть делегатом - например, Comparison<T> для порядка сортировки.
            string[] names = ["Ольга", "Иван", "Анастасия", "Пётр"];
            Comparison<string> byLength = (a, b) => a.Length.CompareTo(b.Length);
            Array.Sort(names, byLength);
            Console.WriteLine($"Делегат как стратегия (по длине): {string.Join(", ", names)}");
        }
    }
}
