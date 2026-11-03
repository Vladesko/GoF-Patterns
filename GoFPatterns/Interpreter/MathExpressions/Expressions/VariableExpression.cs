namespace GoFPatterns.Interpreter.MathExpressions.Expressions
{
    // Expresión terminal: una variable. Su valor se toma del contexto.
    public class VariableExpression(string name) : IExpression
    {
        public double Interpret(Context context) => context.Get(name);

        public override string ToString() => name;
    }
}
