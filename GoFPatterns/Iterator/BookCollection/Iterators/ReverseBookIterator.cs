namespace GoFPatterns.Iterator.BookCollection.Iterators
{
    // Iterador concreto: otro algoritmo de recorrido para la misma colección.
    public class ReverseBookIterator(Library library) : IBookIterator
    {
        private int _position = library.Count - 1;

        public bool HasNext() => _position >= 0;

        public Book Next() =>
            HasNext() ? library.GetAt(_position--) : throw new InvalidOperationException("No hay más libros.");

        public void Reset() => _position = library.Count - 1;
    }
}
