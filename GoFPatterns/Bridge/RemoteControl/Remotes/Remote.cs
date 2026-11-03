namespace GoFPatterns.Bridge.RemoteControl.Remotes
{
    // Abstracción: lógica de control de alto nivel.
    // Guarda una referencia al implementador (el "puente") y le delega el trabajo real.
    public class Remote(IDevice device)
    {
        protected readonly IDevice Device = device;

        public void TogglePower()
        {
            Console.WriteLine($"[{GetType().Name}] encendido/apagado");
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
