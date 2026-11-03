using System.Globalization;
using GoFPatterns.Interpreter.MathExpressions.Expressions;

namespace GoFPatterns.Interpreter.MathExpressions
{
    // Il parser non fa parte del pattern Interpreter, ma qualcuno deve pur costruire l'albero sintattico.
    // Parser a discesa ricorsiva per la grammatica:
    //   expression := term   (('+' | '-') term)*
    //   term       := factor (('*' | '/') factor)*
    //   factor     := number | variable | '(' expression ')'
    public class ExpressionParser
    {
        private string _text = string.Empty;
        private int _position;

        public IExpression Parse(string text)
        {
            _text = text.Replace(" ", string.Empty);
            _position = 0;
            var expression = ParseExpression();
            if (_position < _text.Length)
                throw new FormatException($"Simbolo inatteso '{_text[_position]}' alla posizione {_position}.");
            return expression;
        }

        private IExpression ParseExpression()
        {
            var left = ParseTerm();
            while (Current is '+' or '-')
            {
                var operation = _text[_position++];
                left = new BinaryExpression(left, operation, ParseTerm());
            }
            return left;
        }

        private IExpression ParseTerm()
        {
            var left = ParseFactor();
            while (Current is '*' or '/')
            {
                var operation = _text[_position++];
                left = new BinaryExpression(left, operation, ParseFactor());
            }
            return left;
        }

        private IExpression ParseFactor()
        {
            if (Current == '(')
            {
                _position++;
                var inner = ParseExpression();
                if (Current != ')')
                    throw new FormatException("Manca ')'.");
                _position++;
                return inner;
            }

            var start = _position;
            if (char.IsDigit(Current))
            {
                while (char.IsDigit(Current) || Current == '.') _position++;
                return new NumberExpression(double.Parse(_text[start.._position], CultureInfo.InvariantCulture));
            }

            if (char.IsLetter(Current))
            {
                while (char.IsLetterOrDigit(Current)) _position++;
                return new VariableExpression(_text[start.._position]);
            }

            throw new FormatException($"Simbolo inatteso alla posizione {_position}.");
        }

        private char Current => _position < _text.Length ? _text[_position] : '\0';
    }
}
