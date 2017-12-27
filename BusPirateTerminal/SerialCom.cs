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
        public int ComDataBits { get; set; }
        public StopBits ComStopBits { get; set; }
        public bool ComOk { get; set; }
        public int PortIni { get; set; }
        public int PortFin { get; set; }

        //
        // Constructores
        //
        
        //
        #region Constructores
        //
        /// <summary>
        ///   Auto configuración para BusPirate.
        ///   Dispone de todos los parámetros para establecer
        ///   la comunicación, excepto el puerto COM empleado, 
        ///   que lo localiza.
        /// </summary>
        public SerialCom(int portIni, int portFin)
        {
            ComSpeed = 115200;
            ComParity = Parity.None;
            ComDataBits = 8;
            ComStopBits = StopBits.One;
            ComOk = true;
            PortIni = portIni;
            PortFin = portFin;
            //
            ComPort = BucaPuertoCom();
            if (ComPort == null)
            {
                ComOk = false;
            }
        }

        /// <summary>
        ///   Permite establecer los parámetros de comunicación.
        /// </summary>
        /// <param name="comPort">
        ///   Número de puerto COM.
        /// </param>
        /// <param name="comSpeed">
        ///   Velocidad de comunicación.
        /// </param>
        /// <param name="comParity">
        ///   Bit de paridad
        /// </param>
        /// <param name="comDataBits">
        ///   Número de bits de datos
        /// </param>
        /// <param name="comStopBits">
        ///   Número de bits de parada
        /// </param>
        public SerialCom(string comPort, int comSpeed, string comParity, int comDataBits, string comStopBits)
        {
            ComPort = comPort;
            ComSpeed = comSpeed;
            ComDataBits = comDataBits;
            ComOk = true;

            // Acopla los parámetros de paridad
            switch (comParity)
            {
                case "even":
                    ComParity = Parity.Even;
                    break;
                case "mark":
                    ComParity = Parity.Mark;
                    break;
                case "none":
                    ComParity = Parity.None;
                    break;
                case "odd":
                    ComParity = Parity.Odd;
                    break;
                case "space":
                    ComParity = Parity.Space;
                    break;
                default:
                    ComParity = Parity.None;
                    break;
            }
            
            // Acopla los parámetors de bits de parada
            switch (comStopBits)
            {
                case "none":
                    ComStopBits = StopBits.None;
                    break;
                case "one":
                    ComStopBits = StopBits.One;
                    break;
                case "onepointfive":
                    ComStopBits = StopBits.OnePointFive;
                    break;
                case "two":
                    ComStopBits = StopBits.Two;
                    break;
                default:
                    ComStopBits = StopBits.One;
                    break;
            }

            if (!VerificaPuertoComDisponible(comPort))
            {
                // Puerto no disponible
                ComPort = null;
                ComOk = false;
            }
        }
        //
        #endregion
        //

        //
        // Métodos
        //

        /// <summary>
        ///   Muestra los parámetros que se han empleado para 
        ///   establecer la comunicación.
        /// </summary>
        /// <returns>
        ///   Información sobre los parámetros
        /// </returns>
        public string MostrarParametros()
        {
            string listaParametros = $"Parámetros de la conexión: \n" +
                $" - ComPort: {ComPort} \n" +
                $" - ComSpeed: {ComSpeed} \n" +
                $" - ComParity: {ComParity} \n" +
                $" - ComBits: {ComDataBits} \n" +
                $" - ComStopBits: {ComStopBits}";
            return listaParametros;
        }

        /// <summary>
        ///   Establece el proceso de comunicación
        /// </summary>
        public void Conectar()
        {
            Consola consola = new Consola();

            if (ComOk)
            {
                consola.MsgConexionEstablecida(ComPort);

                using (var serialPort = new SerialPort(ComPort, ComSpeed, ComParity, ComDataBits, ComStopBits))
                {
                    bool ok = true;
                    string command = null;

                    try
                    {
                        serialPort.Open();
                    }
                    catch (System.IO.IOException)
                    {
                        Console.WriteLine(value: $"{consola.Prompt}Puerto {ComPort} NO DISPONIBLE");
                    }

                    new Thread(() => ok = RespuestaDsipositivoCOM(serialPort, consola)).Start();

                    if (!ok)
                    {
                        Console.WriteLine(value: $"{consola.Prompt}Cerrando consola ...");
                    }

                    while (true)
                    {
                        command = Console.ReadLine();

                        try
                        {
                            // Condición de salida del terminal
                            if ((command == "quit") || (command == "QUIT"))
                            {
                                break;
                            }
                            // Envía el comando introducido en el 
                            // terminal vía puerto serie.
                            serialPort.WriteLine(command);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(value: $"{consola.Prompt} {e}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(value: $"{consola.Prompt}Conexión: NO ESTABLECIDA");
            }
        }

        /// <summary>
        ///   Muestra por consola la respuesta del dispositivo
        ///   conectado al puerto COM.
        /// </summary>
        /// <param name="comPort">
        ///   Puerto COM empleado
        /// </param>
        /// <returns>
        ///   true, si no hay problemas en el proceso.
        /// </returns>
        private bool RespuestaDsipositivoCOM(SerialPort comPort, Consola consola)
        {
            bool ok = true;

            try
            {
                // Escribe en el terminal la respuesta
                // recibida vía puerto serie
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
        /// <param name="comPort">
        ///   Puerto COM a verificar
        /// </param>
        /// <returns>
        ///   Conexión disponible.
        /// </returns>
        private bool VerificaPuertoComDisponible(string comPort)
        {
            bool ok = true;

            using (var serialPort = new SerialPort(comPort, ComSpeed, ComParity, ComDataBits, ComStopBits))
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
        /// <returns>
        ///   Número del puerto localizado.
        /// </returns>
        public string BucaPuertoCom()
        {
            string _portName = "COM";
            string _portSearch = "";
            string _puertoLocalizado = null;
            bool _portOk = false;

            for (int port = PortIni; port <= PortFin; port++)
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