namespace GoFPatterns.Adapter.SensorFeature
{
    //Questo è codice legacy. Serve un Adapter
    public class FahrenheitSensor
    {
        //Serve per la dimostrazione
        //Dovrebbe essere privato e ottenuto da un altro servizio o simili
        private readonly int CurrentTemperature = new Random().Next(55, 65); //Emulazione della temperatura

        //Qui avviene del lavoro. Potrebbe essere una richiesta a un altro servizio
        public decimal GetCurrentTemperature() => CurrentTemperature;
    }
}
