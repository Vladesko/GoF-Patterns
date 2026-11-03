namespace GoFPatterns.Iterator.Tree
{
    // Агрегат - дерево категорий каталога. Внутренняя структура (список дочерних узлов) скрыта,
    // клиенты обходят дерево только через итераторы.
    public class CategoryNode(string name)
    {
        private readonly List<CategoryNode> _children = [];

        public string Name { get; } = name;

        public CategoryNode Add(params CategoryNode[] children)
        {
            _children.AddRange(children);
            return this;
        }

        // Итератор 1: обход в глубину. Возвращает узел вместе с его глубиной.
        // Вместо рекурсии используется явный стек, поэтому глубокие деревья не вызывают StackOverflow.
        public IEnumerable<(CategoryNode Node, int Depth)> DepthFirst()
        {
            var stack = new Stack<(CategoryNode, int)>();
            stack.Push((this, 0));
            while (stack.Count > 0)
            {
                var (node, depth) = stack.Pop();
                yield return (node, depth);

                // Кладём дочерние узлы в обратном порядке, чтобы первый дочерний узел обрабатывался первым.
                for (var i = node._children.Count - 1; i >= 0; i--)
                {
                    stack.Push((node._children[i], depth + 1));
                }
            }
        }

        // Итератор 2: обход в ширину (уровень за уровнем).
        public IEnumerable<(CategoryNode Node, int Depth)> BreadthFirst()
        {
            var queue = new Queue<(CategoryNode, int)>();
            queue.Enqueue((this, 0));
            while (queue.Count > 0)
            {
                var (node, depth) = queue.Dequeue();
                yield return (node, depth);

                foreach (var child in node._children)
                {
                    queue.Enqueue((child, depth + 1));
                }
            }
        }
    }
}
