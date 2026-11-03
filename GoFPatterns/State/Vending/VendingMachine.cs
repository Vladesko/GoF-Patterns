using GoFPatterns.State.Vending.States;

namespace GoFPatterns.State.Vending
{
    // Contexto: máquina expendedora. Los datos (saldo, existencias) se guardan aquí; el comportamiento depende del estado.
    public class VendingMachine
    {
        // Los estados no tienen campos, así que se puede compartir una instancia de cada estado (son flyweights).
        internal static readonly IVendingState Idle = new IdleState();
        internal static readonly IVendingState HasMoney = new HasMoneyState();
        internal static readonly IVendingState SoldOut = new SoldOutState();

        private IVendingState _state;

        public VendingMachine(int stock, int price)
        {
            Stock = stock;
            Price = price;
            _state = stock > 0 ? Idle : SoldOut;
        }

        public int Stock { get; internal set; }
        public int Price { get; }
        public int Balance { get; internal set; }
        public string StateName => _state.GetType().Name.Replace("State", string.Empty);

        internal void SetState(IVendingState state) => _state = state;

        public void InsertCoin(int amount)
        {
            Console.WriteLine($"  > introducir {amount}");
            _state.InsertCoin(this, amount);
        }

        public void SelectProduct()
        {
            Console.WriteLine("  > seleccionar producto");
            _state.SelectProduct(this);
        }

        public void Refund()
        {
            Console.WriteLine("  > devolver dinero");
            _state.Refund(this);
        }
    }
}
