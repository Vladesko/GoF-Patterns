namespace GoFPatterns.Iterator.Tree
{
    // Aggregate - albero delle categorie del catalogo. La struttura interna (lista dei figli) è nascosta,
    // i client attraversano l'albero solo tramite gli iterator.
    public class CategoryNode(string name)
    {
        private readonly List<CategoryNode> _children = [];

        public string Name { get; } = name;

        public CategoryNode Add(params CategoryNode[] children)
        {
            _children.AddRange(children);
            return this;
        }

        // Iterator 1: attraversamento in profondità. Restituisce il nodo con la sua profondità.
        // Si usa uno stack esplicito invece della ricorsione, così gli alberi profondi non causano StackOverflow.
        public IEnumerable<(CategoryNode Node, int Depth)> DepthFirst()
        {
            var stack = new Stack<(CategoryNode, int)>();
            stack.Push((this, 0));
            while (stack.Count > 0)
            {
                var (node, depth) = stack.Pop();
                yield return (node, depth);

                // Inserisce i figli in ordine inverso, così il primo figlio viene elaborato per primo.
                for (var i = node._children.Count - 1; i >= 0; i--)
                {
                    stack.Push((node._children[i], depth + 1));
                }
            }
        }

        // Iterator 2: attraversamento in ampiezza (livello per livello).
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
