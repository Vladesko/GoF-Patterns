namespace GoFPatterns.Builder.PizzaShop
{
    // Распорядитель (Director) - знает порядок шагов для известных рецептов.
    // Он работает с любым строителем через интерфейс IPizzaBuilder.
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
            _builder.SetSize("средняя");
            _builder.SetDough("пшеничное");
            _builder.SetSauce("томатный");
            _builder.AddTopping("моцарелла");
            _builder.AddTopping("базилик");
            return _builder.GetResult();
        }

        public Pizza MakePepperoni()
        {
            _builder.Reset();
            _builder.SetSize("большая");
            _builder.SetDough("пшеничное");
            _builder.SetSauce("томатный");
            _builder.AddTopping("пепперони");
            _builder.AddExtraCheese();
            return _builder.GetResult();
        }
    }
}
