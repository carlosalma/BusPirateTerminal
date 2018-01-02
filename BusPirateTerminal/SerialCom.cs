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

using System;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace BusPirateTerminal
{
    internal class SerialCom
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
        
        //
        #region Constructores

   
        /// <summary>
        /// </summary>
        public SerialCom()
        {
        }
        
        /// <summary>
        ///     Constructor que se emplea para seleccionar un puerto
        ///     COM partiendo de su ID.
        ///     Los ID de puerto, se pueden listar mediante el parámetro -l
        /// </summary>
        public SerialCom(int id)
        {
            ComPort = ExtraePuertoComPorID(id - 1);
        }

        // TODO: adapatar los parámetros de entrada del constructor
        /// <summary>
        ///     Auto configuración para BusPirate.
        ///     Emplea estos parámetros para realizar la prueba de
        ///     puerto COM mediante:
        ///     VerificaPuertoComDisponible(string comPort)
        /// </summary>
        /// <param name="patron">
        ///     Patrón a emplear al localizar el dispositivo conectado
        /// </param>
        public SerialCom(string patron)
        {
            ComSpeed = 115200;
            ComParity = Parity.None;
            ComDataBits = 8;
            ComStopBits = StopBits.One;
            ComOk = true;
            
            ComPort = BucaPuertoComMediantePatron(patron);
            if (ComPort == null) ComOk = false;
        }

        /// <summary>
        ///     Permite establecer los parámetros de comunicación.
        /// </summary>
        /// <param name="comPort">
        ///     Número de puerto COM.
        /// </param>
        /// <param name="comSpeed">
        ///     Velocidad de comunicación.
        /// </param>
        /// <param name="comParity">
        ///     Bit de paridad
        /// </param>
        /// <param name="comDataBits">
        ///     Número de bits de datos
        /// </param>
        /// <param name="comStopBits">
        ///     Número de bits de parada
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
        ///     Establece el proceso de comunicación
        /// </summary>
        public void Conectar()
        {
            var consola = new Consola();

            if (ComOk)
            {
                consola.MsgConexionEstablecida(ComPort);

                using (var serialPort = new SerialPort(ComPort, ComSpeed, ComParity, ComDataBits, ComStopBits))
                {
                    var ok = true;

                    try
                    {
                        serialPort.Open();
                    }
                    catch (IOException)
                    {
                        Console.WriteLine($"{consola.Prompt}Puerto {ComPort} NO DISPONIBLE");
                    }

                    new Thread(() =>
                    {
                        if (serialPort != null) ok = RespuestaDispositivoCom(serialPort, consola);
                    }).Start();

                    if (!ok) Console.WriteLine($"{consola.Prompt}Cerrando consola ...");

                    while (true)
                    {
                        var command = Console.ReadLine();

                        try
                        {
                            // Condición de salida del terminal
                            if (command == "quit" || command == "QUIT")
                                break;

                            // Envía el comando introducido en el 
                            // terminal vía puerto serie.
                            serialPort.WriteLine(command);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"{consola.Prompt} {e}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"{consola.Prompt}Conexión: NO ESTABLECIDA");
            }
        }

        /// <summary>
        ///     Respuesta por consola del dispositivo conectado al puerto COM.
        /// </summary>
        /// <param name="comPort">
        ///     Puerto COM empleado
        /// </param>
        /// <param name="consola">
        /// </param>
        /// <returns>
        ///     true, si no hay problemas en el proceso.
        /// </returns>
        private bool RespuestaDispositivoCom(SerialPort comPort, Consola consola)
        {
            bool ok;

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
        ///     Busca un dispositivo conectado a un puerto COM.
        ///     Para localizar un dispositivo concreto, emplea un patrón con formato
        ///     string que lo identifica.
        /// </summary>
        /// <param name="patron">
        ///     Patrón de busqueda, que identifique al dispositivo concreto.
        ///     Por ejemplo, BusPirate en OSX se identifica como:
        ///     "/dev/tty.usbserial-A505M5LI", por lo que se puede emplear por ejemplo
        ///     como patrón "usbserial".
        /// </param>
        /// <returns>
        ///     Puerto localizado, de lo contrario Null.
        /// </returns>
        public string BucaPuertoComMediantePatron(string patron)
        {
            foreach (var puerto in ListarPuertosCom())
            {
                // Hay patrón
                if (patron != null)
                                 
                    // Localizar el patrón en el puerto 
                    if (System.Text.RegularExpressions.Regex.IsMatch(puerto, patron))
           
                        // Verificar si el puerto con el patrón responde
                        if (VerificaPuertoComDisponible(puerto)) return puerto;
               
            }
            return null;
        }

        /// <summary>
        ///     Verifica si la conexión con el puerto COM indicado está disponible.
        /// </summary>
        /// <param name="comPort">
        ///     Puerto COM a verificar
        /// </param>
        /// <returns>
        ///     Conexión disponible.
        /// </returns>
        private bool VerificaPuertoComDisponible(string comPort)
        {
            var ok = true;

            using (var serialPort = new SerialPort(comPort, ComSpeed, ComParity, ComDataBits, ComStopBits))
            {
                try
                {
                    if (!serialPort.IsOpen) serialPort.Open();
                }
                catch (IOException)
                {
                    ok = false;
                }
                catch (ObjectDisposedException)
                {
                    ok = false;
                }
                finally
                {
                    if (serialPort.IsOpen) serialPort.Close();
                }

                return ok;
            }
        }

        /// <summary>
        ///     Matriz con la lista de los puertos COM disponibles.
        /// </summary>
        /// <returns>
        ///     Listado de puertos COM
        /// </returns>
        public string[] ListarPuertosCom()
        {
            string[] puertos = SerialPort.GetPortNames();

            return puertos;
        }

        /// <summary>
        ///     Extrae el puerto COM correspondiente al ID introducido
        /// </summary>
        /// <param name="id">
        ///     Posición en la matriz de puertos COM
        /// </param>
        /// <returns>
        ///     Puerto COM seleccionado
        /// </returns>
        public string ExtraePuertoComPorID(int id)
        {
            string[] puertos = SerialPort.GetPortNames();
            string puerto = null;
            
            if ((id >= 0) && (id <= puertos.Length))
            {
                puerto = puertos[id];
            }

            return puerto;
        }
    }
}