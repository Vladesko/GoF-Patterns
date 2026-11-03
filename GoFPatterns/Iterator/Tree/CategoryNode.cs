namespace GoFPatterns.Iterator.Tree
{
    // Agregado: árbol de categorías del catálogo. La estructura interna (lista de hijos) está oculta;
    // los clientes recorren el árbol solo mediante iteradores.
    public class CategoryNode(string name)
    {
        private readonly List<CategoryNode> _children = [];

        public string Name { get; } = name;

        public CategoryNode Add(params CategoryNode[] children)
        {
            _children.AddRange(children);
            return this;
        }

        // Iterador 1: recorrido en profundidad. Devuelve el nodo junto con su profundidad.
        // Se usa una pila explícita en lugar de recursión, para que los árboles profundos no provoquen StackOverflow.
        public IEnumerable<(CategoryNode Node, int Depth)> DepthFirst()
        {
            var stack = new Stack<(CategoryNode, int)>();
            stack.Push((this, 0));
            while (stack.Count > 0)
            {
                var (node, depth) = stack.Pop();
                yield return (node, depth);

                // Apilamos los hijos en orden inverso para que el primer hijo se procese primero.
                for (var i = node._children.Count - 1; i >= 0; i--)
                {
                    stack.Push((node._children[i], depth + 1));
                }
            }
        }

        // Iterador 2: recorrido en anchura (nivel a nivel).
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
