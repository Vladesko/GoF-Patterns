namespace GoFPatterns.Iterator.BookCollection.Iterators
{
    // Конкретный итератор - возвращает только книги заданного автора.
    public class AuthorFilterIterator(Library library, string author) : IBookIterator
    {
        private int _position;

        public bool HasNext()
        {
            // Пропускаем книги других авторов.
            while (_position < library.Count && library.GetAt(_position).Author != author)
            {
                _position++;
            }
            return _position < library.Count;
        }

        public Book Next() =>
            HasNext() ? library.GetAt(_position++) : throw new InvalidOperationException("Книг больше нет.");

        public void Reset() => _position = 0;
    }
}
