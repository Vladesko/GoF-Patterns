🔌 Adapter Pattern (Адаптер)
Адаптер — это структурный паттерн проектирования, который позволяет объектам с несовместимыми интерфейсами работать вместе.

Проблема
Представьте: вы пишете систему мониторинга температуры. Все новые датчики возвращают показания в градусах Цельсия, но у вас есть старый датчик, который работает только с Фаренгейтами.


``` csharp
// Современный интерфейс
public interface ITemperatureSensor
{
    decimal GetTemperatureCelsius();
}
```
// Новый датчик - всё ОК ✅
public class CelsiusSensor : ITemperatureSensor { }

// Старый датчик - НЕ совместим ❌
public class FahrenheitSensor
{
    public decimal GetCurrentTemperature() // Возвращает Fahrenheit!
}
Проблема: Нельзя просто так использовать старый датчик с новым интерфейсом!

Решение
Создайте адаптер — обёртку, которая преобразует интерфейс старого класса в нужный интерфейс.

csharp

public class FahrenheitSensorAdapter : ITemperatureSensor
{
    private readonly FahrenheitSensor _sensor;
    
    public FahrenheitSensorAdapter(FahrenheitSensor sensor)
    {
        _sensor = sensor;
    }
    
    public decimal GetTemperatureCelsius()
    {
        var fahrenheit = _sensor.GetCurrentTemperature();
        return (fahrenheit - 32m) * 5m / 9m; // Конверсия!
    }
}
Теперь старый датчик работает через единый интерфейс! ✨

Структура паттерна
Участники
Роль	Класс	Описание
Target	ITemperatureSensor	Интерфейс, который ожидает клиент
Client	TemperatureMonitor	Работает с ITemperatureSensor
Adaptee	FahrenheitSensor	Существующий класс с несовместимым интерфейсом
Adapter	FahrenheitSensorAdapter	Адаптирует Adaptee к интерфейсу Target
Пример из реальной жизни
Зарядка для телефона в путешествии 🔌
Вы едете из России 🇷🇺 в Великобританию 🇬🇧:

Ваше зарядное устройство → FahrenheitSensor (несовместимо)
Британская розетка → ITemperatureSensor (нужный интерфейс)
Переходник → FahrenheitSensorAdapter (адаптер!)
text

┌─────────────┐      ┌──────────┐      ┌─────────────┐
│   Зарядка   │──────│Переходник│──────│   Розетка   │
│  (EU plug)  │      │ (Adapter)│      │  (UK socket)│
└─────────────┘      └──────────┘      └─────────────┘
Реализация
1. Целевой интерфейс (Target)
csharp

namespace GoFPatterns.Adapter.SensorFeature
{
    /// <summary>
    /// Target interface - современный стандарт для датчиков температуры
    /// </summary>
    public interface ITemperatureSensor
    {
        decimal GetTemperatureCelsius();
    }
}
2. Современная реализация
csharp

/// <summary>
/// Современный датчик - работает напрямую с интерфейсом
/// </summary>
public class CelsiusSensor : ITemperatureSensor
{
    private static readonly Random _random = new();
    private readonly decimal _currentTemperature = _random.Next(15, 35);

    public decimal GetTemperatureCelsius() => _currentTemperature;
}
3. Легаси код (Adaptee)
csharp

/// <summary>
/// Старый датчик - несовместимый интерфейс (legacy code)
/// </summary>
public class FahrenheitSensor
{
    private static readonly Random _random = new();
    private readonly decimal _currentTemperature = _random.Next(59, 95);

    // Другое имя метода + возвращает Fahrenheit
    public decimal GetCurrentTemperature() => _currentTemperature;
}
4. Адаптер (Adapter) - ключевой элемент!
csharp

/// <summary>
/// Adapter - преобразует интерфейс FahrenheitSensor в ITemperatureSensor
/// </summary>
public class FahrenheitSensorAdapter : ITemperatureSensor
{
    private readonly FahrenheitSensor _sensor;

    public FahrenheitSensorAdapter(FahrenheitSensor sensor)
    {
        _sensor = sensor ?? throw new ArgumentNullException(nameof(sensor));
    }

    public decimal GetTemperatureCelsius()
    {
        var fahrenheit = _sensor.GetCurrentTemperature();
        
        // Конверсия: C = (F - 32) × 5/9
        return (fahrenheit - 32m) * 5m / 9m;
    }
}
5. Демонстрация использования
csharp

public class AdapterDemo
{
    public void Demonstrate()
    {
        Console.WriteLine("=== Adapter Pattern Demo ===\n");

        // Современный датчик
        ITemperatureSensor celsiusSensor = new CelsiusSensor();
        DisplayTemperature("Celsius Sensor", celsiusSensor);

        // Старый датчик (несовместим напрямую)
        var fahrenheitSensor = new FahrenheitSensor();
        Console.WriteLine($"Fahrenheit Sensor (raw): {fahrenheitSensor.GetCurrentTemperature():F1}°F\n");

        // Старый датчик через адаптер
        ITemperatureSensor adapter = new FahrenheitSensorAdapter(fahrenheitSensor);
        DisplayTemperature("Fahrenheit Sensor (adapted)", adapter);

        Console.WriteLine("\n=== Unified Interface ===");
        ProcessSensor(celsiusSensor);
        ProcessSensor(adapter); // Тот же метод работает с обоими!
    }

    private void DisplayTemperature(string name, ITemperatureSensor sensor)
    {
        var temp = sensor.GetTemperatureCelsius();
        Console.WriteLine($"{name}: {temp:F1}°C");
    }

    private void ProcessSensor(ITemperatureSensor sensor)
    {
        var temp = sensor.GetTemperatureCelsius();
        var status = temp switch
        {
            < 18 => "Cold ❄️",
            >= 18 and < 25 => "Comfortable 😊",
            _ => "Hot 🔥"
        };
        Console.WriteLine($"   {temp:F1}°C → {status}");
    }
}
Вывод программы
text

=== Adapter Pattern Demo ===

Celsius Sensor: 23.0°C
Fahrenheit Sensor (raw): 77.0°F

Fahrenheit Sensor (adapted): 25.0°C

=== Unified Interface ===
   23.0°C → Comfortable 😊
   25.0°C → Hot 🔥
Когда использовать
✅ Используйте Adapter когда:
Нужно использовать существующий класс, но его интерфейс не соответствует вашим требованиям
Работа с legacy кодом, который нельзя изменить
Нужно использовать несколько существующих подклассов с единым интерфейсом
Интеграция с внешними библиотеками/API
❌ НЕ используйте когда:
Можете изменить исходный класс напрямую
Интерфейсы слишком различаются (нужен Facade)
Требуется двусторонняя адаптация (рассмотрите Bridge)
Преимущества и недостатки
✅ Преимущества
Single Responsibility Principle - Отделяет конверсию интерфейсов от бизнес-логики
Open/Closed Principle - Можно добавлять новые адаптеры без изменения клиентского кода
Повторное использование - Позволяет использовать существующий код в новых условиях
Гибкость - Один класс может иметь несколько адаптеров
❌ Недостатки
Сложность кода - Увеличивается общее количество классов
Производительность - Дополнительный уровень абстракции (обычно незначительно)
Связанные паттерны
Паттерн	Отличие от Adapter
Bridge	Проектируется заранее, Adapter применяется к существующему коду
Decorator	Расширяет функциональность, Adapter меняет интерфейс
Proxy	Тот же интерфейс, Adapter предоставляет другой
Facade	Упрощает сложный интерфейс, Adapter адаптирует один интерфейс
Реальные примеры использования
В .NET Framework
csharp

// DbDataAdapter - адаптирует DataSet к базе данных
SqlDataAdapter adapter = new SqlDataAdapter(command);
adapter.Fill(dataSet);

// StreamReader - адаптирует Stream к текстовому интерфейсу
using StreamReader reader = new StreamReader(fileStream);
string text = reader.ReadToEnd();
В ASP.NET Core
csharp

// ILogger adapter для разных логгеров (NLog, Serilog, Log4Net)
public class NLogAdapter : ILogger
{
    private readonly NLog.Logger _logger;
    
    public void Log(LogLevel level, string message)
    {
        // Адаптация к NLog API
        _logger.Log(ConvertLogLevel(level), message);
    }
}
Дополнительные ресурсы
📖 Gang of Four - Design Patterns
🎓 Refactoring Guru - Adapter
📝 Microsoft Docs - Design Patterns
🎥 Adapter Pattern Tutorial
Заключение
Паттерн Adapter — это мост между старым и новым кодом. Он позволяет:

✅ Интегрировать legacy системы без их переписывания
✅ Использовать сторонние библиотеки с вашим интерфейсом
✅ Следовать принципам SOLID
✅ Сохранять код чистым и расширяемым
Используйте адаптеры с умом, и ваш код будет гибким и поддерживаемым! 🚀

Автор: Vladesko

Репозиторий: GoF-Patterns

Лицензия: MIT






