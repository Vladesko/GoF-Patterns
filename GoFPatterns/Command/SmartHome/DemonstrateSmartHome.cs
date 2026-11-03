using GoFPatterns.Command.SmartHome.Commands;
using GoFPatterns.Command.SmartHome.Devices;

namespace GoFPatterns.Command.SmartHome
{
    public class DemonstrateSmartHome
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Command: mando del hogar inteligente ===\n");

            var livingRoom = new Light("salón");
            var kitchen = new Light("cocina");
            var thermostat = new Thermostat();
            var security = new SecuritySystem();

            var livingRoomOn = new ActionCommand("Encender la luz del salón", livingRoom.TurnOn);
            var allLightsOff = new MacroCommand("Apagar todas las luces",
                new ActionCommand("Apagar el salón", livingRoom.TurnOff),
                new ActionCommand("Apagar la cocina", kitchen.TurnOff));

            // El escenario "Salir de casa" se construye a partir de otros comandos, incluida otra macro.
            var leaveHome = new MacroCommand("Salir de casa",
                allLightsOff,
                new ActionCommand("Temperatura eco", () => thermostat.SetTemperature(17)),
                new ActionCommand("Activar la alarma", security.Arm));

            var comeHome = new MacroCommand("Volver a casa",
                new ActionCommand("Desactivar la alarma", security.Disarm),
                new ActionCommand("Temperatura de confort", () => thermostat.SetTemperature(22)),
                livingRoomOn);

            var remote = new RemoteControl();
            remote.SetCommand(1, livingRoomOn);
            remote.SetCommand(2, allLightsOff);
            remote.SetCommand(3, leaveHome);
            remote.SetCommand(4, comeHome);

            remote.PressButton(1);
            remote.PressButton(3);
            remote.PressButton(4);
            remote.PressButton(9);

            // Los comandos son objetos, así que se pueden guardar y ejecutar más tarde.
            Console.WriteLine("\nProgramando comandos para la noche:");
            remote.Schedule(allLightsOff);
            remote.Schedule(new ActionCommand("Temperatura nocturna", () => thermostat.SetTemperature(19)));
            Console.WriteLine("  ...son las 23:00...");
            remote.RunScheduled();
        }
    }
}
