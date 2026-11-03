🌐 **Русский** | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Iterator) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Iterator) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Iterator) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Iterator)

# Iterator (Итератор)

**Тип:** поведенческий паттерн.

**Iterator** даёт способ **последовательно обходить элементы** составного объекта (коллекции), **не раскрывая его внутреннего устройства** — массив это, список, дерево или что-то ещё.

---

## Какую проблему решает

Коллекции бывают разными: массив, связный список, дерево, граф, результат запроса к БД. Клиенту нужно просто «пройти по всем элементам», но:

- если обход написан в клиенте, клиент **привязан к структуре** коллекции (индексы для массива, рекурсия для дерева). Поменяли массив на дерево — переписываем всех клиентов;
- обходов может быть **несколько** (прямой, обратный, с фильтром, в глубину, в ширину). Если добавлять их в саму коллекцию, она обрастает чужими обязанностями;
- иногда нужно **несколько независимых обходов одновременно** — значит, позиция обхода не может храниться в самой коллекции.

## Решение

Вынести логику обхода в отдельный объект — **итератор**. Итератор:
- хранит **текущую позицию**;
- предоставляет простой интерфейс: «есть ли следующий?» и «дай следующий»;
- знает внутреннее устройство коллекции, а клиент — нет.

Коллекция (**агрегат**) умеет создавать для себя итераторы. Каждый вызов создаёт новый итератор со своей позицией.

### Iterator в .NET

В C# паттерн встроен в язык:

| GoF | .NET |
|-----|------|
| Aggregate | `IEnumerable<T>` (`GetEnumerator()`) |
| Iterator | `IEnumerator<T>` (`MoveNext()`, `Current`, `Reset()`) |
| Клиентский цикл | `foreach` |
| Написание итератора | `yield return` — компилятор сам генерирует класс-итератор |

Поэтому в реальном коде свой `IBookIterator` обычно не пишут — реализуют `IEnumerable<T>`. Оба способа показаны ниже.

---

## Участники

| Роль | Класс в примере | Описание |
|------|-----------------|----------|
| **Iterator** | `IBookIterator`, `IEnumerator<T>` | Интерфейс обхода: `HasNext()`, `Next()`, `Reset()` |
| **ConcreteIterator** | `ForwardBookIterator`, `ReverseBookIterator`, `AuthorFilterIterator`, итераторы, сгенерированные из `yield return` | Реализует конкретный алгоритм обхода и хранит текущую позицию |
| **Aggregate** | `IBookAggregate`, `IEnumerable<T>` | Интерфейс коллекции, создающей итераторы |
| **ConcreteAggregate** | `Library`, `CategoryNode` | Коллекция, которая возвращает нужные итераторы |
| **Client** | демо-классы | Обходит коллекцию только через итератор |

## Схема

```text
 Client ─────► «interface» IBookAggregate        «interface» IBookIterator
                 CreateIterator()        ─────►    HasNext(), Next(), Reset()
                        ▲                                   ▲
                        │                ┌──────────────────┼──────────────────┐
                     Library             │                  │                  │
                  (Book[] внутри) ◄── ForwardBookIterator  ReverseBookIterator  AuthorFilterIterator
                                         _position          _position           _position, author
```

---

## Пример 1. Библиотека книг (`BookCollection/`) — классический Iterator и `IEnumerable<T>`

**ConcreteAggregate** — `Library` хранит книги в **массиве** с ручным расширением (`Array.Resize`). Клиенты об этом не знают — массив можно заменить на что угодно, не трогая их.

**Iterator** — `IBookIterator` с методами `HasNext()`, `Next()`, `Reset()`.

**ConcreteIterator** (папка `Iterators/`):
- `ForwardBookIterator` — от первой книги к последней;
- `ReverseBookIterator` — от последней к первой;
- `AuthorFilterIterator` — пропускает книги других авторов:

```csharp
public bool HasNext()
{
    while (_position < library.Count && library.GetAt(_position).Author != author)
        _position++;
    return _position < library.Count;
}
```

Итераторы обращаются к коллекции через `internal`-метод `GetAt` — клиентам он не виден.

**Aggregate** — `IBookAggregate` объявляет фабричные методы итераторов; `Library` их реализует.

**Client** — метод `Print` в демо принимает **любой** `IBookIterator` и обходит его одним и тем же циклом `while (iterator.HasNext())`.

Также `Library` реализует `IEnumerable<Book>` через `yield return`:

```csharp
public IEnumerator<Book> GetEnumerator()
{
    for (var i = 0; i < _count; i++)
        yield return _books[i];
}
```

Это сразу даёт `foreach` и весь LINQ (`library.Where(...).OrderBy(...)`).

## Пример 2. Обход дерева категорий (`Tree/`)

**ConcreteAggregate** — `CategoryNode`: узел дерева каталога. Список детей приватный.

**ConcreteIterator** — два алгоритма обхода, написанные через `yield return`:
- `DepthFirst()` — **в глубину** (как меню с отступами). Используется явный `Stack`, а не рекурсия, чтобы глубокое дерево не вызвало `StackOverflowException`;
- `BreadthFirst()` — **в ширину**, по уровням, через `Queue`.

```csharp
public IEnumerable<(CategoryNode Node, int Depth)> BreadthFirst()
{
    var queue = new Queue<(CategoryNode, int)>();
    queue.Enqueue((this, 0));
    while (queue.Count > 0)
    {
        var (node, depth) = queue.Dequeue();
        yield return (node, depth);
        foreach (var child in node._children)
            queue.Enqueue((child, depth + 1));
    }
}
```

**Client** — обходит одно и то же дерево двумя способами через обычный `foreach`.

Важное свойство итераторов на `yield` — **ленивость**: элементы вычисляются по одному, по требованию. В демо поиск `First(n => n.Name == "Laptops")` останавливает обход, как только узел найден, и посещает только часть дерева.

---

## Когда применять

- Нужно скрыть от клиента внутреннее устройство сложной коллекции.
- Нужно несколько вариантов обхода одной коллекции.
- Нужен единый интерфейс обхода для разных коллекций.
- Нужен ленивый обход (большие или бесконечные последовательности, постраничная загрузка из БД).

## Когда не нужен

- Коллекция простая (`List<T>`, массив) — используйте встроенный `foreach`.

## Плюсы и минусы

**Плюсы**
- Упрощает коллекции: логика обхода вынесена (SRP).
- Разные алгоритмы обхода без изменения коллекции (Open/Closed).
- Несколько независимых обходов одновременно.
- Ленивые вычисления.

**Минусы**
- Для простых коллекций — лишнее усложнение.
- Изменение коллекции во время обхода — отдельная проблема (стандартные коллекции .NET бросают `InvalidOperationException`).

## Связь с другими паттернами

- **Composite** — итераторы часто используют для обхода деревьев Composite.
- **Factory Method** — `CreateIterator()` в коллекции — это фабричный метод, возвращающий нужный подкласс итератора.
- **Memento** — можно сохранить состояние итерации и вернуться к нему.
- **Visitor** + Iterator: итератор обходит структуру, а посетитель выполняет операцию над каждым элементом.
