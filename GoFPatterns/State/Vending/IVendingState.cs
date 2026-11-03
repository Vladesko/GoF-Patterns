namespace GoFPatterns.State.Vending
{
    // Zustand – Reaktion des Automaten auf die Aktionen des Benutzers.
    public interface IVendingState
    {
        void InsertCoin(VendingMachine machine, int amount);
        void SelectProduct(VendingMachine machine);
        void Refund(VendingMachine machine);
    }
}
