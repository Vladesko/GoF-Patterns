namespace GoFPatterns.Interpreter.MathExpressions.Expressions
{
    // Terminal expression - a number. Leaf of the syntax tree.
    public class NumberExpression(double value) : IExpression
    {
        public double Interpret(Context context) => value;

        public override string ToString() => value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
}
