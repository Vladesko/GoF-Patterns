namespace GoFPatterns.Interpreter.MathExpressions
{
    // Espressione astratta - ogni nodo dell'albero sintattico sa interpretare se stesso.
    public interface IExpression
    {
        double Interpret(Context context);
    }
}
