namespace GoFPatterns.Adapter.SensorFeature
{
    //Questa è la classe per la funzionalità moderna. Non serve un adapter
    public class CelsiusSensor : ITemperatureSensor
    {

        //Serve per la dimostrazione
        //Dovrebbe essere privato o ottenuto da un altro servizio
        private readonly int CurrentTemperature = new Random().Next(15, 35); //Emulazione della temperatura

        //Qui avviene del lavoro. Potrebbe essere una richiesta a un altro servizio
        public decimal GetTemperatureCelsius() => CurrentTemperature;
    }
}
