namespace GoFPatterns.Bridge.RemoteControl
{
    // Implementador: operaciones de bajo nivel que puede realizar cualquier dispositivo.
    // Los mandos (abstracción) se construyen sobre estas operaciones primitivas.
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
