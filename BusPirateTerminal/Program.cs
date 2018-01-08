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
using CommandLineParser.Exceptions;

namespace BusPirateTerminal
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var consola = new Consola();
            consola.MsgPresentacion();

            var parser = new CommandLineParser.CommandLineParser();
            var param = new Parametros(consola.Version);

            try
            {
                parser.ExtractArgumentAttributes(param);
                parser.ParseCommandLine(args);
                param.SeleccionParametros(parser, param);
            }
            catch (CommandLineException)
            {
                Parametros.MostrarAyudaParametros(parser, param);
            }
            catch (Exception)
            {
                Parametros.MostrarAyudaParametros(parser, param);
            }
        }
    }
}