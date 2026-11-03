🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Bridge) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Bridge) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Bridge) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Bridge) | **Español**

# Bridge (Puente)

**Tipo:** patrón estructural.

**Bridge** divide una clase grande (o una jerarquía) en dos jerarquías independientes, la **abstracción** y la **implementación**, que pueden evolucionar por separado. La conexión entre ellas es una referencia (el «puente») desde la abstracción hacia la implementación.

---

## Qué problema resuelve

Tenemos mandos (básico, avanzado) y dispositivos (televisor, radio). Si se resuelve con herencia, sale una clase por cada combinación:

```text
Remote
 ├── TvRemote
 ├── RadioRemote
 ├── AdvancedTvRemote
 └── AdvancedRadioRemote
```

Si añadimos un proyector, son dos clases más. Si añadimos un «mando por voz», una más por cada dispositivo. El número de clases crece como un **producto** (N mandos × M dispositivos) y el código se duplica.

La causa es que la clase intenta ampliarse a la vez **en dos direcciones independientes** (tipo de mando y tipo de dispositivo).

## Solución

Sustituir la herencia por **composición**. Una dirección («qué hacemos»: mando/informe) se convierte en la jerarquía de la **abstracción**, y la otra («cómo lo hacemos»: dispositivo/formato), en la jerarquía de la **implementación**. La abstracción guarda una referencia al objeto de implementación y le delega el trabajo.

Ahora hay N + M clases, y cualquier mando funciona con cualquier dispositivo.

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Abstraction** | `Remote`, `Report` | Lógica de control de alto nivel. Guarda una referencia al Implementor y le delega el trabajo |
| **RefinedAbstraction** | `AdvancedRemote`, `SalesReport`, `InventoryReport` | Amplía la abstracción sin tocar las implementaciones |
| **Implementor** | `IDevice`, `IReportRenderer` | Interfaz de la implementación: un conjunto de operaciones primitivas. No tiene por qué coincidir con la interfaz de la abstracción |
| **ConcreteImplementor** | `Tv`, `Radio`, `PlainTextRenderer`, `HtmlRenderer` | Implementaciones de las primitivas dependientes de la plataforma |
| **Client** | clases de demo | Conecta la abstracción con la implementación necesaria |

## Esquema

```text
        Abstracción                         Implementador
 ┌──────────────────────┐   puente   ┌────────────────────────┐
 │ Remote               │◆─────────► │ «interface» IDevice    │
 │ - Device: IDevice    │            │ Enable(), Disable()    │
 │ + TogglePower()      │            │ SetVolume(), SetChannel│
 │ + VolumeUp()         │            └────────────────────────┘
 └──────────────────────┘                  ▲          ▲
            ▲                              │          │
 ┌──────────────────────┐                 Tv        Radio
 │ AdvancedRemote       │
 │ + Mute(), Unmute()   │
 └──────────────────────┘
```

---

## Ejemplo 1. Mandos y dispositivos (`RemoteControl/`)

**Implementor**: `IDevice` describe las operaciones **primitivas** del dispositivo: encender, apagar, ajustar el volumen y el canal.

**ConcreteImplementor**: `Tv` y `Radio` (carpeta `Devices/`) las implementan de forma distinta: el televisor tiene un volumen de 0–100, la radio de 0–20, y sus canales son una lista de emisoras que se recorre en círculo.

**Abstraction**: `Remote` (carpeta `Remotes/`) implementa las acciones de **alto nivel** mediante las primitivas del dispositivo:

```csharp
public class Remote(IDevice device)
{
    protected readonly IDevice Device = device;   // este es el «puente»

    public void VolumeUp() => Device.SetVolume(Device.Volume + 5);
    public void ChannelUp() => Device.SetChannel(Device.Channel + 1);
}
```

**RefinedAbstraction**: `AdvancedRemote` añade `Mute`, `Unmute` y `SetFavoriteChannel`. No hizo falta modificar ni `Tv` ni `Radio`: «silenciar» es simplemente `SetVolume(0)` recordando el volumen anterior.

**Client**: la demo forma las combinaciones `Remote + Tv`, `AdvancedRemote + Radio` y `AdvancedRemote + Tv`: 4 combinaciones posibles con 4 clases, y no con 4 clases-combinación aparte más la base.

## Ejemplo 2. Informes y formatos (`Reports/`)

**Tarea:** hay tipos de informe (ventas, almacén) y formatos de salida (texto plano, HTML). Mañana pueden añadir un informe de clientes y el formato PDF.

**Implementor**: `IReportRenderer` sabe dibujar las primitivas: título, fila «nombre — valor» y pie.

**ConcreteImplementor**: `PlainTextRenderer` (alineación en columnas) y `HtmlRenderer` (etiquetas) en la carpeta `Renderers/`.

**Abstraction**: la clase abstracta `Report` guarda un `IReportRenderer` y declara `Build()`.

**RefinedAbstraction**: `SalesReport` e `InventoryReport` (carpeta `ReportTypes/`) saben **qué** mostrar: el de ventas calcula el total y el de almacén marca los artículos que se agotan. **Cómo** se ve lo decide el renderizador:

```csharp
var lines = new List<string> { Renderer.RenderTitle("Inventario") };
foreach (var (item, count) in stock)
{
    var value = count < LowStockLimit ? $"{count} (¡poco!)" : count.ToString();
    lines.Add(Renderer.RenderRow(item, value));
}
```

**Client**: la demo recorre todos los renderizadores y todos los informes; la implementación se puede elegir en tiempo de ejecución (por ejemplo, según la cabecera `Accept` de la petición).

---

## Cuándo usarlo

- La clase debe ampliarse en dos (o más) direcciones independientes.
- Hay que cambiar la implementación en tiempo de ejecución.
- Se quiere dividir una clase «monolítica» con varias variantes de una misma funcionalidad (por ejemplo, trabajo con distintas BD/plataformas).
- Código multiplataforma: la abstracción es la interfaz gráfica/lógica y la implementación, la API de un sistema operativo concreto.

## Cuándo no hace falta

- Solo hay una implementación y no se prevé otra: sería un nivel de abstracción superfluo.

## Ventajas y desventajas

**Ventajas**
- El número de clases crece como una suma, no como un producto.
- La abstracción y la implementación evolucionan de forma independiente (Open/Closed).
- El cliente trabaja solo con la abstracción de alto nivel, sin ver los detalles de la plataforma.
- La implementación se puede sustituir en tiempo de ejecución.

**Desventajas**
- Complica el código si en realidad solo hay una jerarquía.

## Relación con otros patrones

- **Adapter** se aplica a clases incompatibles ya existentes para que funcionen juntas. **Bridge** se diseña de antemano para que las partes evolucionen de forma independiente.
- **Strategy** tiene una estructura parecida (un objeto delega el trabajo en otro a través de una interfaz), pero resuelve otro problema: sustituir un algoritmo. En Bridge la propia abstracción también es una jerarquía.
- **Abstract Factory** puede crear parejas coherentes «abstracción + implementación».
- **Builder** + **Bridge**: el director hace el papel de abstracción y los builders, el de implementaciones.
