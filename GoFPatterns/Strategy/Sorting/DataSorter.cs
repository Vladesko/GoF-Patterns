using System.Diagnostics;
using GoFPatterns.Strategy.Sorting.Strategies;

namespace GoFPatterns.Strategy.Sorting
{
    // Contexto: ordena los datos con la estrategia indicada o elige una estrategia por sí mismo.
    public class DataSorter
    {
        public TimeSpan Sort(int[] items, ISortStrategy strategy)
        {
            var stopwatch = Stopwatch.StartNew();
            strategy.Sort(items);
            return stopwatch.Elapsed;
        }

        // El contexto puede elegir la estrategia según los datos: el cliente no tiene que conocer los algoritmos.
        public static ISortStrategy ChooseFor(int[] items) =>
            items.Length < 10 ? new BubbleSortStrategy() : new BuiltInSortStrategy();
    }
}
