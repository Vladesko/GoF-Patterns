namespace GoFPatterns.Interpreter.MathExpressions.Expressions
{
    // Terminaler Ausdruck – eine Zahl. Ein Blatt des Syntaxbaums.
    public class NumberExpression(double value) : IExpression
    {
        public double Interpret(Context context) => value;

        public override string ToString() => value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
}
