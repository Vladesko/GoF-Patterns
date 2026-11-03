🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Strategy) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Strategy) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Strategy) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Strategy) | **Español**

# Strategy (Estrategia)

**Tipo:** patrón de comportamiento.

**Strategy** define una **familia de algoritmos intercambiables**, saca cada uno a una clase aparte y permite sustituirlos mientras el programa se ejecuta. El cliente trabaja con el algoritmo a través de una interfaz común.

---

## Qué problema resuelve

Una tienda online calcula el coste del envío: por correo postal, por mensajero o a un punto de recogida. Si todo está en una sola clase:

```csharp
public decimal Calculate(Parcel parcel, string type)
{
    if (type == "post") return 3 + parcel.Weight * 1.2m;
    else if (type == "courier") return 10 + parcel.Distance * 0.05m + ...;
    else if (type == "pickup") return parcel.Value >= 100 ? 0 : 2.5m;
    ...
}
```

- cada nueva forma de envío implica modificar esta clase y arriesgarse a romper las demás;
- la clase crece sin control y los algoritmos están mezclados;
- es difícil probar un algoritmo por separado;
- los mismos `if` aparecen en el cálculo de plazos, en la visualización, etc.

## Solución

Sacar cada algoritmo a una clase aparte, la **estrategia**, con una interfaz común. La clase **contexto** guarda una referencia a la estrategia y le delega el trabajo sin saber qué estrategia concreta ha recibido. El cliente elige la estrategia adecuada y se la pasa al contexto; se puede sustituir en cualquier momento.

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Strategy** | `IDeliveryStrategy`, `ISortStrategy` | Interfaz común de todas las variantes del algoritmo |
| **ConcreteStrategy** | `PostDeliveryStrategy`, `CourierDeliveryStrategy`, `PickupPointStrategy`, `BubbleSortStrategy`, `QuickSortStrategy`, `BuiltInSortStrategy` | Implementaciones concretas del algoritmo |
| **Context** | `DeliveryCalculator`, `DataSorter` | Guarda la estrategia (o la recibe como parámetro) y la llama a través de la interfaz |
| **Client** | clases de demo | Elige la estrategia y se la pasa al contexto |

## Esquema

```text
 Client ── elige ──► PostDeliveryStrategy
   │                          │
   ▼                          ▼  implementa
 DeliveryCalculator ◆────► «interface» IDeliveryStrategy
 (Context)                    CalculateCost(parcel)
   SetStrategy(s)             EstimateDays(parcel)
   GetQuote(parcel)                 ▲        ▲        ▲
                                    │        │        │
                                  Post   Courier   PickupPoint
```

---

## Ejemplo 1. Coste de envío (`Delivery/`)

**Strategy**: `IDeliveryStrategy` con los métodos `CalculateCost` y `EstimateDays`. Los datos de entrada son un `record Parcel` (peso, distancia, valor declarado).

**ConcreteStrategy** (carpeta `Strategies/`): cada una contiene solo su propia fórmula:
- `PostDeliveryStrategy`: barato y lento; el precio depende del peso;
- `CourierDeliveryStrategy`: más caro, depende de la distancia, 1–2 días;
- `PickupPointStrategy`: gratis para pedidos a partir de 100:

```csharp
public decimal CalculateCost(Parcel parcel) =>
    parcel.DeclaredValue >= FreeDeliveryThreshold ? 0m : 2.5m;
```

**Context**: `DeliveryCalculator` recibe la estrategia en el constructor, puede cambiarla con `SetStrategy` y forma la oferta sin saber qué estrategia hay dentro:

```csharp
public string GetQuote(Parcel parcel) =>
    $"{_strategy.Name} coste: {_strategy.CalculateCost(parcel)}, días: {_strategy.EstimateDays(parcel)}";
```

**Client**: la demo pasa un mismo paquete por todas las estrategias y después **elige automáticamente** la más barata y la más rápida: las estrategias se pueden comparar porque tienen una interfaz común.

Para añadir, por ejemplo, el envío con dron, basta con escribir una clase nueva: no hay que modificar ni el contexto ni las demás estrategias.

## Ejemplo 2. Algoritmos de ordenación (`Sorting/`)

**Strategy**: `ISortStrategy.Sort(int[] items)`.

**ConcreteStrategy** (carpeta `Strategies/`):
- `BubbleSortStrategy`: sencilla, O(n²);
- `QuickSortStrategy`: ordenación rápida, O(n log n) de media;
- `BuiltInSortStrategy`: envoltorio sobre `Array.Sort` (la ordenación introspectiva de .NET).

**Context**: `DataSorter`:
- `Sort(items, strategy)`: la estrategia se pasa **como parámetro del método**, en lugar de guardarse en un campo. Esto también es Strategy: lo importante es poder sustituir el algoritmo, no dónde se guarda la referencia;
- `ChooseFor(items)`: el contexto puede **elegir él mismo** la estrategia según los datos (para un array diminuto, la burbuja; si no, la integrada).

**Client**: la demo ordena los mismos 5000 números con todas las estrategias y compara los tiempos: se ve claramente por qué conviene que los algoritmos sean intercambiables.

Al final se muestra que en C# una estrategia sencilla puede ser simplemente un **delegado**: `Array.Sort(names, byLength)`, donde `byLength` es un `Comparison<string>`. Así funcionan también `IComparer<T>`, `IEqualityComparer<T>` y las lambdas de LINQ (`OrderBy(x => ...)`).

---

## Cuándo usarlo

- Hay varias variantes de un mismo algoritmo y hay que cambiar entre ellas.
- Hay muchas clases parecidas que solo se diferencian en el comportamiento.
- Hay que eliminar una gran sentencia condicional que elige la variante del algoritmo.
- Hay que aislar los detalles del algoritmo del código que lo usa.
- Ejemplos: formas de pago/envío, cálculo de descuentos e impuestos, compresión, ordenación, enrutamiento, validación.

## Cuándo no hace falta

- Hay dos algoritmos que casi nunca cambian: un `if` es más sencillo.
- El cliente no necesita conocer las variantes: entonces quizá sea mejor Template Method o simple polimorfismo.

## Ventajas y desventajas

**Ventajas**
- Los algoritmos se sustituyen en tiempo de ejecución.
- El código de los algoritmos está aislado del resto (SRP).
- La herencia se sustituye por composición.
- Las estrategias nuevas se añaden sin modificar el contexto (Open/Closed).
- Cada estrategia es fácil de probar por separado.

**Desventajas**
- El cliente debe conocer las diferencias entre las estrategias para elegir la adecuada.
- Clases de más si hay pocos algoritmos (en C# se suaviza con delegados).

## Relación con otros patrones

- **State**: casi la misma estructura, pero la estrategia **la elige el cliente** y las estrategias son independientes; los estados **se cambian ellos mismos** entre sí.
- **Template Method** cambia partes del algoritmo mediante **herencia** (de forma estática, a nivel de clases); **Strategy** cambia el algoritmo completo mediante **composición** (de forma dinámica, a nivel de objetos).
- **Decorator** cambia la «envoltura» del objeto; **Strategy**, sus «entrañas».
- **Bridge** también se basa en la delegación a través de una interfaz, pero allí la abstracción también forma una jerarquía.
- **Command** también encapsula comportamiento en un objeto, pero un comando es una **acción** (que se puede aplazar o deshacer), mientras que una estrategia es una **forma** de realizar una misma tarea.
- **Abstract Factory** de este proyecto (`IPaymentProviderFactory`) usa una idea parecida: `PaymentService` trabaja con cualquier proveedor a través de una interfaz.
