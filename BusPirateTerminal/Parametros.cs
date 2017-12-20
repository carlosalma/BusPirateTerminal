//
// Parametros.cs: Gestión de los parámetros
//
// Authors:
//   Carlos Alonso (carlos@carlosalma.es)
//
// Copyright (C) Apache License Version 2.0 (http://www.apache.org/licenses)
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using CommandLineParser.Arguments;
using System.IO.Ports;
using System.Collections.Generic;

// TODO: verificar los textos con las definiciones de los parámetros de comunicación.
// TODO: traducir los textos.
// TODO: sacar los textos de esta clase

// TODO: Anadir al texto de ayuda
/* Paridad
Even - Establece el bit de paridad para que el recuento de bits definidos es un número par.
Mark - Deja el bit de paridad que se establece en 1.
None - Se produce ninguna comprobación de paridad.
Odd - Establece el bit de paridad para que el recuento de bits establecidos sea un número impar.
Space - Deja el bit de paridad establecido en 0.
*/

// TODO: Anadir al texto de ayuda
/* Velocidad
110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 56000, 57600, 115200, 128000, 153600, 230400, 256000, 230400, 256000, 460800, 921600
*/


namespace BusPirateTerminal
{
    /// <summary>
    ///   Define los parámetros
    /// </summary>
    class Parametros
    {
        enum PosibleParity {Even, Mark, None, Odd, Space };

        //
        #region ParametrosDisponibles
        //
        private string modo;
        [EnumeratedValueArgument(typeof(string), 'm', "modo",
            AllowedValues = "pirate;manual",
            Description =
                "Modos de configuración: <pirate> auto-configuración, <manual> configuración manual.",
            IgnoreCase = true,
            FullDescription =
                "Existen dos modos de configuración, el modo 'pirate' que configura automáticamente \n " +
                "los parámetros de comunicación con 'Bus Pirate' y el modo 'manual', que requiere \n " +
                "de la introducción de todos los parámetros de comunicación.",
            Example = "\n" +
                "- Modo automático: BusPirateTerminal.exe -m pirate \n" +
                "- Modo manual: BusPirateTerminal.exe - m manual -p COM3 -s 115200 -a none -b 8 -i 1")]
        public string Modo { get => modo; set => modo = value; }
        //
        private string port;
        [ValueArgument(typeof(string), 'p', "port", Description = "Número de puerto COM")]
        public string Port { get => port; set => port = value; }
        //
        private int speed;
        [ValueArgument(typeof(int), 's', "speed", Description = "Velocidad de comunicación en bps")]
        public int Speed { get => speed; set => speed = value; }
        //
        private String parity;
        [ValueArgument(typeof(String), 'a', "parity", Description = "Bit de paridad")]
        public String Parity { get => parity; set => parity = value; }
        //
        private int combits;
        [ValueArgument(typeof(int), 'b', "combits", Description = "Número de bits de comunicación")]
        public int Combits { get => combits; set => combits = value; }
        //
        private StopBits stopbits;
        [ValueArgument(typeof(StopBits), 'i', "stopbits", Description = "Bit de parada")]
        public StopBits Stopbits { get => stopbits; set => stopbits = value; }
        //
        private bool help;
        [SwitchArgument('h', "help", true, Description = "Esta ayuda")]
        public bool Help { get => help; set => help = value; }
        //
        private bool version;
        [SwitchArgument('v', "version", true, Description = "Versión")]
        public bool Version { get => version; set => version = value; }
        //
        public string Cabecera { get; }
        public string Pie { get; }
        //
        #endregion
        //

        //
        // Constructor
        //
        public Parametros(string version)
        {
            Cabecera = "\n" +
                "Mini terminal de puerto serie que permite establecer comunicación con \n" +
                "el interface de 'Bus Pirate'. \n";

            Pie = $"Autor: Carlos AlMa - 2017 - ({version}) \n";
        }

        //
        // Métodos
        //

        /// <summary>
        ///   Muestra la ayuda sobre el uso de los parámetros
        /// </summary>
        /// <param name="parser">
        ///   Parser, analiza los parámetros pasados por línea
        ///   de comandos.
        /// </param>
        /// <param name="param">
        ///   Parámetros ppasados por línea de comandos
        /// </param>
        public void MostrarAyudaParametros(CommandLineParser.CommandLineParser parser, Parametros param)
        {
            parser.ShowUsageHeader = param.Cabecera;
            parser.ShowUsageFooter = param.Pie;
            parser.ShowUsage();
        }

        /// <summary>
        ///   Acciones a realizar con los parámetros.
        ///   Verifica si los parámetros introducidos están dentro 
        ///   de rango, de lo contrario asigna valores por defecto
        ///   o muestra mensajes de error.
        /// </summary>
        /// <param name="parser">
        ///   Parser, analiza los parámetros pasados por línea
        ///   de comandos.
        /// </param>
        /// <param name="param">
        ///   Parámetros ppasados por línea de comandos
        /// </param>
        public void SeleccionParametros(CommandLineParser.CommandLineParser parser, Parametros param)
        {

            Consola consola = new Consola();

            // Acceso a los valores de los parámetros
            if (param.Modo == "pirate")
            {
                SerialCom conexionSerie = new SerialCom();
                conexionSerie.Conectar();
            }

            if (param.Modo == "manual")
            {
                bool ok = true;
                string paramPort = null;
                int paramSpeed = 0;
                //
                if (param.Port == null)
                {
                    Console.WriteLine(value: $"{consola.Prompt}No se ha introducido el número de puerto.");
                    ok = false;
                }
                else
                {
                    // TODO: Verificar que la entrada está dentro de rango
                    // TODO: Conversión a mayusculas si procede
                    paramPort = param.Port;
                }
                //
                if (param.Speed > 0)
                {
                    List<int> posibleSpeed = new List<int>();
                    posibleSpeed.Add(item: 110);
                    posibleSpeed.Add(item: 300);
                    posibleSpeed.Add(item: 600);
                    posibleSpeed.Add(item: 1200);
                    posibleSpeed.Add(item: 2400);
                    posibleSpeed.Add(item: 4800);
                    posibleSpeed.Add(item: 9600);
                    posibleSpeed.Add(item: 14400);
                    posibleSpeed.Add(item: 19200);
                    posibleSpeed.Add(item: 28800);
                    posibleSpeed.Add(item: 38400);
                    posibleSpeed.Add(item: 56000);
                    posibleSpeed.Add(item: 57600);
                    posibleSpeed.Add(item: 115200);
                    posibleSpeed.Add(item: 128000);
                    posibleSpeed.Add(item: 230400);
                    posibleSpeed.Add(item: 256000);
                    posibleSpeed.Add(item: 230400);
                    posibleSpeed.Add(item: 256000);
                    posibleSpeed.Add(item: 460800);
                    posibleSpeed.Add(item: 921600);

                    if (posibleSpeed.Contains(param.Speed))
                    {
                        paramSpeed = param.Speed;
                    }
                    else
                    {
                        paramSpeed = 115200;
                        Console.WriteLine(value: $"{consola.Prompt}Asignada velocidad {paramSpeed}");
                    }
                }
                else
                {
                    paramSpeed = 115200;
                    Console.WriteLine(value: $"{consola.Prompt}Asignada velocidad {paramSpeed}");
                }
                //
                if (param.Parity == null)
                {
                    Console.WriteLine(value: $"{consola.Prompt}No se ha introducido bit de paridad");
                    // TODO: asignar bit de paridad none
                }
                else
                {
                    // TODO: veriifcar si el valor introducido está en el enumerado
                    
                }
                //


                if (ok)
                {
                    SerialCom conexionSerie = new SerialCom();
                    conexionSerie.Conectar();
                }
            }

            if (!param.Help)
            {
                param.MostrarAyudaParametros(parser, param);
            }
        }
    }
}
