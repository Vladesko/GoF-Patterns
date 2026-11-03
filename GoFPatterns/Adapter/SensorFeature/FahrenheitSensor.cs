namespace GoFPatterns.Adapter.SensorFeature
{
    // Este es código heredado. Necesita un Adapter
    public class FahrenheitSensor
    {
        // Necesario para la demostración
        // Debería ser privado y obtenerse de otro servicio o similar
        private readonly int CurrentTemperature = new Random().Next(55, 65); // Emulación de la temperatura

        // Aquí se hace cierto trabajo. Podría ser una petición a otro servicio
        public decimal GetCurrentTemperature() => CurrentTemperature;
    }
}
