🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Memento) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Memento) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Memento) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Memento) | **Español**

# Memento (Recuerdo, Instantánea)

**Tipo:** patrón de comportamiento.

**Memento** permite **guardar y restaurar** un estado anterior de un objeto **sin exponer** los detalles de su implementación y sin romper la encapsulación.

---

## Qué problema resuelve

Hay que implementar «Deshacer» en un editor o «Guardar/Cargar» en un juego. Para ello hay que guardar en algún sitio copias del estado del objeto.

La variante ingenua es leer desde fuera todos los campos del objeto y guardarlos. Pero:
- los campos suelen ser **privados**: habría que hacerlos públicos y romper la encapsulación;
- el código que guarda el estado empieza a **depender de la estructura interna** del objeto: si se añade un campo, hay que cambiar el código del historial;
- desde fuera el estado se puede **modificar** por accidente, y la restauración devolvería el objeto a un estado incorrecto.

## Solución

Encargar la creación de la instantánea **al propio objeto**: solo él conoce su estado completo. El objeto crea una **instantánea** (memento), un objeto inmutable con una copia del estado, y sabe restaurarse a partir de ella.

Del almacenamiento de las instantáneas se ocupa un objeto aparte: el **cuidador** (caretaker). Decide *cuándo* guardar y restaurar, pero **no puede mirar dentro** de la instantánea (o solo puede leer sus metadatos).

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Originator** | `TextEditor`, `Hero` | El objeto cuyo estado se guarda. Crea las instantáneas y se restaura a partir de ellas |
| **Memento** | `TextEditor.Snapshot` (a través de `IEditorMemento`), `HeroMemento` | Instantánea inmutable del estado |
| **Caretaker** | `EditorHistory`, `SaveManager` | Guarda las instantáneas y decide cuándo guardar/restaurar. No modifica su contenido |

## Esquema

```text
 ┌──────────────────────┐   Save()    ┌────────────────────────────┐
 │ TextEditor           │────────────►│ Snapshot  «private nested» │
 │ (Originator)         │             │ Text, Cursor, Font         │
 │ - _text, _cursor,    │◄────────────│ implements IEditorMemento  │
 │   _font              │  Restore()  └────────────────────────────┘
 └──────────────────────┘                       ▲
            ▲                                   │ solo ve
            │ Save / Restore                    │ IEditorMemento
 ┌──────────────────────────┐                   │ (Label, CreatedAt)
 │ EditorHistory (Caretaker)│───────────────────┘
 │ Stack<IEditorMemento>    │
 └──────────────────────────┘
```

---

## Ejemplo 1. Historial del editor de texto (`TextEditorHistory/`): encapsulación estricta

**Originator**: `TextEditor` con estado privado: texto, posición del cursor y fuente. No expone ningún getter para estos campos.

**Memento**: `Snapshot` está declarado como **clase anidada privada** del editor. La clase anidada ve todo lo que hay dentro del editor, pero su tipo no es accesible desde fuera: los clientes reciben la instantánea solo a través de la **interfaz estrecha** `IEditorMemento` (etiqueta y fecha de creación):

```csharp
public IEditorMemento Save(string label) => new Snapshot(label, _text, _cursor, _font);

public void Restore(IEditorMemento memento)
{
    if (memento is not Snapshot snapshot)
        throw new ArgumentException("Tipo de memento desconocido.");
    _text = snapshot.Text;
    _cursor = snapshot.Cursor;
    _font = snapshot.Font;
}

private sealed class Snapshot(...) : IEditorMemento { ... }
```

Es el equivalente en C# de las interfaces «amplia» y «estrecha» de GoF: el editor ve todo el contenido de la instantánea; el cuidador, solo los metadatos.

**Caretaker**: `EditorHistory` guarda las instantáneas en una pila. `Backup` obtiene una instantánea del editor y `Undo` saca la última y se la pasa al editor. El cuidador no lee la instantánea ni puede modificarla.

**Client**: la demo hace ediciones, guardando el estado antes de cada una, y luego vuelve atrás: junto con el texto se restauran el cursor y la fuente.

## Ejemplo 2. Partidas guardadas (`GameSave/`): la instantánea como `record`

**Originator**: `Hero`: nivel, salud, oro, ubicación, inventario. Las propiedades son de solo lectura desde fuera (`private set`) y cambian mediante acciones del juego (`Travel`, `Fight`, `Buy`).

**Memento**: `HeroMemento`, un `sealed record`. En C#, un record es una forma cómoda de describir una instantánea inmutable. El inventario se **copia** en un array nuevo (`[.. _inventory]`); de lo contrario, la instantánea compartiría la lista con el héroe y las compras posteriores al guardado se «colarían» en la partida guardada.

**Caretaker**: `SaveManager` guarda las instantáneas en ranuras con nombre y sabe cargarlas. Aquí la encapsulación es más laxa: la instantánea es pública y el gestor lee sus datos para la lista de partidas. Pero solo puede **modificar** al héroe mediante `hero.Load(memento)`, y la instantánea en sí es inmutable.

**Client**: la demo juega una parte de la partida, guarda, pierde contra el jefe y carga primero el autoguardado y después una partida anterior.

---

## Cuándo usarlo

- Hay que guardar instantáneas del estado para deshacer, revertir una transacción, guardar la partida o guardar borradores.
- Obtener el estado del objeto directamente expondría detalles de implementación.

## Cuándo no hace falta

- El estado del objeto es sencillo y público: basta con copiarlo.
- El estado es enorme pero cambia poco a poco: guardar instantáneas completas es caro; es mejor guardar los cambios (el enfoque de Command con `Undo`).

## Ventajas y desventajas

**Ventajas**
- No rompe la encapsulación del objeto original.
- Simplifica el objeto original: no necesita guardar el historial.

**Desventajas**
- Consumo de memoria si hay muchas instantáneas o son grandes.
- El cuidador debe gestionar el ciclo de vida de las instantáneas (eliminar las obsoletas).
- En lenguajes de tipado dinámico es difícil garantizar que nadie modifique la instantánea; en C# ayudan las clases anidadas `private` y los `record` inmutables.

## Relación con otros patrones

- **Command** + **Memento**: antes de ejecutarse, el comando guarda una instantánea del receptor, y `Undo` la restaura. Command guarda «cómo deshacer»; Memento, «lo que había».
- **Iterator**: se puede guardar una instantánea de la posición de la iteración.
- **Prototype**: si el estado es sencillo, la instantánea se puede hacer clonando el objeto.
- **State**: la instantánea puede incluir el objeto de estado actual.
