using System.Globalization;
using GoFPatterns.Interpreter.MathExpressions.Expressions;

namespace GoFPatterns.Interpreter.MathExpressions
{
    // Der Parser ist nicht Teil des Interpreter-Musters selbst, aber irgendwer muss den Syntaxbaum bauen.
    // Parser mit rekursivem Abstieg für die Grammatik:
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
                throw new FormatException($"Unerwartetes Zeichen '{_text[_position]}' an Position {_position}.");
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
                    throw new FormatException("Fehlende ')'.");
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

            throw new FormatException($"Unerwartetes Zeichen an Position {_position}.");
        }

        private char Current => _position < _text.Length ? _text[_position] : '\0';
    }
}
