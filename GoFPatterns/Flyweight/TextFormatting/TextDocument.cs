namespace GoFPatterns.Flyweight.TextFormatting
{
    // Cliente: documento que guarda miles de caracteres pero solo unos pocos estilos.
    public class TextDocument
    {
        private readonly List<FormattedCharacter> _characters = [];
        private readonly StyleFactory _styles = new();

        public int CharactersCount => _characters.Count;
        public int StylesCount => _styles.Count;

        public void Append(string text, string font, int size, bool bold = false, string color = "negro")
        {
            var style = _styles.Get(font, size, bold, color);
            foreach (var symbol in text)
            {
                _characters.Add(new FormattedCharacter(symbol, _characters.Count, style));
            }
        }

        public int CountDistinctStyleObjects() =>
            _characters.Select(c => c.Style).Distinct(ReferenceEqualityComparer.Instance).Count();

        public string Render()
        {
            // Agrupamos los caracteres vecinos con el mismo objeto de estilo en fragmentos.
            var fragments = new List<string>();
            CharacterStyle? current = null;
            var buffer = new System.Text.StringBuilder();
            foreach (var character in _characters)
            {
                if (!ReferenceEquals(character.Style, current) && buffer.Length > 0)
                {
                    fragments.Add(Format(buffer.ToString(), current!));
                    buffer.Clear();
                }
                current = character.Style;
                buffer.Append(character.Symbol);
            }
            if (buffer.Length > 0)
                fragments.Add(Format(buffer.ToString(), current!));

            return string.Join(Environment.NewLine, fragments);
        }

        private static string Format(string text, CharacterStyle style) =>
            $"  [{style.Font} {style.Size}{(style.Bold ? " negrita" : "")} {style.Color}] \"{text}\"";
    }
}
