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
using System.Collections.Generic;
using CommandLineParser.Arguments;

// TODO: Reponer todos las llaves de los if

namespace BusPirateTerminal
{
    /// <summary>
    ///     Define los parámetros
    /// </summary>
    internal class Parametros
    {
        //
        private readonly List<string> _patrones = new List<string>();
        public int ParamDataBits;
        public string ParamParity;

        // Parámetros de comunicación
        public string ParamPort;
        public int ParamSpeed;

        public string ParamStopBits;

        //
        // Constructor
        //
        public Parametros(string version)
        {
            Cabecera = "\n" +
                       "Terminal de comunicación via puerto serie, orientado a la comunicación \n" +
                       "con el dispositivo 'Pirate Bus' de 'Dangerous Prototypes'. \n\n" +
                       "Existen dos modos de empleo: \n\n" +
                       "- Modo automático (sin parámetros), el terminal se configura con los parámetros \n" +
                       "  por defecto necesarios y, localiza el puerto serie (COM) al que se ha conectado el \n" +
                       "  dispositivo. \n\n" +
                       "- Modo manual, es necesario introducir los parámetros de comunicación manualmente. \n" +
                       "  Si se omite un parámetro, este se sustituye por el valor por defecto. \n\n" +
                       "Ejemplos: \n\n" +
                       "- Modo automático: BusPirateTerminal.exe \n" +
                       "- Modo manual: BusPirateTerminal.exe -p 3 -s 115200 -a none -b 8 -i one" +
                       "\n";

            Pie = "\n" +
                  "Velocidades de comunicación: \n" +
                  "  110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 56000, \n" +
                  "  57600, 115200, 128000, 153600, 230400, 256000, 230400, 256000, 460800, 921600 \n\n" +
                  "Bits de paridad: \n" +
                  "- Even:  Establece el bit de paridad para que el recuento de bits definidos es un número par. \n" +
                  "- Mark:  Deja el bit de paridad que se establece en 1. \n" +
                  "- None:  Se produce ninguna comprobación de paridad. \n" +
                  "- Odd:   Establece el bit de paridad para que el recuento de bits establecidos sea impar. \n" +
                  "- Space: Deja el bit de paridad establecido en 0. \n\n" +
                  "Bits de datos: 5, 7, 8 \n\n" +
                  "Bits de parada: none, one, onepointfive, two \n\n" +
                  $"Autor: Carlos AlMa - 2017 - ({version}) \n";
            //
            ParamPort = "";
            ParamSpeed = 115200;
            ParamParity = "none";
            ParamDataBits = 8;
            ParamStopBits = "one";
            //
            _patrones.Add(@"usbserial");
            _patrones.Add(@"COM\d");
        }
        //

        private string Cabecera { get; }
        private string Pie { get; }

        //
        // Métodos
        //

        /// <summary>
        ///     Muestra la ayuda sobre el uso de los parámetros
        /// </summary>
        /// <param name="parser">
        ///     Parser, analiza los parámetros pasados por línea
        ///     de comandos.
        /// </param>
        /// <param name="param">
        ///     Parámetros ppasados por línea de comandos
        /// </param>
        public static void MostrarAyudaParametros(CommandLineParser.CommandLineParser parser, Parametros param)
        {
            parser.ShowUsageHeader = param.Cabecera;
            parser.ShowUsageFooter = param.Pie;
            parser.ShowUsage();
        }

        /// <summary>
        ///     Acciones a realizar con los parámetros.
        ///     Verifica si los parámetros introducidos están dentro
        ///     de rango, de lo contrario asigna valores por defecto
        ///     o muestra mensajes de error.
        /// </summary>
        /// <param name="parser">
        ///     Analiza los parámetros pasados por línea de comandos.
        /// </param>
        /// <param name="param">
        ///     Parámetros pasados por línea de comandos.
        /// </param>
        public void SeleccionParametros(CommandLineParser.CommandLineParser parser, Parametros param)
        {
            var consola = new Consola();

            // Ayuda
            if (param.Help) MostrarAyudaParametros(parser, param);

            // Listado de puertos
            if (param.ListCom) consola.MsgListadoPuertos();

            // Flujo de configuración manual
            if (ParamPort == null || param.Help || param.ListCom) return;

            // TODO: Verificar el funcionamiento de la verificación y asignación de puerto COM
            // TODO: Verificar el funcionamimento del listado de puertos en Windows

            // Puerto
            ValidaParamPort(param.Port, _patrones);
            Console.WriteLine($"{consola.Prompt}Puerto: {ParamPort}");

            // Velocidad
            ValidaParamSpeed(param.Speed);
            Console.WriteLine($"{consola.Prompt}Velocidad: {ParamSpeed}");

            // Paridad
            ValidaParidad(param.Parity);
            Console.WriteLine($"{consola.Prompt}Bit de paridad: {ParamParity}");

            // Bits de datos
            ValidaDataBits(param.DataBits);
            Console.WriteLine($"{consola.Prompt}Bits de comunicaciones: {ParamDataBits}");

            // Bits de parada
            ValidaStopBits(param.StopBits);
            Console.WriteLine($"{consola.Prompt}Asignado bit de parada: {ParamStopBits}");

            // >>> CONEXIÓN <<<
            var conexionSerie = new SerialCom(ParamPort, ParamSpeed, ParamParity, ParamDataBits, ParamStopBits);
            if (param.Info && conexionSerie.ComOk) consola.MostrarParametros(conexionSerie);

            conexionSerie.Conectar();
        }

        /// <summary>
        ///     Si se indica un ID de puerto, se emplea para establecer la conexión
        ///     con el dispositivo, de lo contrario, se intenta localizar un
        ///     dispositivo cuyo nombre disponga de un patron de texto determinado.
        /// </summary>
        /// <param name="selecPort">
        ///     Número de puerto COM.
        /// </param>
        /// <param name="patrones">
        ///     Matriz que contiene los patrones de texto a localizar en el nombre
        ///     del dispositivo.
        /// </param>
        public void ValidaParamPort(int selecPort, IEnumerable<string> patrones)
        {
            // TODO: verificar en windows el método de busqueda de puerto
            // TODO: Intentar eliminar paramPort = conexionSerie.ComPort;

            var consola = new Consola();

            if (selecPort > 0)
            {
                var conexionSerie = new SerialCom(selecPort);
                ParamPort = conexionSerie.ComPort;
            }
            // Si no se ha especificado ningún Id de puerto
            else
            {
                // Realiza la busqueda de dispositivo por patrón
                foreach (var patron in patrones)
                {
                    var conexionSerie = new SerialCom(patron);

                    // Si se localiza una conexión que coincida con el patrón
                    if (conexionSerie.ComPort != null)
                    {
                        ParamPort = conexionSerie.ComPort;
                        Console.WriteLine($"{consola.Prompt}Localizado puerto con el patrón: {patron}");
                        return;
                    }
                }
            }
        }

        /// <summary>
        ///     Verifica si la velocidad introducida, se corresponde con algun
        ///     valor normalizado. En caso contrario se asigna por defecto el
        ///     valor "115200", que es el emleado por BusPirate.
        /// </summary>
        /// <param name="paramSpeed">
        ///     Velocidad de comunicación.
        /// </param>
        public void ValidaParamSpeed(int paramSpeed)
        {
            var posibleSpeed = new List<int>
            {
                110,
                300,
                600,
                1200,
                2400,
                4800,
                9600,
                14400,
                19200,
                28800,
                38400,
                56000,
                57600,
                115200,
                128000,
                230400,
                256000,
                230400,
                256000,
                460800,
                921600
            };

            if (posibleSpeed.Contains(paramSpeed)) ParamSpeed = paramSpeed;
        }

        /// <summary>
        ///     Verifica si la paridad introducida, se corresponde
        ///     con algun valor normalizado. En caso contrario se
        ///     asigna por defecto el valor "none", que es el
        ///     emleado por BusPirate.
        /// </summary>
        /// <param name="paramParity">
        ///     Bit de paridad
        /// </param>
        public void ValidaParidad(string paramParity)
        {
            var posibleParity = new List<string> {"even", "mark", "none", "odd", "space"};

            if (posibleParity.Contains(paramParity)) ParamParity = paramParity;
        }

        /// <summary>
        ///     Verifica si el número de bits de datos, se corresponde
        ///     con algun valor normalizado. En caso contrario se asigna
        ///     por defecto el valor "8", que es el emleado por BusPirate.
        /// </summary>
        /// <param name="paramDataBits">
        ///     Número de bits de datos
        /// </param>
        public void ValidaDataBits(int paramDataBits)
        {
            var posibleDataBits = new List<int> {5, 7, 8};

            if (posibleDataBits.Contains(paramDataBits)) ParamDataBits = paramDataBits;
        }

        /// <summary>
        ///     Verifica si el numero de bits de parada, se corresponde
        ///     con algun valor normalizado. En caso contrario se asigna
        ///     por defecto el valor "one", que es el emleado por BusPirate.
        /// </summary>
        /// <param name="paramStopBits">
        ///     Número de bits de parada
        /// </param>
        public void ValidaStopBits(string paramStopBits)
        {
            var posibleStopBits = new List<string> {"none", "one", "onepointfive", "two"};

            if (posibleStopBits.Contains(paramStopBits)) ParamStopBits = paramStopBits;
        }

        //

        #region ParametrosDeEntrada

        //
        [ValueArgument(typeof(int), 'p', "port", Description = "Número de puerto COM")]
        public int Port { get; set; }

        //
        [ValueArgument(typeof(int), 's', "speed", Description = "Velocidad de comunicación en bps")]
        public int Speed { get; set; }

        //
        [ValueArgument(typeof(string), 'a', "parity", Description = "Bit de paridad")]
        public string Parity { get; set; }

        //
        [ValueArgument(typeof(int), 'b', "combits", Description = "Número de bits de comunicación")]
        public int DataBits { get; set; }

        //
        [ValueArgument(typeof(string), 'i', "stopbits", Description = "Bit de parada")]
        public string StopBits { get; set; }

        //
        [SwitchArgument('f', "info", false, Description = "Información sobre los parámetros de la conexión.")]
        public bool Info { get; set; }

        //
        [SwitchArgument('l', "list", false, Description = "Listado de puertos COM disponibles.")]
        public bool ListCom { get; set; }

        //
        [SwitchArgument('h', "help", false, Description = "Esta ayuda.")]
        public bool Help { get; set; }

        //

        #endregion

        //
    }
}