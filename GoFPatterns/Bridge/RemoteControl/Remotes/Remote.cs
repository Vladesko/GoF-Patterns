namespace GoFPatterns.Bridge.RemoteControl.Remotes
{
    // Abstraction - logica di controllo di alto livello.
    // Contiene un riferimento all'implementor (il "ponte") e gli delega il lavoro reale.
    public class Remote(IDevice device)
    {
        protected readonly IDevice Device = device;

        public void TogglePower()
        {
            Console.WriteLine($"[{GetType().Name}] accensione/spegnimento");
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
