🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Adapter) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Adapter) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Adapter) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Adapter) | **Español**

# Adapter (Adaptador)

**Tipo:** patrón estructural.

**Adapter** permite que objetos con **interfaces incompatibles** trabajen juntos. El adaptador es un «convertidor»: envuelve un objeto existente y ofrece al cliente la interfaz que este espera.

Ejemplos de la vida real: el adaptador de enchufe para otro país, USB → HDMI, un conversor de formatos de archivo.

---

## Qué problema resuelve

Un sistema de monitorización de temperatura trabaja con sensores a través de la interfaz `ITemperatureSensor`, que devuelve la temperatura **en grados Celsius**. Todos los sensores nuevos la implementan.

Pero hay un sensor antiguo, `FahrenheitSensor` (código heredado o una biblioteca de terceros):
- su método tiene **otro nombre**: `GetCurrentTemperature()`;
- devuelve la temperatura **en Fahrenheit**;
- no se puede modificar su código (es ajeno o hay otro código que depende de él).

El cliente no puede usar este sensor sin más: las interfaces son incompatibles. Reescribir el cliente para cada sensor «especial» es mala idea: en el cliente aparecerían `if (sensor is FahrenheitSensor)` y fórmulas de conversión.

## Solución

Crear una clase **adaptadora** que:
- implementa **la interfaz que necesita el cliente** (`ITemperatureSensor`);
- guarda una referencia al **objeto adaptado** (`FahrenheitSensor`);
- en los métodos de la interfaz llama a los métodos del objeto adaptado y **convierte** los datos (Fahrenheit → Celsius).

El cliente trabaja con el adaptador como con cualquier otro sensor y no sabe que dentro hay un dispositivo antiguo.

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Target** | `ITemperatureSensor` | La interfaz que espera el cliente |
| **Adaptee** | `FahrenheitSensor` | Clase existente con una interfaz incompatible |
| **Adapter** | `FahrenheitSensorAdapter` | Implementa Target y traduce las llamadas en llamadas a Adaptee |
| **Client** | `DemonstarteAdapter` (método `DisplayTemperature`) | Trabaja solo con Target |
| *(implementación normal de Target)* | `CelsiusSensor` | Sensor moderno: no necesita adaptador |

## Esquema

```text
                     ┌─────────────────────────────┐
 Client ────────────►│ «interface» ITemperatureSensor│
 DisplayTemperature  │ GetTemperatureCelsius()     │
                     └─────────────────────────────┘
                          ▲                  ▲
                          │                  │
                ┌──────────────┐   ┌──────────────────────────┐       ┌────────────────────────┐
                │ CelsiusSensor│   │ FahrenheitSensorAdapter  │──────►│ FahrenheitSensor       │
                └──────────────┘   │ GetTemperatureCelsius(): │       │ (Adaptee, heredado)    │
                                   │  (F - 32) * 5 / 9        │       │ GetCurrentTemperature()│
                                   └──────────────────────────┘       └────────────────────────┘
```

---

## Ejemplo. Sensores de temperatura (`SensorFeature/`)

**Target**: `ITemperatureSensor`:

```csharp
public interface ITemperatureSensor
{
    decimal GetTemperatureCelsius();
}
```

**Implementación normal de Target**: `CelsiusSensor`, un sensor moderno que implementa la interfaz directamente (la temperatura se emula con un número aleatorio de 15–35 °C).

**Adaptee**: `FahrenheitSensor`, un sensor antiguo con su propio método `GetCurrentTemperature()`, que devuelve grados Fahrenheit (emulación de 55–65 °F). **No** implementa la interfaz `ITemperatureSensor`, y no lo modificamos.

**Adapter**: `FahrenheitSensorAdapter` recibe el sensor antiguo mediante un primary constructor, implementa `ITemperatureSensor` y convierte las unidades:

```csharp
public class FahrenheitSensorAdapter(FahrenheitSensor fahrenheitSensor) : ITemperatureSensor
{
    private readonly FahrenheitSensor _fahrenheitSensor = fahrenheitSensor;

    public decimal GetTemperatureCelsius() =>
        (_fahrenheitSensor.GetCurrentTemperature() - 32m) * 5m / 9m;
}
```

El adaptador convierte dos cosas a la vez: el **nombre del método** (`GetCurrentTemperature` → `GetTemperatureCelsius`) y el **formato de los datos** (°F → °C).

**Client**: `DemonstarteAdapter`:
1. muestra que `CelsiusSensor` funciona directamente;
2. muestra el valor «en bruto» del sensor antiguo en Fahrenheit, incompatible con el cliente;
3. envuelve el sensor antiguo: `ITemperatureSensor adapter = new FahrenheitSensorAdapter(fahrenheitSensor);`
4. pasa **ambos** sensores al mismo método `DisplayTemperature(ITemperatureSensor sensor)`, que evalúa la temperatura (frío / agradable / calor) y no sabe nada del adaptador.

---

## Adaptador de objetos y adaptador de clases

- **Adaptador de objetos** (como en el ejemplo): usa **composición**, guarda una referencia al objeto adaptado. Funciona con cualquier subclase de Adaptee. Es la variante principal en C#.
- **Adaptador de clases**: **hereda** de Adaptee e implementa Target. En C# solo es posible si Target es una interfaz (no hay herencia múltiple de clases), y ata el adaptador a una clase concreta.

## Cuándo usarlo

- Hay que usar una clase existente, pero su interfaz no encaja.
- Trabajo con código heredado que no se puede modificar.
- Integración de bibliotecas de terceros y API externas con su propia interfaz (por ejemplo, un `IPaymentGateway` propio sobre el SDK de un sistema de pagos).
- Hay que unificar varias clases heterogéneas bajo una única interfaz.

## Cuándo no hace falta

- La clase original se puede modificar sin más.
- No se trata de «traducir» la interfaz, sino de simplificarla para todo un subsistema: eso es tarea de Facade.

## Ventajas y desventajas

**Ventajas**
- Separa la conversión de interfaces de la lógica de negocio (SRP).
- Los adaptadores nuevos se añaden sin modificar el cliente (Open/Closed).
- Permite reutilizar código existente.

**Desventajas**
- Más clases.
- Un nivel adicional de indirección.

## Relación con otros patrones

| Patrón | Diferencia con Adapter |
|--------|------------------------|
| **Bridge** | Se diseña de antemano para que las partes evolucionen de forma independiente; Adapter se aplica a código ya existente |
| **Decorator** | Conserva la interfaz y añade comportamiento; Adapter cambia la interfaz |
| **Proxy** | Conserva la misma interfaz y controla el acceso; Adapter ofrece una interfaz distinta |
| **Facade** | Crea una interfaz nueva y simplificada para todo un subsistema; Adapter adapta una sola interfaz existente |

## Ejemplos en .NET

- `StreamReader` / `StreamWriter`: adaptan un `Stream` de bytes para trabajar con texto.
- `SqlDataAdapter`: adapta una fuente de datos a `DataSet`.
- Los proveedores de logging (Serilog, NLog) para `Microsoft.Extensions.Logging.ILogger`: adaptadores de loggers de terceros a la interfaz estándar.
