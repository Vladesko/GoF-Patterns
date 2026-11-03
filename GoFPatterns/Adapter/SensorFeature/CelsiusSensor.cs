namespace GoFPatterns.Adapter.SensorFeature
{
    // Esta es una clase de la funcionalidad moderna. No necesita adaptador
    public class CelsiusSensor : ITemperatureSensor
    {

        // Necesario para la demostración
        // Debería ser privado u obtenerse de otro servicio
        private readonly int CurrentTemperature = new Random().Next(15, 35); // Emulación de la temperatura

        // Aquí se hace cierto trabajo. Podría ser una petición a otro servicio
        public decimal GetTemperatureCelsius() => CurrentTemperature;
    }
}
