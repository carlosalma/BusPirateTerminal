using System;

namespace MicroTerminal
{
    class Consola
    {
        public string Prompt { get; }

        public Consola()
        {
            Prompt = "::> ";
        }

        public void MsgPresentacion()
        {
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Consola de acceso a Bus Pirate \n");
            Console.WriteLine("                                 -- C.AL.MA --");
            Console.WriteLine("----------------------------------------------");
        }

        public void MsgConexionEstablecida(string comPort)
        {
            Console.WriteLine(value: $"{Prompt}Conexión: ESTABLECIDA");
            Console.WriteLine(value: $"{Prompt}Puerto: {comPort}");
            Console.WriteLine(value: $"{Prompt}Para salir de la consola, teclear: quit");
            Console.WriteLine("----------------------------------------------");
        }
    }
}
