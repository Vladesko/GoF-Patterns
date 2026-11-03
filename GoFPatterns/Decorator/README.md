🌐 **Русский** | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Decorator) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Decorator) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Decorator) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Decorator)

# Decorator (Декоратор)

**Тип:** структурный паттерн.

**Decorator** позволяет динамически добавлять объекту новое поведение, «оборачивая» его в объекты-обёртки с тем же интерфейсом. Обёртки можно вкладывать друг в друга в любом порядке и количестве.

---

## Какую проблему решает

Кофейня продаёт напитки, к которым можно добавлять молоко, сироп, увеличенный объём... Если решать через наследование:

```text
Espresso
 ├── EspressoWithMilk
 ├── EspressoWithSyrup
 ├── EspressoWithMilkAndSyrup
 ├── LargeEspressoWithMilkAndSyrup
 └── ...  (и всё то же для чая)
```

Число подклассов растёт комбинаторно. Кроме того, наследование **статично**: нельзя во время работы программы «добавить молоко» к уже созданному объекту. А двойное молоко вообще не выразить без ещё одного класса.

То же со хранилищем данных: иногда нужно сжатие, иногда шифрование, иногда и то, и другое, иногда ещё и логирование — подклассов под каждую комбинацию не напасёшься.

## Решение

Заменить наследование **композицией**. Декоратор:
- реализует **тот же интерфейс**, что и оборачиваемый объект;
- хранит ссылку на оборачиваемый объект;
- делегирует ему вызовы, добавляя своё поведение **до или после**.

Так как декоратор выглядит для клиента как обычный компонент, его можно обернуть ещё одним декоратором — получится «матрёшка». Клиент не знает, работает он с исходным объектом или с цепочкой обёрток.

---

## Участники

| Роль | Класс в примере | Описание |
|------|-----------------|----------|
| **Component** | `IBeverage`, `IDataSource` | Общий интерфейс и для обёрток, и для оборачиваемых объектов |
| **ConcreteComponent** | `Espresso`, `BlackTea`, `InMemoryDataSource` | Базовый объект, к которому добавляют поведение |
| **BaseDecorator** | `BeverageDecorator`, `DataSourceDecorator` | Хранит ссылку на Component и по умолчанию просто делегирует ему вызовы |
| **ConcreteDecorator** | `MilkDecorator`, `SyrupDecorator`, `LargeSizeDecorator`, `CompressionDecorator`, `EncryptionDecorator`, `LoggingDecorator` | Добавляют конкретное поведение до/после вызова обёрнутого объекта |
| **Client** | демо-классы | Собирает нужную комбинацию обёрток и работает с ней через Component |

## Схема

```text
            ┌───────────────────────┐
 Client ───►│ «interface» IBeverage │◄─────────────────┐
            │ GetDescription()      │                  │ wraps
            │ GetCost()             │                  │
            └───────────────────────┘                  │
               ▲                 ▲                     │
               │                 │                     │
          Espresso    ┌──────────────────────────┐     │
          BlackTea    │ «abstract» BeverageDecorator│◆──┘
                      │ - Beverage: IBeverage    │
                      └──────────────────────────┘
                         ▲          ▲           ▲
                   MilkDecorator  SyrupDecorator  LargeSizeDecorator

  new LargeSize( new Syrup( new Milk( new Espresso() ) ) )
       │             │          │          └─ 2.00
       │             │          └─ +0.50
       │             └─ +0.70
       └─ ×1.5
```

---

## Пример 1. Кофейня (`Coffee/`)

**Component** — `IBeverage` с методами `GetDescription()` и `GetCost()`.

**ConcreteComponent** — `Espresso` и `BlackTea` (папка `Beverages/`) знают только свою базовую цену.

**BaseDecorator** — `BeverageDecorator` (папка `AddOns/`) хранит обёрнутый напиток и делегирует ему всё через `virtual`-методы:

```csharp
public abstract class BeverageDecorator(IBeverage beverage) : IBeverage
{
    protected IBeverage Beverage { get; } = beverage;

    public virtual string GetDescription() => Beverage.GetDescription();
    public virtual decimal GetCost() => Beverage.GetCost();
}
```

**ConcreteDecorator** — каждая добавка переопределяет методы, вызывая `base` и дополняя результат:

```csharp
public class MilkDecorator(IBeverage beverage) : BeverageDecorator(beverage)
{
    public override string GetDescription() => $"{base.GetDescription()} + молоко";
    public override decimal GetCost() => base.GetCost() + 0.50m;
}
```

- `SyrupDecorator` показывает, что у декоратора может быть **своё состояние** (вкус сиропа).
- `LargeSizeDecorator` не прибавляет, а **умножает** цену. Поэтому в демо видно, что **порядок обёрток важен**: «большой (эспрессо + сироп)» дороже, чем «большой эспрессо + сироп».

**Client** — собирает напитки «матрёшкой»: `new MilkDecorator(new MilkDecorator(new Espresso()))` — двойное молоко без отдельного класса. Те же добавки без изменений работают и с чаем.

## Пример 2. Источник данных (`DataStream/`)

**Задача:** данные (например, данные клиента с номером карты) сохраняются в хранилище. В зависимости от настроек их нужно сжимать, шифровать, логировать — в любой комбинации.

**Component** — `IDataSource` с `Write`/`Read`.

**ConcreteComponent** — `InMemoryDataSource` хранит строку как есть (имитация файла или записи в БД).

**BaseDecorator** — `DataSourceDecorator` (папка `Decorators/`).

**ConcreteDecorator** — здесь хорошо видно поведение «до» и «после»:

```csharp
// CompressionDecorator
public override void Write(string data) => base.Write(Compress(data));   // ДО записи — сжать
public override string Read() => Decompress(base.Read());                // ПОСЛЕ чтения — распаковать
```

- `CompressionDecorator` — GZip + Base64.
- `EncryptionDecorator` — XOR с ключом (только для демонстрации, это не настоящее шифрование!).
- `LoggingDecorator` — данные не меняет, только пишет в консоль размер.

**Client** — демо оборачивает хранилище:

```csharp
IDataSource source = new LoggingDecorator(
    new CompressionDecorator(
        new EncryptionDecorator(protectedStorage, "secret")),
    "клиент");
```

При записи данные проходят снаружи внутрь (лог → сжатие → шифрование → хранилище), при чтении — изнутри наружу. В хранилище лежит сжатая зашифрованная строка, а клиент читает исходный текст и даже не знает об этом.

Это ровно то, как устроены потоки в .NET: `new GZipStream(new CryptoStream(new FileStream(...)))` — классический Decorator. Также декораторами являются `BufferedStream`, `StreamReader` (поверх `Stream`), а в ASP.NET Core — middleware-конвейер и декорирование сервисов в DI (например, через Scrutor).

---

## Когда применять

- Нужно добавлять обязанности объектам **динамически** и незаметно для клиента.
- Расширение наследованием невозможно (класс `sealed`) или приводит к взрыву числа подклассов.
- Нужно комбинировать независимые «надстройки»: кеширование, логирование, повторные попытки, валидация, метрики.

## Когда не нужен

- Нужна одна фиксированная комбинация поведения — проще написать класс.
- Поведение должно зависеть от того, какие обёртки уже есть (декораторы друг о друге знать не должны).

## Плюсы и минусы

**Плюсы**
- Гибче наследования: поведение добавляется и комбинируется во время выполнения.
- Каждый декоратор отвечает за одну вещь (SRP).
- Можно применять несколько раз (двойное молоко).
- Можно расширять `sealed`-классы и сторонний код через их интерфейс.

**Минусы**
- Много мелких классов.
- Трудно убрать конкретную обёртку из середины цепочки.
- Поведение зависит от порядка обёрток — это легко упустить.
- Код конфигурации «матрёшки» бывает громоздким.

## Связь с другими паттернами

- **Adapter** меняет интерфейс объекта; **Decorator** оставляет интерфейс тем же и расширяет поведение; **Proxy** оставляет интерфейс тем же и **контролирует доступ** (обычно сам управляет жизненным циклом объекта, а декоратор получает объект от клиента).
- **Composite** структурно похож (рекурсивная композиция), но у декоратора один вложенный объект и цель — добавить поведение.
- **Chain of Responsibility** тоже передаёт запрос по цепочке, но её звенья могут **прервать** обработку; декоратор всегда передаёт вызов дальше.
- **Strategy** меняет «внутренности» объекта, **Decorator** — его «оболочку».
