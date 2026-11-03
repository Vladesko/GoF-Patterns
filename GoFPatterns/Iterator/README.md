🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Iterator) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Iterator) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Iterator) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Iterator) | **Español**

# Iterator (Iterador)

**Tipo:** patrón de comportamiento.

**Iterator** ofrece una forma de **recorrer secuencialmente los elementos** de un objeto compuesto (una colección) **sin exponer su estructura interna**, ya sea un array, una lista, un árbol o cualquier otra cosa.

---

## Qué problema resuelve

Las colecciones son muy variadas: array, lista enlazada, árbol, grafo, el resultado de una consulta a la BD. El cliente solo necesita «pasar por todos los elementos», pero:

- si el recorrido está escrito en el cliente, el cliente queda **atado a la estructura** de la colección (índices para el array, recursión para el árbol). Si se cambia el array por un árbol, hay que reescribir todos los clientes;
- puede haber **varios** recorridos (directo, inverso, con filtro, en profundidad, en anchura). Si se añaden a la propia colección, esta acumula responsabilidades ajenas;
- a veces hacen falta **varios recorridos independientes a la vez**, así que la posición del recorrido no puede guardarse en la propia colección.

## Solución

Sacar la lógica de recorrido a un objeto aparte: el **iterador**. El iterador:
- guarda la **posición actual**;
- ofrece una interfaz sencilla: «¿hay siguiente?» y «dame el siguiente»;
- conoce la estructura interna de la colección, mientras que el cliente no.

La colección (el **agregado**) sabe crear iteradores para sí misma. Cada llamada crea un iterador nuevo con su propia posición.

### Iterator en .NET

En C# el patrón está integrado en el lenguaje:

| GoF | .NET |
|-----|------|
| Aggregate | `IEnumerable<T>` (`GetEnumerator()`) |
| Iterator | `IEnumerator<T>` (`MoveNext()`, `Current`, `Reset()`) |
| Bucle del cliente | `foreach` |
| Escribir un iterador | `yield return`: el compilador genera la clase iteradora |

Por eso en el código real no se suele escribir un `IBookIterator` propio, sino que se implementa `IEnumerable<T>`. Más abajo se muestran ambas formas.

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Iterator** | `IBookIterator`, `IEnumerator<T>` | Interfaz de recorrido: `HasNext()`, `Next()`, `Reset()` |
| **ConcreteIterator** | `ForwardBookIterator`, `ReverseBookIterator`, `AuthorFilterIterator`, iteradores generados a partir de `yield return` | Implementa un algoritmo de recorrido concreto y guarda la posición actual |
| **Aggregate** | `IBookAggregate`, `IEnumerable<T>` | Interfaz de la colección que crea los iteradores |
| **ConcreteAggregate** | `Library`, `CategoryNode` | La colección que devuelve los iteradores necesarios |
| **Client** | clases de demo | Recorre la colección solo a través del iterador |

## Esquema

```text
 Client ─────► «interface» IBookAggregate        «interface» IBookIterator
                 CreateIterator()        ─────►    HasNext(), Next(), Reset()
                        ▲                                   ▲
                        │                ┌──────────────────┼──────────────────┐
                     Library             │                  │                  │
                (Book[] por dentro) ◄─ ForwardBookIterator  ReverseBookIterator  AuthorFilterIterator
                                         _position          _position           _position, author
```

---

## Ejemplo 1. Biblioteca de libros (`BookCollection/`): Iterator clásico e `IEnumerable<T>`

**ConcreteAggregate**: `Library` guarda los libros en un **array** que se amplía a mano (`Array.Resize`). Los clientes no lo saben: el array se puede sustituir por cualquier otra cosa sin tocarlos.

**Iterator**: `IBookIterator` con los métodos `HasNext()`, `Next()` y `Reset()`.

**ConcreteIterator** (carpeta `Iterators/`):
- `ForwardBookIterator`: del primer libro al último;
- `ReverseBookIterator`: del último al primero;
- `AuthorFilterIterator`: se salta los libros de otros autores:

```csharp
public bool HasNext()
{
    while (_position < library.Count && library.GetAt(_position).Author != author)
        _position++;
    return _position < library.Count;
}
```

Los iteradores acceden a la colección mediante el método `internal` `GetAt`, que los clientes no ven.

**Aggregate**: `IBookAggregate` declara los métodos fábrica de los iteradores; `Library` los implementa.

**Client**: el método `Print` de la demo acepta **cualquier** `IBookIterator` y lo recorre con el mismo bucle `while (iterator.HasNext())`.

Además, `Library` implementa `IEnumerable<Book>` mediante `yield return`:

```csharp
public IEnumerator<Book> GetEnumerator()
{
    for (var i = 0; i < _count; i++)
        yield return _books[i];
}
```

Esto proporciona de inmediato `foreach` y todo LINQ (`library.Where(...).OrderBy(...)`).

## Ejemplo 2. Recorrido de un árbol de categorías (`Tree/`)

**ConcreteAggregate**: `CategoryNode`, un nodo del árbol del catálogo. La lista de hijos es privada.

**ConcreteIterator**: dos algoritmos de recorrido escritos con `yield return`:
- `DepthFirst()`: **en profundidad** (como un menú con sangrías). Se usa una `Stack` explícita y no recursión, para que un árbol profundo no provoque `StackOverflowException`;
- `BreadthFirst()`: **en anchura**, por niveles, mediante una `Queue`.

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

**Client**: recorre el mismo árbol de dos maneras con un `foreach` normal.

Una propiedad importante de los iteradores con `yield` es la **evaluación perezosa**: los elementos se calculan de uno en uno, bajo demanda. En la demo, la búsqueda `First(n => n.Name == "Portátiles")` detiene el recorrido en cuanto encuentra el nodo y solo visita una parte del árbol.

---

## Cuándo usarlo

- Hay que ocultar al cliente la estructura interna de una colección compleja.
- Se necesitan varias formas de recorrer una misma colección.
- Se necesita una interfaz de recorrido única para distintas colecciones.
- Se necesita un recorrido perezoso (secuencias grandes o infinitas, carga paginada desde la BD).

## Cuándo no hace falta

- La colección es sencilla (`List<T>`, un array): use el `foreach` integrado.

## Ventajas y desventajas

**Ventajas**
- Simplifica las colecciones: la lógica de recorrido está separada (SRP).
- Distintos algoritmos de recorrido sin modificar la colección (Open/Closed).
- Varios recorridos independientes al mismo tiempo.
- Evaluación perezosa.

**Desventajas**
- Para colecciones sencillas, una complicación innecesaria.
- Modificar la colección durante el recorrido es un problema aparte (las colecciones estándar de .NET lanzan `InvalidOperationException`).

## Relación con otros patrones

- **Composite**: los iteradores se usan a menudo para recorrer árboles Composite.
- **Factory Method**: `CreateIterator()` en la colección es un método fábrica que devuelve la subclase de iterador adecuada.
- **Memento**: se puede guardar el estado de la iteración y volver a él.
- **Visitor** + Iterator: el iterador recorre la estructura y el visitante ejecuta una operación sobre cada elemento.
