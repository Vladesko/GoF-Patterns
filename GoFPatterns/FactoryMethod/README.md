🌐 **Русский** | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/FactoryMethod) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/FactoryMethod) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/FactoryMethod) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/FactoryMethod)

# Factory Method (Фабричный метод)

**Тип:** порождающий паттерн.

**Factory Method** определяет метод для создания объекта в базовом классе, но позволяет **подклассам решать, какой именно класс создавать**. Код, использующий объект, зависит только от его интерфейса, а не от конкретного класса.

---

## Какую проблему решает

Сервис уведомлений сначала умел отправлять только SMS, и код выглядел так:

```csharp
var sms = new SmsNotification();
sms.Notify(message);
```

Потом понадобились email и push. Если продолжать в том же духе, в коде появляются `switch`/`if` с `new` для каждого типа — во всех местах, где отправляются уведомления. Код жёстко привязан к конкретным классам, и добавление нового канала требует правок по всему проекту.

То же с абонементами в фитнес-клубе: «зал», «зал + бассейн», «персональные тренировки». Если клиентский код сам вызывает `new GymMembership()`, он зависит от всех видов абонементов.

## Решение

Заменить прямой вызов `new` вызовом специального **фабричного метода**. Метод объявлен в базовом классе (создателе) и возвращает **интерфейс продукта**, а подклассы-создатели переопределяют его и возвращают конкретные продукты.

Важно: у создателя обычно есть и **своя логика**, которая работает с продуктом через интерфейс и не знает, какой именно продукт создан (в примере с уведомлениями — метод `Notify`).

---

## Участники

| Роль | Класс в примере | Описание |
|------|-----------------|----------|
| **Product** | `INotificationService`, `IMembership` | Общий интерфейс создаваемых объектов |
| **ConcreteProduct** | `SmsNotification`, `UmailNotification`, `PushNotification`, `GymMembership`, `GymPoolMembership`, `PersonalTrainingMembership` | Конкретные реализации продукта |
| **Creator** | `NotificationFactory`, `MembershipFactory` | Объявляет фабричный метод; может содержать логику, работающую с продуктом |
| **ConcreteCreator** | `SmsNotificator`, `UmailNotificator`, `PushNotificator`, `GymMembershipFactory`, `GymPoolMembershipFactory`, `PersonalTrainingMembershipFactory` | Переопределяет фабричный метод и возвращает конкретный продукт |
| **Client** | `DemondtrsteFactoryMethod` | Работает с создателями и продуктами |

## Схема

```text
 ┌───────────────────────────────────────────┐          ┌─────────────────────────────┐
 │ «abstract» NotificationFactory (Creator)  │          │ «interface»                 │
 │ # CreateNotification(): INotificationService ──────►│ INotificationService        │
 │ + Notify(message):                        │ creates  │ Notify(message)             │
 │     var s = CreateNotification();         │          └─────────────────────────────┘
 │     s.Notify(message);                    │                ▲        ▲        ▲
 └───────────────────────────────────────────┘                │        │        │
        ▲                ▲                ▲          SmsNotification  Umail   Push
        │                │                │                          Notification Notification
  SmsNotificator   UmailNotificator  PushNotificator
  Create… => new SmsNotification()  ...
```

---

## Пример 1. Система уведомлений (`NotifySystem/`) — создатель с собственной логикой

**Product** — `INotificationService` с методом `Notify(string message)`.

**ConcreteProduct** (папка `NotifiationServices/`) — `SmsNotification`, `UmailNotification`, `PushNotification`: каждый «отправляет» сообщение своим каналом.

**Creator** — абстрактный `NotificationFactory` (папка `Factories/`):

```csharp
public abstract class NotificationFactory
{
    protected abstract INotificationService CreateNotification();   // фабричный метод

    public void Notify(string message)                               // логика создателя
    {
        var notificationService = CreateNotification();
        notificationService.Notify(message);
    }
}
```

Это классическая форма паттерна: метод `Notify` реализован **один раз** в базовом классе и работает с продуктом только через интерфейс. Фабричный метод `protected` — клиент не создаёт продукт сам, а просто вызывает `Notify`.

**ConcreteCreator** — `SmsNotificator`, `UmailNotificator`, `PushNotificator` переопределяют только фабричный метод:

```csharp
internal class SmsNotificator : NotificationFactory
{
    protected override INotificationService CreateNotification() => new SmsNotification();
}
```

**Client** — `DemondtrsteFactoryMethod.DemonstrateNotify()` создаёт нужный создатель и вызывает `Notify`. Чтобы добавить, например, Telegram, нужно два класса — `TelegramNotification` и `TelegramNotificator`; существующий код не меняется.

## Пример 2. Абонементы фитнес-клуба (`Fitnes/`) — фабрики продуктов

**Product** — `IMembership` (название, описание, цена).

**ConcreteProduct** (папка `Memberships/`):
- `GymMembership` — зал, 50;
- `GymPoolMembership` — зал + бассейн, 75;
- `PersonalTrainingMembership` — персональные тренировки, 100.

**Creator** — абстрактный `MembershipFactory` с публичным фабричным методом:

```csharp
internal abstract class MembershipFactory
{
    public abstract IMembership CreateMembership();
}
```

**ConcreteCreator** (папка `Factories/`) — `GymMembershipFactory`, `GymPoolMembershipFactory`, `PersonalTrainingMembershipFactory` — каждая возвращает свой абонемент.

**Client** — `DemondtrsteFactoryMethod.DemonstrateFitnes()` выбирает нужную фабрику и получает абонемент через `CreateMembership()`, дальше работает с ним как с `IMembership`.

Здесь у создателя нет собственной логики — это упрощённая форма паттерна, которую часто используют на практике: фабрику (например, выбранную по настройкам или по выбору пользователя) передают в код, который создаёт объекты, не зная их классов.

---

## Когда применять

- Заранее неизвестно, объекты каких классов понадобится создавать.
- Нужно дать возможность расширять создаваемые объекты через наследование (фреймворки, библиотеки).
- Базовый класс содержит общий алгоритм, а конкретный продукт для него определяют подклассы.
- Нужно централизовать создание объектов, чтобы при необходимости переиспользовать их (кеш, пул).

## Когда не нужен

- Продукт один и не изменится — простой `new` понятнее.
- Нужно создавать **семейства** согласованных объектов — это Abstract Factory.

## Плюсы и минусы

**Плюсы**
- Избавляет код от привязки к конкретным классам продуктов.
- Код создания продуктов в одном месте (SRP).
- Новые продукты добавляются без изменения существующего кода (Open/Closed).

**Минусы**
- На каждый продукт — свой подкласс-создатель, иерархии растут параллельно.

## Связь с другими паттернами

- **Abstract Factory** часто реализуется набором фабричных методов, но создаёт **семейство** продуктов и передаётся как объект.
- **Template Method** — фабричный метод часто является шагом шаблонного метода: `NotificationFactory.Notify()` — шаблонный метод, который вызывает фабричный `CreateNotification()`.
- **Prototype** не требует подклассов-создателей, но требует инициализации прототипов. Factory Method — наоборот.
- **Iterator** — `GetEnumerator()` в коллекциях .NET — это фабричный метод, возвращающий итератор.
- Factory Method часто эволюционирует в **Builder** или **Abstract Factory**, когда создание усложняется.
