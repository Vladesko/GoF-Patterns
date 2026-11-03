namespace GoFPatterns.Interpreter.MathExpressions.Expressions
{
    // Expresión terminal: un número. Hoja del árbol sintáctico.
    public class NumberExpression(double value) : IExpression
    {
        public double Interpret(Context context) => value;

        public override string ToString() => value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
}
