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
La lista de animaciones soportadas son:
 - [Pos](doc/AnimacionPos.md)
 - [Size](doc/AnimacionSize.md)
