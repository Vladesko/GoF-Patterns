namespace GoFPatterns.State.Vending
{
    // State - reaction of the machine to user actions.
    public interface IVendingState
    {
        void InsertCoin(VendingMachine machine, int amount);
        void SelectProduct(VendingMachine machine);
        void Refund(VendingMachine machine);
    }
}
