namespace GoFPatterns.Strategy.Sorting.Strategies
{
    // Konkrete Strategie – einfach, aber O(n^2). Nur für sehr kleine Arrays geeignet.
    public class BubbleSortStrategy : ISortStrategy
    {
        public string Name => "Bubblesort";

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
