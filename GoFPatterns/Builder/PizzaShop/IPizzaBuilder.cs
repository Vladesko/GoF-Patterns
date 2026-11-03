namespace GoFPatterns.Builder.PizzaShop
{
    // Builder - declares construction steps that are common for all pizza builders.
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
