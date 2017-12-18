using System;
using System.IO.Ports;
using System.Threading;
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
            // Gestión de parámetros
            CommandLineParser.CommandLineParser parser = new CommandLineParser.CommandLineParser();
            Parametros param = new Parametros();
            
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
            // FIN: Gestión de parámetros
            //
            
            SerialCom comOk = new SerialCom();
            
            if (comOk.ComOk)
            {
                consola.MsgConexionEstablecida(comOk.ComPort);

                using (var serialPort = new SerialPort(comOk.ComPort, comOk.ComSpeed, comOk.ComParity, comOk.ComBits, comOk.ComStopBits))
                {
                    try
                    {
                        serialPort.Open();
                    }
                    catch (System.IO.IOException)
                    {
                        Console.WriteLine(value: $"{consola.Prompt}Puerto {comOk.ComPort} NO DISPONIBLE");
                        return;
                    }

                    bool ok = true;
                    new Thread(() => ok = EscribirLineasDesde(serialPort)).Start();

                    if (!ok)
                    {
                        Console.WriteLine(value: $"{consola.Prompt}Cerrando consola ...");
                    }

                    while (true)
                    {
                        Console.Write(value: consola.Prompt);
                        var command = Console.ReadLine();

                        try
                        {
                            if ((command == "quit") || (command == "QUIT"))
                            {
                                break;
                            }
                            serialPort.WriteLine(command);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(value: $"{consola.Prompt} {e}");
                            return;
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
        /// Muestra por consola la respuesta del dispositivo
        /// conectado al puerto COM.
        /// </summary>
        /// <param name="comPort">Puerto COM empleado</param>
        /// <returns></returns>
        static bool EscribirLineasDesde(SerialPort comPort)
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
        /// Acciones a realizar con los parámetros.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="param">parámetros</param>
        static void SeleccionParametros(CommandLineParser.CommandLineParser parser, Parametros param)
        {
            // Acceso a los valores de los parámetros
            if (param.Modo == "auto")
            {
                Console.WriteLine("-------------------->>>> Has seleccionado el modo auto");
            }
            else if (param.Modo == "manual")
            {
                Console.WriteLine("-------------------->>>>> Has seleccionado el modo manual");
            }
            if (!param.Help)
            {
                param.MostrarAyudaParametros(parser, param);
            }
        }
    }
}

