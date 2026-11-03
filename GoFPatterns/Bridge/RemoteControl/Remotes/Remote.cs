namespace GoFPatterns.Bridge.RemoteControl.Remotes
{
    // Абстракция - высокоуровневая логика управления.
    // Хранит ссылку на исполнителя ("мост") и делегирует ему реальную работу.
    public class Remote(IDevice device)
    {
        protected readonly IDevice Device = device;

        public void TogglePower()
        {
            Console.WriteLine($"[{GetType().Name}] питание");
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
