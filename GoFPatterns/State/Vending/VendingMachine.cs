using GoFPatterns.State.Vending.States;

namespace GoFPatterns.State.Vending
{
    // Context - distributore automatico. I dati (credito, scorte) sono qui, il comportamento dipende dallo stato.
    public class VendingMachine
    {
        // Gli stati non hanno campi, quindi una sola istanza di ogni stato può essere condivisa (sono flyweight).
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
            Console.WriteLine($"  > inserisci {amount}");
            _state.InsertCoin(this, amount);
        }

        public void SelectProduct()
        {
            Console.WriteLine("  > seleziona prodotto");
            _state.SelectProduct(this);
        }

        public void Refund()
        {
            Console.WriteLine("  > restituzione");
            _state.Refund(this);
        }
    }
}
