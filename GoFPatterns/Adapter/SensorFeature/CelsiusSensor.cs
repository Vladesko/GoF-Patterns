namespace GoFPatterns.Adapter.SensorFeature
{
    // Klasse für eine moderne Funktion. Hier wird kein Adapter benötigt
    public class CelsiusSensor : ITemperatureSensor
    {

        // Nur für die Demonstration
        // Sollte privat sein oder von einem anderen Dienst bezogen werden
        private readonly int CurrentTemperature = new Random().Next(15, 35); // Temperatur-Emulation

        // Hier passiert die eigentliche Arbeit, z. B. eine Anfrage an einen anderen Dienst
        public decimal GetTemperatureCelsius() => CurrentTemperature;
    }
}
