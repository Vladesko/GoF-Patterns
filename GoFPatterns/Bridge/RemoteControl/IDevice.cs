namespace GoFPatterns.Bridge.RemoteControl
{
    // Implementor - operazioni di basso livello che ogni dispositivo sa eseguire.
    // I telecomandi (abstraction) sono costruiti sopra queste operazioni primitive.
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
