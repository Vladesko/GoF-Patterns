using GoFPatterns.Interpreter.MathExpressions.Expressions;

namespace GoFPatterns.Interpreter.MathExpressions
{
    public class DemonstrateMathExpressions
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Interpreter: Espressioni matematiche ===\n");

            // L'albero sintattico può essere costruito a mano: (prezzo * qta) - sconto
            IExpression manual = new BinaryExpression(
                new BinaryExpression(new VariableExpression("prezzo"), '*', new VariableExpression("qta")),
                '-',
                new VariableExpression("sconto"));

            var context = new Context();
            context.Set("prezzo", 250);
            context.Set("qta", 3);
            context.Set("sconto", 50);
            Console.WriteLine($"Albero costruito a mano {manual} = {manual.Interpret(context)}");

            // ... oppure dal parser a partire da una stringa.
            var parser = new ExpressionParser();
            string[] formulas = ["2 + 3 * 4", "(2 + 3) * 4", "prezzo * qta * (1 - iva)", "stipendio / 12"];
            context.Set("iva", 0.2);
            context.Set("stipendio", 60000);

            Console.WriteLine();
            foreach (var formula in formulas)
            {
                var tree = parser.Parse(formula);
                Console.WriteLine($"{formula,-28} albero: {tree,-32} = {tree.Interpret(context)}");
            }

            // Lo stesso albero viene interpretato con contesti diversi.
            var totalPrice = parser.Parse("prezzo * qta");
            Console.WriteLine("\nUn albero, contesti diversi:");
            foreach (var (price, count) in new[] { (10.0, 2.0), (99.9, 1.0), (5.0, 100.0) })
            {
                context.Set("prezzo", price);
                context.Set("qta", count);
                Console.WriteLine($"  prezzo={price}, qta={count} -> {totalPrice.Interpret(context)}");
            }
        }
    }
}
