namespace GoFPatterns.Interpreter.MathExpressions
{
    // Abstract expression - every node of the syntax tree can interpret itself.
    public interface IExpression
    {
        double Interpret(Context context);
    }
}
