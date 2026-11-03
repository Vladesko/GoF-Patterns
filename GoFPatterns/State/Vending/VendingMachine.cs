using GoFPatterns.State.Vending.States;

namespace GoFPatterns.State.Vending
{
    // Контекст - торговый автомат. Данные (баланс, остаток) хранятся здесь, поведение зависит от состояния.
    public class VendingMachine
    {
        // У состояний нет полей, поэтому один экземпляр каждого состояния можно разделять (они являются легковесами).
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
            Console.WriteLine($"  > внести {amount}");
            _state.InsertCoin(this, amount);
        }

        public void SelectProduct()
        {
            Console.WriteLine("  > выбрать товар");
            _state.SelectProduct(this);
        }

        public void Refund()
        {
            Console.WriteLine("  > вернуть деньги");
            _state.Refund(this);
        }
    }
}
