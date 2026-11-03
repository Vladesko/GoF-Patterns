namespace GoFPatterns.Strategy.Sorting.Strategies
{
    // Estrategia concreta: ordenación rápida (quicksort), O(n log n) de media.
    public class QuickSortStrategy : ISortStrategy
    {
        public string Name => "Quicksort";

        public void Sort(int[] items) => QuickSort(items, 0, items.Length - 1);

        private static void QuickSort(int[] items, int left, int right)
        {
            if (left >= right) return;

            var pivot = items[(left + right) / 2];
            int i = left, j = right;
            while (i <= j)
            {
                while (items[i] < pivot) i++;
                while (items[j] > pivot) j--;
                if (i <= j)
                {
                    (items[i], items[j]) = (items[j], items[i]);
                    i++;
                    j--;
                }
            }
            QuickSort(items, left, j);
            QuickSort(items, i, right);
        }
    }
}
