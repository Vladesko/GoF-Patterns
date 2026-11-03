namespace GoFPatterns.Builder.PizzaShop
{
    // Builder – deklariert die Bauschritte, die allen Pizza-Buildern gemeinsam sind.
    public interface IPizzaBuilder
    {
        void Reset();
        void SetSize(string size);
        void SetDough(string dough);
        void SetSauce(string sauce);
        void AddTopping(string topping);
        void AddExtraCheese();
        Pizza GetResult();
    }
}
