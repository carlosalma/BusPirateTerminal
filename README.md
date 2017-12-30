# Bus Pirate Micro Terminal

La siguiente aplicación, esta escrita en C# y tiene carácter formativo.

Actúa como terminal de comunicación vía puerto serie, permitiendo la comunicación con la herramienta [Bus Pirate](http://dangerousprototypes.com/blog/bus-pirate-manual/) de [Dangerous Prototypes](http://dangerousprototypes.com/), o cualquier otra que emplee, este tipo de comunicación.

# Uso

El terminal dispone de dos modos de configuración:

- Modo automático (sin parámetros): el terminal se configura con los parámetros por defecto necesarios para comunicarse con Bus Piratey y localiza el puerto serie (COM) al que se ha conectado el dispositivo.
- Modo manual: requiere de los parámetros de comunicación. Si se omite un parámetro, este se sustituye por el valor por defecto.

## Ejemplos:

- Modo automático: BusPirateTerminal.exe
- Modo manual: BusPirateTerminal.exe -p 3 -s 115200 -a none -b 8 -i one

# Parámetros de comunicación serie

Velocidades de comunicación [Baud rate]

- Indica el número de bits por segundo que se transmiten y se mide en *bps*. 
- Opciones disponibles:
  
  110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 56000, 57600, 115200, 128000, 153600, 230400, 256000, 230400, 256000, 460800, 921600

Bits de paridad [Parity bits]

- Permite verificar si existen errores de transmisión de datos
- Opciones disponibles:

  * Even [par]: Establece el bit de paridad para que el recuento de bits sea par.
  * Odd [impar]: Establece el bit de paridad para que el recuento de bits sea impar.
  * Mark [marca]: Fija el bit de paridad establecido a 1.
  * Space [espacio]: Fija el bit de paridad establecido a 0.
  * None [no]: No se produce ninguna comprobación de paridad.
  
Bits de datos [Data bits]

- Tamaño del paquete de información que se envia.
- Opciones disponibles:

  5, 7, 8

Bits de parada [Stop bits]

- Indica el final de cada paquete enviado.
- Opciones disponibles:

  None, One, Onepointfive, Two

**Nota:** *El proceso de comunicación se encuentra en la clase* **SerialCom**.

## Enlaces

- [Dangerous Prototypes](http://dangerousprototypes.com/)
- [BusPirate](http://dangerousprototypes.com/blog/bus-pirate-manual/)

Autor: Carlos AlMa - 2017
