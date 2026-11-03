using System.Collections;
using GoFPatterns.Iterator.BookCollection.Iterators;

namespace GoFPatterns.Iterator.BookCollection
{
    // Aggregate concreto - biblioteca. I libri sono memorizzati in un array, ma i client non lo sanno.
    // Implementa anche IEnumerable<Book> - il modo .NET di realizzare il pattern Iterator - così funzionano foreach e LINQ.
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

        // "yield return" - il compilatore genera per noi una classe iterator.
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
