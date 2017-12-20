//
// Program.cs: Bloque principal.
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
using System.IO.Ports;
using CommandLineParser.Exceptions;

namespace BusPirateTerminal
{
    class Program
    {
        static void Main(string[] args)
        {
            Consola consola = new Consola();
            consola.MsgPresentacion();

            //
            #region GestionParametros
            //
            CommandLineParser.CommandLineParser parser = new CommandLineParser.CommandLineParser();
            Parametros param = new Parametros(consola.Version);
            
            try
            {
                parser.ExtractArgumentAttributes(param);
                parser.ParseCommandLine(args);
            }
            catch (CommandLineException)
            {
                param.MostrarAyudaParametros(parser, param);
            }

            SeleccionParametros(parser, param);

            //
            #endregion
            //
        }

        /// <summary>
        ///   Acciones a realizar con los parámetros.
        /// </summary>
        /// <param name="parser">
        ///   Parser, analiza los parámetros pasados por línea
        ///   de comandos.
        /// </param>
        /// <param name="param">
        ///   Parámetros ppasados por línea de comandos
        /// </param>
        static void SeleccionParametros(CommandLineParser.CommandLineParser parser, Parametros param)
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

                if (param.Port == null)
                {
                    Console.WriteLine("No se ha introducido el numero de puerto");
                    ok = false;
                }
                else
                {
                    // TODO: Verificar que la entrada está dentro de rango
                    // TODO: Conversión a mayusculas si procede
                    paramPort = param.Port;
                }

                if (param.Speed > 0)
                {
                    // TODO: Verificar que la entrada está dentro de rango
                    paramSpeed = param.Speed;                                    }
                else
                {
                    Console.WriteLine("No se ha introducido la velocidad");
                    ok = false;
                }

                if (param.Parity == null)
                {
                    Console.WriteLine("No se ha introducido el numero de puerto");
                    ok = false;
                }
                if (param.Port == null)
                {
                    Console.WriteLine("No se ha introducido el numero de puerto");
                    ok = false;
                }
                if (param.Port == null)
                {
                    Console.WriteLine("No se ha introducido el numero de puerto");
                    ok = false;
                }

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