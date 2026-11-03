using GoFPatterns.State.Vending.States;

namespace GoFPatterns.State.Vending
{
    // Context - vending machine. Data (balance, stock) is stored here, behavior depends on state.
    public class VendingMachine
    {
        // States have no fields, so one instance of each state can be shared (they are flyweights).
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
            Console.WriteLine($"  > insert {amount}");
            _state.InsertCoin(this, amount);
        }

        public void SelectProduct()
        {
            Console.WriteLine("  > select product");
            _state.SelectProduct(this);
        }

        public void Refund()
        {
            Console.WriteLine("  > refund");
            _state.Refund(this);
        }
    }
}
