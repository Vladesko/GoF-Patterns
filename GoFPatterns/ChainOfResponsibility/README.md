🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/ChainOfResponsibility) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/ChainOfResponsibility) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/ChainOfResponsibility) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/ChainOfResponsibility) | **Español**

# Chain of Responsibility (Cadena de responsabilidad)

**Tipo:** patrón de comportamiento.

**Chain of Responsibility** permite pasar una petición de forma secuencial por una **cadena de manejadores**. Cada manejador decide si procesa la petición él mismo, si la pasa al siguiente de la cadena, o ambas cosas.

---

## Qué problema resuelve

**Servicio de soporte.** Una incidencia la puede cerrar el bot (si es una pregunta frecuente), un operador (problema sencillo), un ingeniero (un bug) o un responsable (una avería grave). Si se escribe todo en un solo método:

```csharp
if (ticket.Severity == Low && IsFaq(ticket)) Bot(...);
else if (ticket.Severity <= Medium) Operator(...);
else if (ticket.Severity == High) Engineer(...);
else Manager(...);
```

…cualquier línea de soporte nueva, cualquier cambio de orden o un turno de noche «sin bot ni responsable» obligan a modificar este método. El emisor queda rígidamente acoplado a todos los receptores.

**Procesamiento de una petición HTTP.** Antes de que la petición llegue a la lógica de negocio hay que registrarla en el log, comprobar el límite de peticiones, autenticarla y validarla. Cada comprobación puede **detener** el procesamiento. Si todo esto se escribe en el controlador, este crece sin control y mezcla responsabilidades distintas.

## Solución

Convertir cada comprobación/procesamiento en un objeto **manejador** independiente con una interfaz común. Los manejadores se enlazan en una cadena: cada uno solo conoce al **siguiente**. El cliente envía la petición al **primer** manejador y no sabe quién la procesará al final.

Hay dos variantes de comportamiento de la cadena:
1. **Clásica (GoF):** la petición la procesa **un solo** manejador, el primero que puede; los demás simplemente la pasan (ejemplo `SupportTickets`).
2. **Pipeline (middleware):** **cada** manejador hace su parte del trabajo y decide si llamar al siguiente; cualquiera puede interrumpir la cadena (ejemplo `Middleware`).

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Handler** | `SupportHandler`, `IRequestHandler` | Interfaz común de los manejadores. A menudo contiene una referencia al siguiente manejador (o lo recibe como delegado) |
| **ConcreteHandler** | `ChatBotHandler`, `OperatorHandler`, `EngineerHandler`, `ManagerHandler`, `LoggingHandler`, `RateLimitHandler`, `AuthenticationHandler`, `ValidationHandler`, `EndpointHandler` | Decide si puede procesar la petición; si no, la pasa al siguiente |
| **Request** | `SupportTicket`, `HttpRequestContext` | Los datos que viajan por la cadena |
| **Client** | clases de demo, `RequestPipeline` | Monta la cadena y envía la petición al primer eslabón |

## Esquema

```text
 Client ──► ChatBot ──► Operator ──► Engineer ──► Manager ──► (null)
              │            │            │            │
          Low+FAQ?     ≤ Medium?     High?       Critical?
          procesa      procesa      procesa      procesa
```

---

## Ejemplo 1. Servicio de soporte (`SupportTickets/`): cadena clásica

**Request**: `SupportTicket(Id, Description, Severity)` (`record`); el nivel es `TicketSeverity`.

**Handler**: la clase abstracta `SupportHandler` guarda una referencia al siguiente e implementa el comportamiento «por defecto»: pasarla adelante:

```csharp
public SupportHandler SetNext(SupportHandler next)
{
    _next = next;
    return next;           // devolvemos next para construir la cadena «en línea»
}

public virtual void Handle(SupportTicket ticket)
{
    if (_next is not null)
        _next.Handle(ticket);
    else
        Console.WriteLine($"Nadie pudo gestionar el ticket n.º {ticket.Id}");
}
```

**ConcreteHandler** (carpeta `Handlers/`): cada uno comprueba «su» condición; si se cumple, procesa la petición y **detiene** la cadena (`return`); si no, llama a `base.Handle(ticket)`:

```csharp
public override void Handle(SupportTicket ticket)
{
    if (ticket.Severity <= TicketSeverity.Medium)
    {
        Console.WriteLine($"Operador: resolvió el ticket n.º {ticket.Id}");
        return;
    }
    base.Handle(ticket);
}
```

**Client**: monta la cadena `bot → operador → ingeniero → responsable` y envía todas las incidencias **solo al primer eslabón**. En la salida se ve hasta dónde llegó cada incidencia. Al final se muestra otra configuración («turno de noche», solo operador): nadie se hace cargo de la incidencia crítica; es una situación normal en este patrón y hay que tenerla prevista.

## Ejemplo 2. Pipeline de procesamiento de peticiones (`Middleware/`): como en ASP.NET Core

**Request**: `HttpRequestContext`: los datos de entrada (ruta, IP, token, cuerpo) y el resultado (código de respuesta, texto). Los manejadores pueden **enriquecer** el contexto; por ejemplo, la autenticación guarda `UserName`.

**Handler**: `IRequestHandler.Handle(context, Action next)`. El manejador no guarda una referencia al siguiente, sino que la recibe como **delegado**; así los manejadores son totalmente independientes.

**ConcreteHandler** (carpeta `Handlers/`):
- `LoggingHandler`: trabaja **antes y después** del resto de la cadena (escribe la petición, llama a `next()` y escribe la respuesta);
- `RateLimitHandler`: tras N peticiones desde una misma IP responde `429` y **no llama** a `next()`;
- `AuthenticationHandler`: comprueba el token (`401`); si es válido, guarda el usuario en el contexto;
- `ValidationHandler`: comprueba el cuerpo (`400`);
- `EndpointHandler`: el punto final, que realiza el trabajo (`201`).

**Client**: `RequestPipeline` monta la cadena con el método `Use(...)` y enlaza los manejadores de forma recursiva:

```csharp
private void Invoke(int index, HttpRequestContext context)
{
    if (index >= _handlers.Count) return;
    _handlers[index].Handle(context, () => Invoke(index + 1, context));
}
```

Es una copia simplificada de cómo funciona `app.Use(...)` en ASP.NET Core: cada middleware recibe `next` y decide él mismo si pasa la petición adelante. También son cadenas los `DelegatingHandler` de `HttpClient` y los pipeline behaviors de MediatR.

---

## Cuándo usarlo

- La petición la pueden procesar distintos objetos y no se sabe de antemano cuál.
- El conjunto y el orden de los manejadores deben configurarse dinámicamente.
- Hay que ejecutar por turnos varias comprobaciones/pasos independientes, con posibilidad de interrupción.
- Hay que desacoplar al emisor de la petición de sus receptores.

## Cuándo no hace falta

- Siempre hay un único manejador conocido de antemano.
- El orden y el conjunto de pasos nunca cambian: un código secuencial simple se lee mejor.

## Ventajas y desventajas

**Ventajas**
- Reduce el acoplamiento entre emisor y receptores.
- Cada manejador se encarga de una sola cosa (SRP).
- Los manejadores nuevos se añaden sin modificar los existentes (Open/Closed).
- La cadena se puede reorganizar en tiempo de ejecución.

**Desventajas**
- La petición puede quedar sin procesar.
- Es más difícil de depurar: el recorrido de la petición no se ve en un solo lugar del código.
- El orden de los eslabones importa (por ejemplo, el logging debe ir primero y la autenticación, antes de la validación).

## Relación con otros patrones

- **Decorator** tiene una estructura parecida (una cadena de envoltorios), pero el decorador **siempre** pasa la llamada adelante y añade comportamiento; un eslabón de la cadena puede **interrumpir** el procesamiento.
- **Command**: la petición de la cadena suele modelarse como un comando.
- **Composite**: la cadena a menudo se construye sobre un árbol: la petición va de un elemento a su padre (por ejemplo, el procesamiento de eventos en una interfaz gráfica).
- **Mediator** y **Observer**: otras formas de conectar emisores y receptores, mediante un intermediario o mediante suscripción.
