namespace GoFPatterns.Bridge.RemoteControl.Remotes
{
    // Abstraktion – High-Level-Steuerlogik.
    // Sie hält eine Referenz auf den Implementierer (die "Brücke") und delegiert die eigentliche Arbeit an ihn.
    public class Remote(IDevice device)
    {
        protected readonly IDevice Device = device;

        public void TogglePower()
        {
            Console.WriteLine($"[{GetType().Name}] Ein/Aus");
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
