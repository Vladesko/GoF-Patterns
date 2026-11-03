🌐 **Русский** | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa)

# Паттерны проектирования GoF на C#

Учебный проект с реализацией всех 23 классических паттернов проектирования «Банды четырёх» (Gang of Four, GoF) на C# (.NET 10).
Каждый паттерн лежит в отдельной папке: в ней есть README с описанием (назначение, структура, плюсы и минусы, когда применять) и 1–2 практических примера.
Все примеры можно запустить из общего консольного меню.

## Как запустить

Требуется [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run --project GoFPatterns
```

После запуска откроется меню со списком всех демонстраций. Введите номер нужной демонстрации и нажмите Enter; `0` — выход.

## Паттерны

### Порождающие

| Паттерн | Назначение |
|---|---|
| [Abstract Factory (Абстрактная фабрика)](GoFPatterns/AbtractFactory/README.md) | Создаёт семейства связанных объектов без привязки к их конкретным классам |
| [Builder (Строитель)](GoFPatterns/Builder/README.md) | Пошагово собирает сложный объект |
| [Factory Method (Фабричный метод)](GoFPatterns/FactoryMethod/README.md) | Делегирует создание объектов подклассам |
| [Prototype (Прототип)](GoFPatterns/Prototype/README.md) | Создаёт новые объекты копированием существующих |
| [Singleton (Одиночка)](GoFPatterns/Singleton/README.md) | Гарантирует единственный экземпляр класса и глобальную точку доступа к нему |

### Структурные

| Паттерн | Назначение |
|---|---|
| [Adapter (Адаптер)](GoFPatterns/Adapter/README.md) | Позволяет работать вместе объектам с несовместимыми интерфейсами |
| [Bridge (Мост)](GoFPatterns/Bridge/README.md) | Разделяет абстракцию и реализацию, чтобы они менялись независимо |
| [Composite (Компоновщик)](GoFPatterns/Composite/README.md) | Объединяет объекты в древовидную структуру и работает с ней как с одним объектом |
| [Decorator (Декоратор)](GoFPatterns/Decorator/README.md) | Динамически добавляет объектам новое поведение, оборачивая их |
| [Facade (Фасад)](GoFPatterns/Facade/README.md) | Предоставляет простой интерфейс к сложной подсистеме |
| [Flyweight (Легковес)](GoFPatterns/Flyweight/README.md) | Экономит память, разделяя общее состояние между множеством объектов |
| [Proxy (Заместитель)](GoFPatterns/Proxy/README.md) | Подставляет объект-заместитель, контролирующий доступ к реальному объекту |

### Поведенческие

| Паттерн | Назначение |
|---|---|
| [Chain of Responsibility (Цепочка обязанностей)](GoFPatterns/ChainOfResponsibility/README.md) | Передаёт запрос по цепочке обработчиков, пока один из них его не обработает |
| [Command (Команда)](GoFPatterns/Command/README.md) | Превращает запрос в объект, что позволяет ставить его в очередь и отменять |
| [Interpreter (Интерпретатор)](GoFPatterns/Interpreter/README.md) | Описывает грамматику простого языка и интерпретирует его выражения |
| [Iterator (Итератор)](GoFPatterns/Iterator/README.md) | Даёт последовательный доступ к элементам коллекции, не раскрывая её устройство |
| [Mediator (Посредник)](GoFPatterns/Mediator/README.md) | Убирает прямые связи между объектами, заставляя их общаться через посредника |
| [Memento (Снимок)](GoFPatterns/Memento/README.md) | Сохраняет и восстанавливает состояние объекта, не нарушая инкапсуляцию |
| [Observer (Наблюдатель)](GoFPatterns/Observer/README.md) | Оповещает подписчиков об изменениях состояния объекта |
| [State (Состояние)](GoFPatterns/State/README.md) | Меняет поведение объекта в зависимости от его внутреннего состояния |
| [Strategy (Стратегия)](GoFPatterns/Strategy/README.md) | Выносит взаимозаменяемые алгоритмы в отдельные классы |
| [Template Method (Шаблонный метод)](GoFPatterns/TemplateMethod/README.md) | Задаёт скелет алгоритма, позволяя подклассам переопределять отдельные шаги |
| [Visitor (Посетитель)](GoFPatterns/Visitor/README.md) | Добавляет новые операции над объектами, не изменяя их классы |
