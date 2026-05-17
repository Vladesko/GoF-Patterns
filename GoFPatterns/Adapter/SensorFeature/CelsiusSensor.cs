namespace GoFPatterns.Adapter.SensorFeature
{
    //That is class for modern feature. There is no need adapter
    public class CelsiusSensor : ITemperatureSensor
    {

        //Need for demonstartion
        //Should be private or Get from other Service
        private readonly int CurrentTemperature = new Random().Next(15, 35); //Emulation temperature

        //There is some work. It would be a some request to other service
        public decimal GetTemperatureCelsius() => CurrentTemperature;
    }
}
