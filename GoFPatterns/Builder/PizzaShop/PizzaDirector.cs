namespace GoFPatterns.Builder.PizzaShop
{
    // Director: conoce el orden de los pasos de las recetas conocidas.
    // Trabaja con cualquier builder a través de la interfaz IPizzaBuilder.
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
            _builder.SetSize("mediana");
            _builder.SetDough("trigo");
            _builder.SetSauce("tomate");
            _builder.AddTopping("mozzarella");
            _builder.AddTopping("albahaca");
            return _builder.GetResult();
        }

        public Pizza MakePepperoni()
        {
            _builder.Reset();
            _builder.SetSize("grande");
            _builder.SetDough("trigo");
            _builder.SetSauce("tomate");
            _builder.AddTopping("pepperoni");
            _builder.AddExtraCheese();
            return _builder.GetResult();
        }
    }
}
