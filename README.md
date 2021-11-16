![alt text](logo.svg "Logo")
# AnimacionesWF
Da la posibilidad de crear animaciones para controles mediante XML.
# Estructura XML
La jerarquia de nodos XML el al siguiente:
```xml
<?xml version="1.0" encoding="UTF-8"?>
<animation name="Basic structure">
    <group ms="0">
        <moduleName attr="">
            <propertie>value</propertie>
        </moduleName>
        ...
    </group>
    ...
</animation>
```
# Tags XML
## \<animation>
Es obligatorio, y define el elemento raiz del fichero XML
### Atributos
- name: El nombre de la animacion. No es obligatorio.
## \<group>
Un grupo contiene un conjunto de modulos. Los modulos que estén dentro de un grupo se reproduciran a la vez, y tardaran en ejecutar su secuencia en el tiempo definido por el atributo `ms` del grupo.

### Atributos
| Atributo | Tipo de valor | Obligatorio | Por defecto | Descripción |
|----------|:----------:|:-----------:|:-----------:|-------------|
| ms       | Int        | Si          |0            | El tiempo en milisegundos que va a durar la ejecución del grupo.
| loops    | Int        | No          |0            |El número de veces que se va a repetir dicho grupo. Ej.: "1" repetira 1 vez más el grupo.             |
| startSignal | String | No | "" | Invoca un evento al iniciarse el grupo. El evento enviara un String con el valor que hayamos definido en el atrbituto. |
| endSignal | String | No | "" | Invoca un evento al acabarse el grupo. El evento enviara un String con el valor que hayamos definido en el atrbituto. |

 ### Ejemplos
 ```xml
<group ms="2000" loops="1" startSignal="comienzo" endSignal="final">
    <pos>
        ...
    </pos>
    ...
</group>
```

# Modulos
## Built-in
AnimacionesWF ya viene incorporado con dos modulos para poder empezar a crear animaciones, estos son:
 - [Pos](doc/PositionModule.md)
 - [Size](doc/SizeModule.md)

## Desarrollar modulos
AnimacionesWF viene con una clase para que puedas crear tus propios modulos. Para ello, crea una clase que herede de `AnimationModule`, por ejemplo:
```c#
public class PositionAnimationModule : AnimationModule{
    //...
}
```

### XMLTagName
Es importante definir con que nombre quieres que se reconozca tu modulo. Para ello, definimos el atributo encima de la declaración de la clase:
```c#
    [XMLTagName("pos")]
    public class PositionAnimationModule : AnimationModule
```

### Constructor
Sera necesario crear un cosntructor que llama al base, el cual recibe un XElement, del cual parsearemos la información del nodo de nuestra animación
```c#
public PositionAnimationModule(XElement root) : base(root)
```

El objeto XElement representa el tag XML correspondiente a nuestro modulo.

### Función prepare
Nos sirve para preparar cualquier calculo necesario antes de empezar la animación. Se reciben los parametros `control` (El control sobre el que se va a ejecutar la animación) y `núm. de movimientos` (Cantidad de movimientos que se van a realizar).

### Función setpForward
Es llamada en cada movimiento del grupo actual. Por ejemplo, en el modulo PositionAnimationModule, en cada stepForward se le suma al eje x un valor para llegar a la posición objetivo.

### Consejos para los tipos de valores
Se recomienda el uso de los siguientes tipos de valores para tus modulos. Estos tipos de valores dan más fexibilidad y posibilidades a su funcionalidad:
- Absolutos: Establecen el valor definido. Por ejemplo, en el caso de la posición de un control si definimos el valor `412` para el eje de las x, el control se situara en la posicion 412 para la x
- Relativos: Modifican el valor actual en base al definido. Por ejemplo, en el caso de la posición de un control si definimos el valor `+100` para el eje de las x, el control se posicionara en la x con valor actual + 100
- Congelado/Sin definir: Mantiene el valor actual. En XML si un valor no es definido, es por defecto congelado.
- Porcentajes
