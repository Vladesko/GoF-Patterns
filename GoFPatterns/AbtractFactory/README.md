# Abstract Factory (Абстрактная фабрика)

**Тип:** порождающий паттерн.

**Abstract Factory** позволяет создавать **семейства связанных объектов**, не привязываясь к конкретным классам этих объектов. Клиент работает с фабрикой и продуктами только через интерфейсы.

---

## Какую проблему решает

Сервис оплаты работает с платёжными провайдерами (в примере — вымышленные **PoolPay** и **Strope**). Для каждого провайдера нужен **набор** объектов:
- процессор платежа;
- процессор возврата;
- валидатор платежа.

Эти объекты образуют **семейство**: процессор PoolPay должен работать вместе с валидатором и возвратами PoolPay. Смешать процессор PoolPay с возвратами Strope — ошибка (транзакция одного провайдера не существует у другого).

Если сервис оплаты создаёт объекты сам:

```csharp
if (provider == "PoolPay") { processor = new PoolPayPaymentProcess(); refund = new PoolPayRefundProcessor(); ... }
else if (provider == "Strope") { processor = new StropePaymentProcessor(); refund = new StropeRefundProcessor(); ... }
```

- сервис зависит от **всех** конкретных классов всех провайдеров;
- добавление провайдера — правка сервиса;
- легко ошибиться и создать «несовместимый набор».

## Решение

1. Для каждого вида продукта выделить **интерфейс** (`IPaymentProcessor`, `IRefundProcessor`, `IPaymentValidator`).
2. Объявить **абстрактную фабрику** — интерфейс с методами создания каждого продукта семейства.
3. Для каждого семейства (провайдера) сделать **конкретную фабрику**, которая создаёт продукты только своего семейства.
4. Клиент получает фабрику извне и создаёт продукты только через неё — так он не знает конкретных классов и **не может смешать семейства**.

---

## Участники

| Роль | Класс в примере | Описание |
|------|-----------------|----------|
| **AbstractFactory** | `IPaymentProviderFactory` | Объявляет методы создания всех продуктов семейства |
| **ConcreteFactory** | `PoolPayPaymentFactory`, `StropePaymentFactory` | Создаёт продукты одного семейства (одного провайдера) |
| **AbstractProduct** | `IPaymentProcessor`, `IRefundProcessor`, `IPaymentValidator` | Интерфейсы продуктов |
| **ConcreteProduct** | `PoolPayPaymentProcess`, `PoolPayRefundProcessor`, `PoolPayPaymentValidator`, `StropePaymentProcessor`, `StropeRefundProcessor`, `StropePaymentValidator` | Конкретные реализации продуктов для каждого провайдера |
| **Client** | `PaymentService` (+ `IPaymentService`) | Использует только интерфейсы фабрики и продуктов |

## Схема

```text
                       «interface» IPaymentProviderFactory
                         CreatePaymentProcess(): IPaymentProcessor
                         CreateRefundProcess():  IRefundProcessor
                         CreatePaymentValidator(): IPaymentValidator
                              ▲                         ▲
                              │                         │
               PoolPayPaymentFactory            StropePaymentFactory
                 │      │       │                 │      │       │
                 ▼      ▼       ▼                 ▼      ▼       ▼
         PoolPayPayment PoolPay PoolPay      StropePayment Strope Strope
           Process      Refund  Validator      Processor   Refund Validator

 PaymentService(IPaymentProviderFactory factory)   ← Client
   Pay(amount):  validator.ValidatePayment → processor.ProcessPayment
   Refund(id):   refundProcessor.ProcessRefund
```

---

## Пример. Платёжные провайдеры (`Payments/`)

**AbstractProduct** (папка `Interfaces/Abstractions/`):

```csharp
public interface IPaymentProcessor { Guid ProcessPayment(decimal amount); }
public interface IRefundProcessor  { void ProcessRefund(Guid transactionId); }
public interface IPaymentValidator { bool ValidatePayment(decimal amount); }
```

**AbstractFactory** (папка `Interfaces/AbstractFactories/`):

```csharp
public interface IPaymentProviderFactory
{
    IPaymentProcessor CreatePaymentProcess();
    IRefundProcessor CreateRefundProcess();
    IPaymentValidator CreatePaymentValidator();
}
```

**ConcreteFactory + ConcreteProduct** — у каждого провайдера своя папка:
- `PoolPays/` — `PoolPayPaymentFactory` создаёт `PoolPayPaymentProcess`, `PoolPayRefundProcessor`, `PoolPayPaymentValidator`;
- `Stopes/` — `StropePaymentFactory` создаёт `StropePaymentProcessor`, `StropeRefundProcessor`, `StropePaymentValidator`.

Все конкретные классы помечены `internal` — снаружи сборки о них никто не знает, доступны только интерфейсы.

```csharp
internal class PoolPayPaymentFactory : IPaymentProviderFactory
{
    public IPaymentProcessor CreatePaymentProcess() => new PoolPayPaymentProcess();
    public IPaymentValidator CreatePaymentValidator() => new PoolPayPaymentValidator();
    public IRefundProcessor CreateRefundProcess() => new PoolPayRefundProcessor();
}
```

**Client** — `PaymentService` (папка `Services/`) получает фабрику в конструкторе и **один раз** создаёт через неё весь набор продуктов. Дальше он работает только с интерфейсами:

```csharp
internal class PaymentService(IPaymentProviderFactory paymentProviderFactory) : IPaymentService
{
    private readonly IPaymentProcessor _paymentProcessor = paymentProviderFactory.CreatePaymentProcess();
    private readonly IRefundProcessor _refundProcessor = paymentProviderFactory.CreateRefundProcess();
    private readonly IPaymentValidator _paymentValidator = paymentProviderFactory.CreatePaymentValidator();

    public Guid Pay(decimal amount)
    {
        if (!_paymentValidator.ValidatePayment(amount))
            throw new InvalidOperationException("Invalid payment amount.");
        return _paymentProcessor.ProcessPayment(amount);
    }

    public void Refund(Guid transactionId) => _refundProcessor.ProcessRefund(transactionId);
}
```

Так как все три продукта получены от одной фабрики, они **гарантированно из одного семейства**.

**Демонстрация** — `DemontrateAbstractFactory` создаёт `PaymentService` сначала с фабрикой PoolPay, потом со Strope, и выполняет одинаковые операции `Pay` + `Refund`. Код сервиса один и тот же — меняется только переданная фабрика.

Чтобы подключить нового провайдера, достаточно добавить папку с фабрикой и тремя продуктами. `PaymentService` и уже существующие провайдеры не меняются.

---

## Когда применять

- Система должна работать с разными **семействами** связанных объектов, и они должны использоваться **вместе**.
- Нужно скрыть от клиента конкретные классы продуктов.
- Семейство выбирается один раз (конфигурацией, настройками), а дальше используется везде.
- Примеры: платёжные провайдеры, UI-темы (кнопка + чекбокс + окно для Windows/macOS), провайдеры БД (`DbProviderFactory` в ADO.NET: соединение + команда + параметр), облачные провайдеры (хранилище + очередь + кеш).

## Когда не нужен

- Продукт один, а не семейство — достаточно Factory Method.
- Семейство одно и не будет расширяться.

## Плюсы и минусы

**Плюсы**
- Гарантирует совместимость создаваемых продуктов.
- Клиент не зависит от конкретных классов.
- Код создания продуктов собран в одном месте (SRP).
- Новое семейство добавляется без изменения клиента (Open/Closed).

**Минусы**
- Много интерфейсов и классов.
- Добавить **новый вид продукта** (например, `ISubscriptionProcessor`) сложно: придётся менять интерфейс фабрики и **все** конкретные фабрики.

## Связь с другими паттернами

- **Factory Method** — абстрактная фабрика часто состоит из нескольких фабричных методов. Factory Method создаёт **один** продукт и полагается на наследование; Abstract Factory создаёт **семейство** и передаётся как объект (композиция).
- **Builder** собирает **один сложный** объект по шагам; Abstract Factory сразу возвращает **набор** простых.
- **Prototype** — фабрика может создавать продукты клонированием прототипов.
- **Singleton** — конкретная фабрика обычно нужна в одном экземпляре.
- **Facade** — абстрактную фабрику можно использовать вместо фасада, чтобы скрыть, как создаются объекты подсистемы.
- **Strategy** — с точки зрения `PaymentService` фабрика выглядит как стратегия «какого провайдера использовать».
