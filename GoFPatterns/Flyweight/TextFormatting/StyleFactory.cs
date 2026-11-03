namespace GoFPatterns.Flyweight.TextFormatting
{
    // Flyweight factory - pool of styles. Equal styles are returned as the same object.
    public class StyleFactory
    {
        private readonly Dictionary<CharacterStyle, CharacterStyle> _pool = [];

        public int Count => _pool.Count;

        public CharacterStyle Get(string font, int size, bool bold = false, string color = "black")
        {
            var candidate = new CharacterStyle(font, size, bold, color);
            if (_pool.TryGetValue(candidate, out var existing))
                return existing;

            _pool[candidate] = candidate;
            return candidate;
        }
    }
}
