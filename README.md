![alt text](logo.svg "Logo")
# AnimacionesWF
Da la posibilidad de crear animaciones para controles mediante XML.
# Tipos de valores
- Absolutos: Establecen el valor definido. Por ejemplo, en el caso de la posición de un control si definimos el valor `412` para el eje de las x, el control se situara en la posicion 412 para la x
- Relativos: Modifican el valor actual en base al definido. Por ejemplo, en el caso de la posición de un control si definimos el valor `+100` para el eje de las x, el control se posicionara en la x con valor actual + 100
- Congelado/Sin definir: Mantiene el valor actual. En XML si un valor no es definido, es por defecto congelado.

# Grupos
Los grupos definen el tiempo que van a durar mediante el atributo `ms`, que va definido en milisegundos. Todas las animaciones que esten dentro del grupo se reproduciran a la vez.
Los atributos se definen con el tag `<key-step>`.
## Atributos
 - loops: El número de veces que se va a repetir dicho grupo
    - Valores numéricos

 ## Ejemplos
 ```
<key-step loops="1">
    <pos>
        ...
    </pos>
    ...
</key-step>
```

# Animaciones

## pos
Mueve la posicion de un control a los valores especificados
### Atributos
 - hiddenstart: Define si el control empezara desde uno de los ejes del formulario, fuera del campo visual.
    - top: El control se reposicionara fuera de la parte superior del fomrulario.
    - bottom: El control se reposicionara fuera de la parte inferior del fomrulario.
    - left: El control se reposicionara fuera de la parte izquierda del fomrulario.
    - right: El control se reposicionara fuera de la parte derecha del fomrulario.
### Valores
- x: Modifica la posición x
- y: Modifica la posición y

### Ejemplos
```
<pos>
    <x>+100</x>
    <y>200</y>
</pos>
```
```
<pos hiddenstart="top">
    <y>+200</y>
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

### Ejemplos
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