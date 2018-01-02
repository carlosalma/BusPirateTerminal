using System;

// TODO: buscar info sobre: _serialPort.WriteLine(String.Format("<{0}>: {1}", name, message));

namespace BusPirateTerminal
{
    internal class Consola
    {
        public Consola()
        {
            Prompt = "::> ";
            Version = "0.4";
            ColorTexto = ConsoleColor.Red;
        }

        public string Prompt { get; }
        public string Version { get; }
        public ConsoleColor ColorTexto { get; }

        /// <summary>
        ///     Cabecera
        /// </summary>
        public void MsgPresentacion()
        {
            Console.ForegroundColor = ColorTexto;
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Consola de acceso a Bus Pirate");
            Console.WriteLine("----------------------------------------------");
        }

        /// <summary>
        ///     Cuerpo
        /// </summary>
        /// <param name="comPort">
        /// </param>
        public void MsgConexionEstablecida(string comPort)
        {
            Console.WriteLine($"{Prompt}Conexión: ESTABLECIDA en puerto {comPort}");
            Console.WriteLine($"{Prompt}Para salir de la consola, teclear: quit");
            Console.WriteLine("----------------------------------------------");
        }

        /// <summary>
        ///     Muestra el listado de dispositivos conectados a puertos COM
        /// </summary>
        public void MsgListadoPuertos()
        {
            var conexionSerie = new SerialCom();
            var puertos = conexionSerie.ListarPuertosCom();
            string listado = null;
            int cnt = 0;
            
            
            // RegExp, para dispositivos COM en OSX
            string regExpOsx = @"/dev/tty\.";

            // RegExp, para dispositivos COM en OSX
            string regExpWin = @"COM\d";

            foreach (var puerto in puertos)
            {
                var comEnUsoOsx = System.Text.RegularExpressions.Regex.IsMatch(puerto, regExpOsx);
                var comEnUsoWin = System.Text.RegularExpressions.Regex.IsMatch(puerto, regExpWin);
                    
                if (comEnUsoOsx || comEnUsoWin)
                {
                    cnt++;   
                    listado += "|  " + cnt + " | " + puerto + "\n";
                }
            }
            
            Console.WriteLine($"{Prompt}Listado de puertos \n");
            Console.WriteLine($"|----|--------------------------------------");
            Console.WriteLine($"| ID |   DISPOSITIVO COM");
            Console.WriteLine($"|----|--------------------------------------");
            Console.WriteLine($"{listado}");
        }

        /// <summary>
        ///     Muestra los parámetros que se han empleado para
        ///     establecer la comunicación.
        /// </summary>
        public void MostrarParametros()
        {
            var conexionSerie = new SerialCom();
            var listaParametros = $"Parámetros de la conexión: \n" +
                                  $" - ComPort: {conexionSerie.ComPort} \n" +
                                  $" - ComSpeed: {conexionSerie.ComSpeed} \n" +
                                  $" - ComParity: {conexionSerie.ComParity} \n" +
                                  $" - ComBits: {conexionSerie.ComDataBits} \n" +
                                  $" - ComStopBits{conexionSerie.ComStopBits}";

            Console.WriteLine($"{Prompt}{listaParametros}");
        }
    }
}