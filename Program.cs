using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lexico_1;

namespace Prueba
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (Lexico T = new Lexico("Suma.cpp"))
                {
                    //T.setContenido("Hola");
                    //T.setClasificacion(Token.Tipos.Identificador);

                    //Console.WriteLine(T.getContenido() + " = " + T.getClasificacion());

                    //T.setContenido("1345");
                    //T.setClasificacion(Token.Tipos.Numero);

                    //Console.WriteLine(T.getContenido() + " = " + T.getClasificacion());
                    T.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

        }
    }
}