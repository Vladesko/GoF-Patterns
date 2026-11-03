namespace GoFPatterns.Interpreter.MathExpressions
{
    // Expresión abstracta: cada nodo del árbol sintáctico sabe interpretarse a sí mismo.
    public interface IExpression
    {
        double Interpret(Context context);
    }
}
