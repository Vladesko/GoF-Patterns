namespace GoFPatterns.Strategy.Sorting.Strategies
{
    // Estrategia concreta: sencilla, pero O(n^2). Solo sirve para arrays muy pequeños.
    public class BubbleSortStrategy : ISortStrategy
    {
        public string Name => "Burbuja";

        public void Sort(int[] items)
        {
            for (var i = 0; i < items.Length - 1; i++)
            {
                for (var j = 0; j < items.Length - i - 1; j++)
                {
                    if (items[j] > items[j + 1])
                        (items[j], items[j + 1]) = (items[j + 1], items[j]);
                }
            }
        }
    }
}
