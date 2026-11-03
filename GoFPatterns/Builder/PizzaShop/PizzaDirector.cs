namespace GoFPatterns.Builder.PizzaShop
{
    // Director - conosce l'ordine dei passi per le ricette più note.
    // Funziona con qualsiasi builder tramite l'interfaccia IPizzaBuilder.
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
            _builder.SetSize("media");
            _builder.SetDough("frumento");
            _builder.SetSauce("pomodoro");
            _builder.AddTopping("mozzarella");
            _builder.AddTopping("basilico");
            return _builder.GetResult();
        }

        public Pizza MakePepperoni()
        {
            _builder.Reset();
            _builder.SetSize("grande");
            _builder.SetDough("frumento");
            _builder.SetSauce("pomodoro");
            _builder.AddTopping("pepperoni");
            _builder.AddExtraCheese();
            return _builder.GetResult();
        }
    }
}
