namespace GoFPatterns.Builder.PizzaShop
{
    // Builder - dichiara i passi di costruzione comuni a tutti i builder di pizza.
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
