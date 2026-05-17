# 🔌 Adapter Pattern (Адаптер)

> **Адаптер** — это структурный паттерн проектирования, который позволяет объектам с несовместимыми интерфейсами работать вместе.

---

## Проблема

Представьте: вы пишете систему мониторинга температуры. Все новые датчики возвращают показания в **градусах Цельсия**, но у вас есть старый датчик, который работает только с **Фаренгейтами**.

```csharp
// Современный интерфейс
public interface ITemperatureSensor
{
    decimal GetTemperatureCelsius();
}

// Новый датчик - всё ОК ✅
public class CelsiusSensor : ITemperatureSensor { }

// Старый датчик - НЕ совместим ❌
public class FahrenheitSensor
{
    public decimal GetCurrentTemperature() // Возвращает Fahrenheit!
}

