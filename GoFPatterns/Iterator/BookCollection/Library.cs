using System.Collections;
using GoFPatterns.Iterator.BookCollection.Iterators;

namespace GoFPatterns.Iterator.BookCollection
{
    // Konkretes Aggregat – Bibliothek. Die Bücher liegen in einem Array, aber die Clients wissen das nicht.
    // Sie implementiert auch IEnumerable<Book> – den .NET-Weg des Iterator-Musters –, daher funktionieren foreach und LINQ.
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

        // "yield return" – der Compiler erzeugt die Iterator-Klasse für uns.
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
