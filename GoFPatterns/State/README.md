🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/State) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/State) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/State) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/State) | **Español**

# State (Estado)

**Tipo:** patrón de comportamiento.

**State** permite que un objeto **cambie su comportamiento cuando cambia su estado interno**. Desde fuera parece que el objeto ha cambiado de clase.

---

## Qué problema resuelve

Un pedido de una tienda online pasa por varias etapas: *Nuevo → Pagado → Enviado → Entregado* (o *Cancelado*). Una misma acción se comporta de forma distinta según la etapa: cancelar un pedido nuevo es simplemente cancelarlo; uno pagado, devolver el dinero; uno enviado, imposible.

La implementación típica es un campo `Status` y un `switch` en cada método:

```csharp
public void Cancel()
{
    switch (Status)
    {
        case Status.New: Status = Status.Cancelled; break;
        case Status.Paid: Refund(); Status = Status.Cancelled; break;
        case Status.Shipped: throw ...;
        case Status.Delivered: throw ...;
        ...
    }
}
```

Esos `switch` están en `Pay`, `Ship`, `Deliver`, `Cancel`... Si se añade un estado nuevo («Devolución»), hay que repasar todos los métodos. Las reglas de transición quedan repartidas por la clase y es fácil cometer un error.

## Solución

Sacar cada estado a una **clase aparte** con una interfaz común que incluye todas las acciones. El objeto original (el **contexto**) guarda una referencia al objeto de estado actual y le **delega** todas las acciones. Pasar a otro estado consiste simplemente en sustituir el objeto de estado.

En lugar de un gran `switch` sobre todos los estados, obtenemos varias clases pequeñas, cada una de las cuales describe el **comportamiento en un estado** y **a dónde se puede pasar desde él**.

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Context** | `Order`, `VendingMachine` | Guarda una referencia al estado actual y le delega las acciones. Ofrece un método para cambiar de estado |
| **State** | `IOrderState`, `IVendingState` | Interfaz común de todos los estados: todas las acciones disponibles para el contexto |
| **ConcreteState** | `NewState`, `PaidState`, `ShippedState`, `DeliveredState`, `CancelledState`, `IdleState`, `HasMoneyState`, `SoldOutState` | Implementa el comportamiento en un estado concreto y decide cuándo cambiar el contexto de estado |

## Esquema

```text
 ┌───────────────────────┐        ┌────────────────────────┐
 │ Order (Context)       │◆──────►│ «interface» IOrderState│
 │ - State               │        │ Pay(), Ship(),         │
 │ + Pay() => State.Pay()│        │ Deliver(), Cancel()    │
 │ + TransitionTo(state) │        └────────────────────────┘
 └───────────────────────┘                    ▲
                                ┌─────────────┼──────────────┬────────────┐
                             NewState     PaidState     ShippedState   ...

 Transiciones:  New ──Pay──► Paid ──Ship──► Shipped ──Deliver──► Delivered
                 │            │
                 └──Cancel──► Cancelled ◄──Cancel (reembolso)
```

---

## Ejemplo 1. Ciclo de vida del pedido (`OrderLifecycle/`)

**Context**: `Order` no contiene ni un solo `switch`. Todas las acciones públicas se delegan sin más en el estado actual:

```csharp
public void Pay() => State.Pay(this);
public void Cancel() => State.Cancel(this);

internal void TransitionTo(IOrderState state) => State = state;
```

`TransitionTo` está marcado como `internal`: solo las clases de estado pueden cambiar el estado, no el código externo.

**State**: `IOrderState` enumera todas las acciones sobre el pedido.

**Estado base**: `OrderStateBase` (carpeta `States/`) **prohíbe** por defecto todas las acciones. Por eso los estados concretos solo sobrescriben lo que en ellos está permitido; no hace falta escribir «no se puede» para el resto de acciones en cada clase.

**ConcreteState**:
- `NewState`: se permiten el pago (→ `Paid`) y la cancelación (→ `Cancelled`);
- `PaidState`: el envío (→ `Shipped`) y la cancelación **con devolución del dinero**:

```csharp
public override void Cancel(Order order)
{
    Console.WriteLine($"Reembolso de {order.Amount:F2} al cliente");
    order.TransitionTo(new CancelledState());
}
```

- `ShippedState`: solo la entrega (→ `Delivered`);
- `DeliveredState`, `CancelledState`: finales, no sobrescriben nada.

Se ve que la acción «cancelar» se comporta de forma distinta en dos estados y está prohibida en los demás, y para ello no hay ni un solo `if` sobre el estado.

**Client**: la demo intenta ejecutar las acciones en un orden incorrecto (enviar un pedido sin pagar, cancelar uno ya enviado) y los estados las rechazan correctamente.

## Ejemplo 2. Máquina expendedora (`Vending/`)

**Context**: `VendingMachine` guarda los **datos** (existencias, precio, saldo) y el estado actual.

**ConcreteState** (carpeta `States/`):
- `IdleState`: espera dinero; una moneda → `HasMoney`;
- `HasMoneyState`: acumula dinero, dispensa el producto y el cambio; si el producto se ha agotado → `SoldOut`, si no → `Idle`;
- `SoldOutState`: devuelve inmediatamente cualquier moneda.

Aquí se muestra una técnica del libro de GoF: los estados **no tienen campos propios**; todos los datos se guardan en el contexto y se pasan como parámetro. Por eso cada estado se puede crear una sola vez y **compartir** (son flyweights, véase Flyweight):

```csharp
internal static readonly IVendingState Idle = new IdleState();
internal static readonly IVendingState HasMoney = new HasMoneyState();
internal static readonly IVendingState SoldOut = new SoldOutState();
```

**Client**: la demo simula a los compradores: intento de comprar sin dinero, dinero insuficiente, compra con cambio, devolución, compra del último producto e intento de echar una moneda en la máquina vacía. Tras cada acción se muestra el estado actual.

---

## Cuándo usarlo

- El comportamiento del objeto depende mucho de su estado, y hay muchos estados.
- El código está lleno de `switch`/`if` sobre un campo de estado que se repiten en distintos métodos.
- Las reglas de transición entre estados son complejas y cambian a menudo.
- Ejemplos: pedidos, gestión documental (borrador → en revisión → aprobado), conexiones TCP, reproductor multimedia, personajes de juegos, máquinas de estados finitos.

## Cuándo no hace falta

- Hay pocos estados y cambian rara vez: un `switch` sobre un `enum` será más sencillo y claro.

## Ventajas y desventajas

**Ventajas**
- Elimina las grandes sentencias condicionales.
- El código de cada estado está en su propia clase (SRP).
- Los estados nuevos se añaden sin modificar los existentes (Open/Closed).
- Las transiciones son explícitas: se ve desde qué estado se puede llegar a cuál.

**Desventajas**
- Muchas clases cuando el número de estados es pequeño.
- La lógica de transición está repartida entre las clases de estado: es más difícil ver la máquina completa (ayuda un diagrama en la documentación).

## Relación con otros patrones

- **Strategy**: la estructura es casi idéntica (el contexto delega en un objeto con una interfaz). La diferencia está en la intención: la estrategia **la elige el cliente** y las estrategias no se conocen entre sí; los estados **se cambian unos a otros** y conocen las transiciones.
- **Flyweight**: los estados sin campos se pueden compartir entre contextos (ejemplo `Vending`).
- **Singleton**: los objetos de estado a veces se implementan como singletons.
- **Bridge** y **Strategy** también se basan en la composición y la delegación, pero resuelven otros problemas.
