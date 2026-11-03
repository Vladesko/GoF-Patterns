namespace GoFPatterns.Interpreter.MathExpressions.Expressions
{
    // Терминальное выражение - число. Лист синтаксического дерева.
    public class NumberExpression(double value) : IExpression
    {
        public double Interpret(Context context) => value;

        public override string ToString() => value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
}
