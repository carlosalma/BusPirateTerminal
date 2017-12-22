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

            CommandLineParser.CommandLineParser parser = new CommandLineParser.CommandLineParser();
            Parametros param = new Parametros(consola.Version);

            try
            {
                parser.ExtractArgumentAttributes(param);
                parser.ParseCommandLine(args);
                param.SeleccionParametros(parser, param);
            }
            catch (CommandLineException)
            {
                param.MostrarAyudaParametros(parser, param);
            }
            catch (Exception)
            {
                param.MostrarAyudaParametros(parser, param);
            }
        }
    }
}