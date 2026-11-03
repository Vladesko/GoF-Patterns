namespace GoFPatterns.Builder.PizzaShop
{
    // Director – kennt die Reihenfolge der Schritte für bekannte Rezepte.
    // Er arbeitet mit jedem Builder über die Schnittstelle IPizzaBuilder.
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
            _builder.SetSize("mittel");
            _builder.SetDough("Weizenteig");
            _builder.SetSauce("Tomate");
            _builder.AddTopping("Mozzarella");
            _builder.AddTopping("Basilikum");
            return _builder.GetResult();
        }

        public Pizza MakePepperoni()
        {
            _builder.Reset();
            _builder.SetSize("groß");
            _builder.SetDough("Weizenteig");
            _builder.SetSauce("Tomate");
            _builder.AddTopping("Pepperoni");
            _builder.AddExtraCheese();
            return _builder.GetResult();
        }
    }
}
