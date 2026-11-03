namespace GoFPatterns.Interpreter.MathExpressions
{
    // Абстрактное выражение - каждый узел синтаксического дерева умеет интерпретировать себя.
    public interface IExpression
    {
        double Interpret(Context context);
    }
}
