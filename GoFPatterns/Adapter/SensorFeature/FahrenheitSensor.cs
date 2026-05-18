namespace GoFPatterns.Adapter.SensorFeature
{
    //That is legacy code. There is need Adapter
    public class FahrenheitSensor
    {
        //Need for demonstartion
        //Should be private and Get from other Service or something
        private readonly int CurrentTemperature = new Random().Next(55, 65); //Emulation temperature

        //There is some work. It would be a some request to other service
        public decimal GetCurrentTemperature() => CurrentTemperature;
    }
}
