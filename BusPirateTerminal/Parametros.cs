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
using System.Collections.Generic;

namespace BusPirateTerminal
{
    /// <summary>
    ///   Define los parámetros
    /// </summary>
    class Parametros
    {
        //
        #region ParametrosDeEntrada
        //
        private int port;
        [ValueArgument(typeof(int), 'p', "port", Description = "Número de puerto COM")]
        public int Port { get => port; set => port = value; }
        //
        private int speed;
        [ValueArgument(typeof(int), 's', "speed", Description = "Velocidad de comunicación en bps")]
        public int Speed { get => speed; set => speed = value; }
        //
        private String parity;
        [ValueArgument(typeof(String), 'a', "parity", Description = "Bit de paridad")]
        public String Parity { get => parity; set => parity = value; }
        //
        private int dataBits;
        [ValueArgument(typeof(int), 'b', "combits", Description = "Número de bits de comunicación")]
        public int DataBits { get => dataBits; set => dataBits = value; }
        //
        private String stopBits;
        [ValueArgument(typeof(String), 'i', "stopbits", Description = "Bit de parada")]
        public String StopBits { get => stopBits; set => stopBits = value; }
        //
        private bool info;
        [SwitchArgument('f', "info", false, Description = "Información sobre los parámetros de la conexión.")]
        public bool Info { get => info; set => info = value; }
        //
        private bool help;
        [SwitchArgument('h', "help", true, Description = "Esta ayuda.")]
        public bool Help { get => help; set => help = value; }
        //
        #endregion
        //

        public string Cabecera { get; }
        public string Pie { get; }
        // Parámetros de comunicación
        public string ParamPort;
        public int ParamSpeed;
        public string ParamParity;
        public int ParamDataBits;
        public string ParamStopBits;

        //
        // Constructor
        //
        public Parametros(string version)
        {
            Cabecera = "\n" +
                "Terminal de comunicación via puerto serie (COM), orientado a la comunicación \n" +
                "con el dispositivo 'Pirate Bus' de 'SparkFun'. \n\n" +

                "Existen dos modos de empleo: \n\n" +

                "- Modo automático (sin parámetros), el terminal se configura con los parámetros \n" +
                "  por defecto necesarios y, localiza el puerto COM al que se ha conectado el \n" +
                "  dispositivo. \n\n" +

                "- Modo manual, es necesario introducir los parámetros de comunicación manualmente. \n" +
                "  Si se omite un parámetro, este se sustituye por el valor por defecto. \n\n" +

                "Ejemplos: \n\n" +

                "- Modo automático: BusPirateTerminal.exe \n" +
                "- Modo manual: BusPirateTerminal.exe -p 3 -s 115200 -a none -b 8 -i one" +
                "\n";

            Pie = $"\n" +
                "Velocidades de comunicación: \n" +
                "  110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 56000, \n" +
                "  57600, 115200, 128000, 153600, 230400, 256000, 230400, 256000, 460800, 921600 \n\n" +
        
                "Bits de paridad: \n" +
                "- Even:  Establece el bit de paridad para que el recuento de bits definidos es un número par. \n" +
                "- Mark:  Deja el bit de paridad que se establece en 1. \n" +
                "- None:  Se produce ninguna comprobación de paridad. \n" +
                "- Odd:   Establece el bit de paridad para que el recuento de bits establecidos sea un número impar. \n" +
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
        ///   Analiza los parámetros pasados por línea de comandos.
        /// </param>
        /// <param name="param">
        ///   Parámetros pasados por línea de comandos.
        /// </param>
        public void SeleccionParametros(CommandLineParser.CommandLineParser parser, Parametros param)
        {
            Consola consola = new Consola();
            var noHelp = false;

            if (param.help)
            {
                ValidaParamPort(param.Port, 0, 13);
                Console.WriteLine(value: $"{consola.Prompt}Puerto: {ParamPort}");
                noHelp = true;
            }
            else
            {
                param.MostrarAyudaParametros(parser, param);
            }

            if ((ParamPort != null) && (noHelp))
            {
                // Velocidad
                ValidaParamSpeed(param.Speed);
                Console.WriteLine(value: $"{consola.Prompt}Velocidad: {ParamSpeed}");

                // Paridad
                ValidaParidad(param.Parity);
                Console.WriteLine(value: $"{consola.Prompt}Bit de paridad: {ParamParity}");

                // Bits de datos
                ValidaDataBits(param.DataBits);
                Console.WriteLine(value: $"{consola.Prompt}Bits de comunicaciones: {ParamDataBits}");

                // Bits de parada
                ValidaStopBits(param.StopBits);
                Console.WriteLine(value: $"{consola.Prompt}Asignado bit de parada: {ParamStopBits}");

                // Conexión
                SerialCom conexionSerie = new SerialCom(ParamPort, ParamSpeed, ParamParity, ParamDataBits, ParamStopBits);
                if (param.Info)
                {
                    Console.WriteLine(value: $"{consola.Prompt}{conexionSerie.MostrarParametros()}");
                }
                conexionSerie.Conectar();
            }
            else if (noHelp)
            {
                Console.WriteLine(value: $"{consola.Prompt}No hay puerto disponible");
            }
        }

        /// <summary>
        ///   Convierte el número de puerto pasado como parámetro
        ///   en el correspondiente puerto COM.
        ///   Si no se indica ningún numero de puerto, se realiza
        ///   un escan de puertos para localizar algúno en uso.
        /// </summary>
        /// <param name="paramPort">
        ///   Número de puerto COM.
        /// </param>
        /// <param name="portIni">
        ///   Número de puerto de inicio al realizar el escan de puertos.
        /// </param>
        /// <param name="portFin">
        ///   Número de puerto final al realizar el escan de puertos.
        /// </param>
        public void ValidaParamPort(int paramPort, int portIni, int portFin)
        {
            if (paramPort > 0)
            {
                ParamPort = "COM" + Convert.ToString(paramPort);                
            }
            else
            {
                SerialCom conexionSerie = new SerialCom(portIni, portFin);
                ParamPort = conexionSerie.BucaPuertoCom();
            }
        }

        /// <summary>
        ///   Verifica si la velocidad introducida, se corresponde 
        ///   con algun valor normalizado. En caso contrario se
        ///   asigna por defecto el valor "115200", que es el 
        ///   emleado por BusPirate.
        /// </summary>
        /// <param name="paramSpeed">
        ///   Velocidad de comunicación.
        /// </param>
        public void ValidaParamSpeed(int paramSpeed)
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
            
            if (posibleSpeed.Contains(paramSpeed))
            {
                ParamSpeed = paramSpeed;
            }
        }

        /// <summary>
        ///   Verifica si la paridad introducida, se corresponde 
        ///   con algun valor normalizado. En caso contrario se
        ///   asigna por defecto el valor "none", que es el 
        ///   emleado por BusPirate.
        /// </summary>
        /// <param name="paramParity">
        ///   Bit de paridad
        /// </param>
        public void ValidaParidad(string paramParity)
        {
            List<string> posibleParity = new List<string>();
            posibleParity.Add(item: "even");
            posibleParity.Add(item: "mark");
            posibleParity.Add(item: "none");
            posibleParity.Add(item: "odd");
            posibleParity.Add(item: "space");

            if (posibleParity.Contains(paramParity))
            {
                ParamParity = paramParity;
            }
        }

        /// <summary>
        ///   Verifica si el número de bits de datos, se corresponde 
        ///   con algun valor normalizado. En caso contrario se asigna 
        ///   por defecto el valor "8", que es el emleado por BusPirate.
        /// </summary>
        /// <param name="paramDataBits">
        ///   Número de bits de datos
        /// </param>
        public void ValidaDataBits(int paramDataBits)
        {
            List<int> posibleDataBits = new List<int>();
            posibleDataBits.Add(item: 5);
            posibleDataBits.Add(item: 7);
            posibleDataBits.Add(item: 8);
            
            if (posibleDataBits.Contains(paramDataBits))
            {
                ParamDataBits = paramDataBits;
            }
        }

        /// <summary>
        ///   Verifica si el numero de bits de parada, se corresponde 
        ///   con algun valor normalizado. En caso contrario se asigna 
        ///   por defecto el valor "one", que es el emleado por BusPirate.
        /// </summary>
        /// <param name="paramStopBits">
        ///   Número de bits de parada
        /// </param>
        public void ValidaStopBits(string paramStopBits)
        {
            List<string> posibleStopBits = new List<string>();
            posibleStopBits.Add(item: "none");
            posibleStopBits.Add(item: "one");
            posibleStopBits.Add(item: "onepointfive");
            posibleStopBits.Add(item: "two");

            if (posibleStopBits.Contains(paramStopBits))
            {
                ParamStopBits = paramStopBits;
            }
        }
    }
}
