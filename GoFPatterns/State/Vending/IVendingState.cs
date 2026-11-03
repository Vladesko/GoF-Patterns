namespace GoFPatterns.State.Vending
{
    // State - reazione del distributore alle azioni dell'utente.
    public interface IVendingState
    {
        void InsertCoin(VendingMachine machine, int amount);
        void SelectProduct(VendingMachine machine);
        void Refund(VendingMachine machine);
    }
}
