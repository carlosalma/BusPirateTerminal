using System.IO.Ports;

namespace MicroTerminal
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
        /// Auto configuración para BusPirate
        /// Dispone de todos los parámetros por defecto
        /// y localiza el puerto COM empleado.
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
        /// Configuración manual.
        /// Hay que introducir todos los parámetros
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

        /// <summary>
        /// Verifica si la conexión con el puerto COM está disponible.
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
        /// Busca el puerto COM disponible dentro de un rango dado.
        /// </summary>
        /// <param name="portIni">Número de puerto inicial</param>
        /// <param name="portFin">Número de puerto final</param>
        /// <returns>Numero del puerto localizado</returns>
        private string BucaPuertoCom(int portIni, int portFin)
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