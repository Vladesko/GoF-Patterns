namespace GoFPatterns.Observer.ProductObserver
{
    // Esta es la interfaz que implementan las clases que quieren ser notificadas de los cambios en el sujeto.
    public interface IObserver
    {
        void Update(decimal price);
    }
}
