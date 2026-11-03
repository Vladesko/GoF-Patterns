namespace GoFPatterns.Adapter.SensorFeature
{
    // Das ist Legacy-Code. Hier wird ein Adapter benötigt
    public class FahrenheitSensor
    {
        // Nur für die Demonstration
        // Sollte privat sein und von einem anderen Dienst o. Ä. bezogen werden
        private readonly int CurrentTemperature = new Random().Next(55, 65); // Temperatur-Emulation

        // Hier passiert die eigentliche Arbeit, z. B. eine Anfrage an einen anderen Dienst
        public decimal GetCurrentTemperature() => CurrentTemperature;
    }
}
