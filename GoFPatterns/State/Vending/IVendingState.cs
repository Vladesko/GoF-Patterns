namespace GoFPatterns.State.Vending
{
    // Estado: reacción de la máquina a las acciones del usuario.
    public interface IVendingState
    {
        void InsertCoin(VendingMachine machine, int amount);
        void SelectProduct(VendingMachine machine);
        void Refund(VendingMachine machine);
    }
}
