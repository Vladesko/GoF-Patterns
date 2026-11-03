namespace GoFPatterns.Interpreter.MathExpressions.Expressions
{
    // Espressione terminale - un numero. Foglia dell'albero sintattico.
    public class NumberExpression(double value) : IExpression
    {
        public double Interpret(Context context) => value;

        public override string ToString() => value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
}
