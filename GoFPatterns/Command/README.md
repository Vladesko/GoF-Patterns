🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Command) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Command) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Command) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Command) | **Español**

# Command (Comando)

**Tipo:** patrón de comportamiento.

**Command** convierte una petición (una acción) en un **objeto independiente** que contiene toda la información sobre la petición. Ese objeto se puede pasar como parámetro, poner en una cola, registrar en un log, ejecutar más tarde y **deshacer**.

---

## Qué problema resuelve

**Editor de texto.** Hay operaciones «insertar», «eliminar» y «reemplazar» que se invocan desde el menú, los atajos de teclado y la barra de herramientas. Hay que poder **deshacerlas y rehacerlas** (Undo/Redo). Si el botón llama directamente a `document.Delete(...)`:
- el código de la llamada se duplica en todos los sitios (menú, botón, atajo);
- no está claro cómo deshacer la acción: el texto eliminado ya se ha perdido;
- no hay un lugar único donde se guarde el historial.

**Hogar inteligente.** Un mando con botones a los que el usuario asigna acciones: «encender la luz», «salir de casa» (apagarlo todo + activar la alarma). El mando no debe conocer todos los dispositivos, y los escenarios deben construirse a partir de acciones sencillas.

## Solución

Modelar cada acción como un objeto con una interfaz común (`Execute()` y, si hace falta, `Undo()`). El comando guarda:
- una referencia al **receptor**, el objeto que realmente hace el trabajo;
- todos los **parámetros** de la llamada;
- los datos necesarios para **deshacer** (por ejemplo, el texto eliminado).

El **invocador** (botón, mando, historial) trabaja solo con la interfaz del comando y no conoce ni al receptor ni los detalles de la operación.

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Command** | `ICommand`, `IHomeCommand` | Interfaz del comando: `Execute()` y a veces `Undo()` |
| **ConcreteCommand** | `InsertTextCommand`, `DeleteTextCommand`, `ReplaceTextCommand`, `ActionCommand`, `MacroCommand` | Guarda el receptor y los parámetros e implementa `Execute` llamando a métodos del receptor |
| **Receiver** | `Document`, `Light`, `Thermostat`, `SecuritySystem` | Contiene la lógica de negocio. No sabe nada de los comandos |
| **Invoker** | `CommandHistory`, `RemoteControl` | Lanza los comandos y puede guardar su historial o una cola |
| **Client** | clases de demo | Crea los comandos, los conecta con los receptores y se los pasa al invocador |

## Esquema

```text
 Client ── crea ──► InsertTextCommand(document, pos, text)
                               │
                               ▼
 CommandHistory (Invoker)   «interface» ICommand          Document (Receiver)
   _undo: Stack<ICommand> ─► Execute()  ──── llama ────►   Insert(pos, text)
   _redo: Stack<ICommand>    Undo()     ──── llama ────►   Delete(pos, len)
```

---

## Ejemplo 1. Editor de texto con Undo/Redo (`TextEditor/`)

**Receiver**: `Document`, un envoltorio sobre `StringBuilder` con los métodos `Insert`, `Delete` y `GetText`. No sabe nada de comandos ni de historial.

**Command**: `ICommand` con `Execute()`, `Undo()` y `Name` (para mostrarlo).

**ConcreteCommand** (carpeta `Commands/`):

`InsertTextCommand`: deshacer una inserción = eliminar lo insertado:

```csharp
public void Execute() => document.Insert(position, text);
public void Undo() => document.Delete(position, text.Length);
```

`DeleteTextCommand`: antes de eliminar, **recuerda** el texto que se elimina; de lo contrario sería imposible deshacerlo:

```csharp
public void Execute()
{
    _deletedText = document.GetText(position, length);
    document.Delete(position, length);
}
public void Undo() => document.Insert(position, _deletedText);
```

`ReplaceTextCommand`: comando **compuesto** de eliminación e inserción; los deshace en orden inverso.

**Invoker**: `CommandHistory` guarda dos pilas:

```csharp
public void Execute(ICommand command)
{
    command.Execute();
    _undo.Push(command);
    _redo.Clear();          // una acción nueva invalida el «futuro»
}

public void Undo()
{
    if (!_undo.TryPop(out var command)) return;
    command.Undo();
    _redo.Push(command);
}
```

**Client**: la demo ejecuta una serie de ediciones y después Undo/Redo. Al final se muestra que, tras una acción nueva, ya no es posible hacer Redo: así funcionan todos los editores.

## Ejemplo 2. Mando del hogar inteligente (`SmartHome/`)

**Receiver**: los dispositivos de la carpeta `Devices/`: `Light`, `Thermostat`, `SecuritySystem`.

**Command**: `IHomeCommand` (`Description`, `Execute()`), sin deshacer.

**ConcreteCommand** (carpeta `Commands/`):
- `ActionCommand`: comando basado en un **delegado**. En C# no es obligatorio crear una clase aparte para cada acción simple: basta con una lambda o una referencia a un método: `new ActionCommand("Activar la alarma", security.Arm)`. En el fondo, `Action` es precisamente un «comando sin deshacer».
- `MacroCommand`: **macrocomando**: guarda una lista de comandos y los ejecuta por turnos. Como también es un `IHomeCommand`, se le pueden anidar otras macros: el escenario «Salir de casa» contiene la macro «Apagar todas las luces» (esto es a la vez un Composite).

**Invoker**: `RemoteControl`:
- los botones (`Dictionary<int, IHomeCommand>`) se configuran en tiempo de ejecución mediante `SetCommand`;
- los comandos se pueden **poner en cola** (`Schedule`) y ejecutar más tarde (`RunScheduled`); esto es posible precisamente porque la acción se ha convertido en un objeto.

**Client**: la demo crea los dispositivos, monta los escenarios y los asigna a los botones. El mando nunca accede directamente a los dispositivos.

---

## Cuándo usarlo

- Hay que parametrizar objetos con acciones (botones, elementos de menú, atajos de teclado).
- Se necesita deshacer/rehacer operaciones.
- Hay que encolar operaciones, ejecutarlas de forma programada o remota (colas de mensajes, tareas en segundo plano).
- Hay que registrar las operaciones para poder «reproducirlas» de nuevo tras un fallo.
- Se necesitan transacciones/escenarios formados por varias acciones (macrocomandos).

## Cuándo no hace falta

- Hay una sola acción, que se invoca desde un único lugar y no requiere deshacer ni cola: una simple llamada a un método es más clara.

## Ventajas y desventajas

**Ventajas**
- El invocador está separado del receptor.
- Deshacer, rehacer, ejecución diferida, colas, logging.
- Los comandos sencillos se combinan en otros complejos (macros).
- Los comandos nuevos se añaden sin modificar el código existente (Open/Closed).

**Desventajas**
- Muchas clases pequeñas (se resuelve en parte con delegados, como en `ActionCommand`).
- Deshacer puede ser complicado: el comando debe guardar datos suficientes para restaurar el estado.

## Relación con otros patrones

- **Memento**: para un deshacer complejo, el comando puede guardar una instantánea del estado del receptor antes de ejecutarse.
- **Chain of Responsibility**: la petición de la cadena suele modelarse como un comando.
- **Composite**: `MacroCommand` es un compuesto de comandos.
- **Strategy** también encapsula comportamiento en un objeto, pero las estrategias son **distintas formas** de hacer lo mismo, mientras que los comandos son **distintas acciones** que se pueden guardar, aplazar y deshacer.
- **Prototype**: los comandos que hay que guardar en el historial a veces se copian.
- En .NET son comandos `ICommand` en WPF/MAUI (MVVM) y las peticiones `IRequest` de MediatR (CQRS).
