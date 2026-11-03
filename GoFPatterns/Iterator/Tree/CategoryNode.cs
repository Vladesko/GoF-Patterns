namespace GoFPatterns.Iterator.Tree
{
    // Aggregate - tree of catalog categories. Internal structure (list of children) is hidden,
    // clients traverse the tree only through iterators.
    public class CategoryNode(string name)
    {
        private readonly List<CategoryNode> _children = [];

        public string Name { get; } = name;

        public CategoryNode Add(params CategoryNode[] children)
        {
            _children.AddRange(children);
            return this;
        }

        // Iterator 1: depth-first traversal. Returns node with its depth.
        // Explicit stack is used instead of recursion, so deep trees don't cause StackOverflow.
        public IEnumerable<(CategoryNode Node, int Depth)> DepthFirst()
        {
            var stack = new Stack<(CategoryNode, int)>();
            stack.Push((this, 0));
            while (stack.Count > 0)
            {
                var (node, depth) = stack.Pop();
                yield return (node, depth);

                // Push children in reverse order, so the first child is processed first.
                for (var i = node._children.Count - 1; i >= 0; i--)
                {
                    stack.Push((node._children[i], depth + 1));
                }
            }
        }

        // Iterator 2: breadth-first traversal (level by level).
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
