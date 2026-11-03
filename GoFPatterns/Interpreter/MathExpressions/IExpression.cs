namespace GoFPatterns.Interpreter.MathExpressions
{
    // Abstrakter Ausdruck – jeder Knoten des Syntaxbaums kann sich selbst interpretieren.
    public interface IExpression
    {
        double Interpret(Context context);
    }
}
