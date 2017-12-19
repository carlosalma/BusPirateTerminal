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

// TODO: verificar los textos con las definiciones de los parámetros de comunicación.
// TODO: traducir los textos.

namespace BusPirateTerminal
{
    /// <summary>
    ///   Define los parámetros
    /// </summary>
    class Parametros
    {
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
        private Parity parity;
        [ValueArgument(typeof(Parity), 'a', "parity", Description = "Bit de paridad")]
        public Parity Parity { get => parity; set => parity = value; }
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
        public Parametros()
        {
            Cabecera = "\n" +
                "Mini terminal de puerto serie que permite establecer comunicación con \n" +
                "el interface de 'Bus Pirate'. \n";

            Pie = "Autor: Carlos AlMa - 2017 - (Ver 0.11) \n";
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
    }
}
