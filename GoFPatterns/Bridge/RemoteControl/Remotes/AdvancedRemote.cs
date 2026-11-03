namespace GoFPatterns.Bridge.RemoteControl.Remotes
{
    // Abstracción refinada: amplía el mando con nuevas funciones.
    // Funciona con cualquier dispositivo; no hace falta modificar los dispositivos.
    public class AdvancedRemote(IDevice device) : Remote(device)
    {
        private int _volumeBeforeMute;

        public void Mute()
        {
            Console.WriteLine($"[{nameof(AdvancedRemote)}] silenciar");
            _volumeBeforeMute = Device.Volume;
            Device.SetVolume(0);
        }

        public void Unmute()
        {
            Console.WriteLine($"[{nameof(AdvancedRemote)}] quitar silencio");
            Device.SetVolume(_volumeBeforeMute);
        }

        public void SetFavoriteChannel(int channel)
        {
            Console.WriteLine($"[{nameof(AdvancedRemote)}] canal favorito");
            Device.SetChannel(channel);
        }
    }
}
