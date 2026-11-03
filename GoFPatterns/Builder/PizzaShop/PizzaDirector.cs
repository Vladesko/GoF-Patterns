namespace GoFPatterns.Builder.PizzaShop
{
    // Director - knows the order of steps for well-known recipes.
    // It works with any builder through IPizzaBuilder interface.
    public class PizzaDirector(IPizzaBuilder builder)
    {
        private IPizzaBuilder _builder = builder;

        public void ChangeBuilder(IPizzaBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            _builder = builder;
        }

        public Pizza MakeMargherita()
        {
            _builder.Reset();
            _builder.SetSize("Medium");
            _builder.SetDough("wheat");
            _builder.SetSauce("tomato");
            _builder.AddTopping("mozzarella");
            _builder.AddTopping("basil");
            return _builder.GetResult();
        }

        public Pizza MakePepperoni()
        {
            _builder.Reset();
            _builder.SetSize("Large");
            _builder.SetDough("wheat");
            _builder.SetSauce("tomato");
            _builder.AddTopping("pepperoni");
            _builder.AddExtraCheese();
            return _builder.GetResult();
        }
    }
}
