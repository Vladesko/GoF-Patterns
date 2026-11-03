namespace GoFPatterns.Iterator.Tree
{
    // Aggregat – Baum der Katalogkategorien. Die interne Struktur (Liste der Kinder) ist verborgen,
    // Clients durchlaufen den Baum nur über Iteratoren.
    public class CategoryNode(string name)
    {
        private readonly List<CategoryNode> _children = [];

        public string Name { get; } = name;

        public CategoryNode Add(params CategoryNode[] children)
        {
            _children.AddRange(children);
            return this;
        }

        // Iterator 1: Tiefensuche. Liefert den Knoten samt seiner Tiefe.
        // Statt Rekursion wird ein expliziter Stack verwendet, damit tiefe Bäume keinen StackOverflow auslösen.
        public IEnumerable<(CategoryNode Node, int Depth)> DepthFirst()
        {
            var stack = new Stack<(CategoryNode, int)>();
            stack.Push((this, 0));
            while (stack.Count > 0)
            {
                var (node, depth) = stack.Pop();
                yield return (node, depth);

                // Kinder in umgekehrter Reihenfolge ablegen, damit das erste Kind zuerst verarbeitet wird.
                for (var i = node._children.Count - 1; i >= 0; i--)
                {
                    stack.Push((node._children[i], depth + 1));
                }
            }
        }

        // Iterator 2: Breitensuche (Ebene für Ebene).
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
