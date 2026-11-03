using GoFPatterns.Command.SmartHome.Commands;
using GoFPatterns.Command.SmartHome.Devices;

namespace GoFPatterns.Command.SmartHome
{
    public class DemonstrateSmartHome
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Команда: пульт умного дома ===\n");

            var livingRoom = new Light("Гостиная");
            var kitchen = new Light("Кухня");
            var thermostat = new Thermostat();
            var security = new SecuritySystem();

            var livingRoomOn = new ActionCommand("Включить свет в гостиной", livingRoom.TurnOn);
            var allLightsOff = new MacroCommand("Выключить весь свет",
                new ActionCommand("Выключить гостиную", livingRoom.TurnOff),
                new ActionCommand("Выключить кухню", kitchen.TurnOff));

            // Сценарий "Ухожу из дома" собран из других команд, включая ещё одну макрокоманду.
            var leaveHome = new MacroCommand("Ухожу из дома",
                allLightsOff,
                new ActionCommand("Экономная температура", () => thermostat.SetTemperature(17)),
                new ActionCommand("Поставить на охрану", security.Arm));

            var comeHome = new MacroCommand("Пришёл домой",
                new ActionCommand("Снять с охраны", security.Disarm),
                new ActionCommand("Комфортная температура", () => thermostat.SetTemperature(22)),
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

            // Команды - это объекты, поэтому их можно сохранить и выполнить позже.
            Console.WriteLine("\nПланируем команды на ночь:");
            remote.Schedule(allLightsOff);
            remote.Schedule(new ActionCommand("Ночная температура", () => thermostat.SetTemperature(19)));
            Console.WriteLine("  ...наступило 23:00...");
            remote.RunScheduled();
        }
    }
}
