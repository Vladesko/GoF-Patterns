namespace GoFPatterns.Observer.ProductObserver
{
    // Это интерфейс, который реализуют классы, желающие получать уведомления об изменениях субъекта.
    public interface IObserver
    {
        void Update(decimal price);
    }
}
