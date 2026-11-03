namespace GoFPatterns.Bridge.RemoteControl
{
    // Implementierer – Low-Level-Operationen, die jedes Gerät beherrscht.
    // Die Fernbedienungen (Abstraktion) bauen auf diesen primitiven Operationen auf.
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
