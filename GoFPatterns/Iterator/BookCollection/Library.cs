using System.Collections;
using GoFPatterns.Iterator.BookCollection.Iterators;

namespace GoFPatterns.Iterator.BookCollection
{
    // Agregado concreto: biblioteca. Los libros se guardan en un array, pero los clientes no lo saben.
    // Además implementa IEnumerable<Book>, la forma .NET del patrón Iterator, así que foreach y LINQ funcionan.
    public class Library : IBookAggregate, IEnumerable<Book>
    {
        private Book[] _books = new Book[2];
        private int _count;

        public int Count => _count;

        public void Add(Book book)
        {
            if (_count == _books.Length)
                Array.Resize(ref _books, _books.Length * 2);
            _books[_count++] = book;
        }

        internal Book GetAt(int index) => _books[index];

        public IBookIterator CreateIterator() => new ForwardBookIterator(this);

        public IBookIterator CreateReverseIterator() => new ReverseBookIterator(this);

        public IBookIterator CreateAuthorIterator(string author) => new AuthorFilterIterator(this, author);

        // "yield return": el compilador genera por nosotros una clase iteradora.
        public IEnumerator<Book> GetEnumerator()
        {
            for (var i = 0; i < _count; i++)
            {
                yield return _books[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
