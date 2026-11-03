namespace GoFPatterns.Bridge.RemoteControl.Remotes
{
    // Abstraction - high-level control logic.
    // It holds a reference to implementor (the "bridge") and delegates real work to it.
    public class Remote(IDevice device)
    {
        protected readonly IDevice Device = device;

        public void TogglePower()
        {
            Console.WriteLine($"[{GetType().Name}] power");
            if (Device.IsEnabled)
                Device.Disable();
            else
                Device.Enable();
        }

        public void VolumeUp() => Device.SetVolume(Device.Volume + 5);

        public void VolumeDown() => Device.SetVolume(Device.Volume - 5);

        public void ChannelUp() => Device.SetChannel(Device.Channel + 1);
    }
}
