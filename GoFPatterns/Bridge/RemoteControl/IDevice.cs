namespace GoFPatterns.Bridge.RemoteControl
{
    // Implementor - low-level operations which every device can do.
    // Remotes (abstraction) are built on top of these primitive operations.
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
