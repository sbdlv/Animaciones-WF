![alt text](logo.svg "Logo")
# AnimacionesWF
Da la posibilidad de crear animaciones para controles mediante XML.
# Tipos de valores
- Absolutos: Establecen el valor definido. Por ejemplo, en el caso de la posición de un control si definimos el valor `412` para el eje de las x, el control se situara en la posicion 412 para la x
- Relativos: Modifican el valor actual en base al definido. Por ejemplo, en el caso de la posición de un control si definimos el valor `+100` para el eje de las x, el control se posicionara en la x con valor actual + 100
- Congelado/Sin definir: Mantiene el valor actual. En XML si un valor no es definido, es por defecto congelado.

# Grupos
Los grupos definen el tiempo que van a durar mediante el atributo `ms`, que va definido en milisegundos. Todas las animaciones que esten dentro del grupo se reproduciran a la vez.

# Animaciones

## pos
Mueve la posicion de un control a los valores especificados
### Valores
- x: Modifica la posición x
- y: Modifica la posición y

### Ejemplo
```
<pos>
    <x>+100</x>
    <y>200</y>
</pos>
```

## size
Modifica el tamaño de un control
### Valores
- w: Modifica el ancho del control
- h: Modifica el alto del control
### Atributos
- modo: Define como se deberia de hacer el resize.
    - normal: Simplemente se establecen los valores definidos. No es necesario definir este modo.
    - center: Mantiene el control centrado a la vez que se hace resize.

### Ejemplo
```
<size mode="normal">
    <w>+100</w>
    <h>55</h>
</size>
```
---
***Modo center*** 

Seguramente produzca errores con animaciones pos

---