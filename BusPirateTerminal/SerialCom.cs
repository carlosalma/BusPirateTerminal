//
// SerialCom.cs: Comunicaciones mediante puerto serie.
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
using System.IO.Ports;
using System;
using System.Threading;

// TODO: Convertir en parámetro el rango de puertos a escanear en el método SerialCom()

namespace BusPirateTerminal
{
    class SerialCom
    {

        //
        // Propiedades
        //

        public string ComPort { get; set; }
        public int ComSpeed { get; set; }
        public Parity ComParity { get; set; }
        public int ComBits { get; set; }
        public StopBits ComStopBits { get; set; }
        public bool ComOk { get; set; }
        
        //
        // Constructores
        //

        /// <summary>
        ///   Auto configuración para BusPirate.
        ///   Dispone de todos los parámetros por defecto
        ///   y localiza el puerto COM empleado.
        /// </summary>
        public SerialCom()
        {
            ComSpeed = 115200;
            ComParity = Parity.None;
            ComBits = 8;
            ComStopBits = StopBits.One;
            //
            ComPort = BucaPuertoCom(portIni: 1, portFin: 9);
            if (ComPort == null)
            {
                ComOk = false;
            }
            else
            {
                ComOk = true;
            }
        }

        /// <summary>
        ///   Entrada de parámetros de comunicación via parámetro.
        /// </summary>
        /// <param name="comPort"></param>
        /// <param name="comSpeed"></param>
        public SerialCom(string comPort, int comSpeed)
        {
            ComPort = comPort;
            ComSpeed = comSpeed;
            // TODO: completar la carga de parámetros
            ComParity = Parity.None;
            ComBits = 8;
            ComStopBits = StopBits.One;

            if (!VerificaPuertoComDisponible(comPort))
            {
                // Puerto no disponible
                ComPort = null;
                ComOk = false;
            }
            else
            {
                // Puerto disponible
                ComPort = comPort;
                ComOk = true;
            }
        }

        // TODO: Completado el constructor anterior, eliminar este
        /// <summary>
        ///   Configuración manual.
        ///   Hay que introducir todos los parámetros
        /// </summary>
        /// <param name="comPort"></param>
        /// <param name="comSpeed"></param>
        /// <param name="comParity"></param>
        /// <param name="comBits"></param>
        /// <param name="comStopBits"></param>
        public SerialCom(string comPort, int comSpeed, Parity comParity, int comBits, StopBits comStopBits)
        {
            ComSpeed = comSpeed;
            ComParity = comParity;
            ComBits = comBits;
            ComStopBits = comStopBits;

            if (! VerificaPuertoComDisponible(comPort))
            {
                // Puerto no disponible
                ComPort = null;
                ComOk = false;
            }
            else
            {
                // Puerto disponible
                ComPort = comPort;
                ComOk = true;
            }
        }

        //
        // Métodos
        //

        public bool Conectar()
        {
            Consola consola = new Consola();

            if (ComOk)
            {
                consola.MsgConexionEstablecida(ComPort);

                using (var serialPort = new SerialPort(ComPort, ComSpeed, ComParity, ComBits, ComStopBits))
                {
                    try
                    {
                        serialPort.Open();
                    }
                    catch (System.IO.IOException)
                    {
                        Console.WriteLine(value: $"{consola.Prompt}Puerto {ComPort} NO DISPONIBLE");
                        //return;
                    }

                    bool ok = true;
                    new Thread(() => ok = EscribirLineasDesde(serialPort)).Start();

                    if (!ok)
                        Console.WriteLine(value: $"{consola.Prompt}Cerrando consola ...");

                    while (true)
                    {
                        Console.Write(value: consola.Prompt);
                        var command = Console.ReadLine();

                        try
                        {
                            if ((command == "quit") || (command == "QUIT"))
                                break;

                            serialPort.WriteLine(command);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(value: $"{consola.Prompt} {e}");
                            // return;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(value: $"{consola.Prompt}Conexión: NO ESTABLECIDA");
            }

            return true;
        }

        /// <summary>
        ///   Muestra por consola la respuesta del dispositivo
        ///   conectado al puerto COM.
        /// </summary>
        /// <param name="comPort">
        ///   Puerto COM empleado
        /// </param>
        /// <returns></returns>
        private bool EscribirLineasDesde(SerialPort comPort)
        {
            bool ok = true;

            try
            {
                while (true) Console.WriteLine(comPort.ReadLine());
            }
            catch (Exception)
            {
                ok = false;
            }
            return ok;
        }

        /// <summary>
        ///   Verifica si la conexión con el puerto COM está disponible.
        /// </summary>
        /// <param name="comPort"></param>
        /// <returns>Conexión disponible</returns>
        private bool VerificaPuertoComDisponible(string comPort)
        {
            bool ok = true;

            using (var serialPort = new SerialPort(comPort, ComSpeed, ComParity, ComBits, ComStopBits))
            {
                try
                {
                    if(!serialPort.IsOpen)
                    {
                        serialPort.Open();
                    }
                }
                catch (System.IO.IOException)
                {
                    ok = false;
                }
                catch (System.ObjectDisposedException)
                {
                    ok = false;
                }
                finally
                {
                    if(serialPort.IsOpen)
                    {
                        serialPort.Close();
                    }
                }
                return ok;
            }
        }

        /// <summary>
        ///   Busca el puerto COM disponible dentro de un rango dado.
        /// </summary>
        /// <param name="portIni">Número de puerto inicial</param>
        /// <param name="portFin">Número de puerto final</param>
        /// <returns>Numero del puerto localizado</returns>
        public string BucaPuertoCom(int portIni, int portFin)
        {
            string _portName = "COM";
            string _portSearch = "";
            string _puertoLocalizado = null;
            bool _portOk = false;

            for (int port = portIni; port <= portFin; port++)
            {
                _portSearch = _portName + port.ToString();
                _portOk = VerificaPuertoComDisponible(_portSearch);

                if (_portOk)
                {
                    _puertoLocalizado = _portSearch;
                    break;
                }
            }
            return _puertoLocalizado;
        }
    }
}