namespace GoFPatterns.Bridge.RemoteControl
{
    // Исполнитель (Implementor) - низкоуровневые операции, которые умеет выполнять любое устройство.
    // Пульты (абстракция) строятся поверх этих примитивных операций.
    public interface IDevice
    {
        string Name { get; }
        bool IsEnabled { get; }
        int Volume { get; }
        int Channel { get; }

        void Enable();
        void Disable();
        void SetVolume(int volume);
        void SetChannel(int channel);
    }
}
