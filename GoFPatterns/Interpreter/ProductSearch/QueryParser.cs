using GoFPatterns.Interpreter.ProductSearch.Expressions;

namespace GoFPatterns.Interpreter.ProductSearch
{
    // Builds syntax tree from query string. Grammar (NOT > AND > OR by priority):
    //   or   := and ('OR' and)*
    //   and  := not ('AND' not)*
    //   not  := 'NOT' not | atom
    //   atom := word | '(' or ')'
    public class QueryParser
    {
        private List<string> _tokens = [];
        private int _position;

        public IQueryExpression Parse(string query)
        {
            _tokens = [.. query.Replace("(", " ( ").Replace(")", " ) ")
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)];
            _position = 0;
            return ParseOr();
        }

        private IQueryExpression ParseOr()
        {
            var left = ParseAnd();
            while (Accept("OR"))
                left = new OrExpression(left, ParseAnd());
            return left;
        }

        private IQueryExpression ParseAnd()
        {
            var left = ParseNot();
            while (Accept("AND"))
                left = new AndExpression(left, ParseNot());
            return left;
        }

        private IQueryExpression ParseNot() =>
            Accept("NOT") ? new NotExpression(ParseNot()) : ParseAtom();

        private IQueryExpression ParseAtom()
        {
            if (Accept("("))
            {
                var inner = ParseOr();
                if (!Accept(")"))
                    throw new FormatException("Missing ')'.");
                return inner;
            }
            if (_position >= _tokens.Count)
                throw new FormatException("Unexpected end of query.");
            return new TagExpression(_tokens[_position++].ToLowerInvariant());
        }

        private bool Accept(string token)
        {
            if (_position < _tokens.Count && _tokens[_position] == token)
            {
                _position++;
                return true;
            }
            return false;
        }
    }
}
