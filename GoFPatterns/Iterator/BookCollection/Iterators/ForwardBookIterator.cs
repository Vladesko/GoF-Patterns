namespace GoFPatterns.Iterator.BookCollection.Iterators
{
    // Iterador concreto: va del primer libro al último.
    // Guarda su propia posición, así que varios iteradores pueden recorrer la misma biblioteca de forma independiente.
    public class ForwardBookIterator(Library library) : IBookIterator
    {
        private int _position;

        public bool HasNext() => _position < library.Count;

        public Book Next() =>
            HasNext() ? library.GetAt(_position++) : throw new InvalidOperationException("No hay más libros.");

        public void Reset() => _position = 0;
    }
}
