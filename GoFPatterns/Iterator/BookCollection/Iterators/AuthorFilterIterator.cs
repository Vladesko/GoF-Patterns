namespace GoFPatterns.Iterator.BookCollection.Iterators
{
    // Iterador concreto: devuelve solo los libros del autor indicado.
    public class AuthorFilterIterator(Library library, string author) : IBookIterator
    {
        private int _position;

        public bool HasNext()
        {
            // Saltamos los libros de otros autores.
            while (_position < library.Count && library.GetAt(_position).Author != author)
            {
                _position++;
            }
            return _position < library.Count;
        }

        public Book Next() =>
            HasNext() ? library.GetAt(_position++) : throw new InvalidOperationException("No hay más libros.");

        public void Reset() => _position = 0;
    }
}
