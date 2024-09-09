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
                using (Lexico l = new Lexico())
                {
                    while (!l.finArchivo())
                    {
                        l.nexToken();
                    }

                    l.EscribeCantidadLineas();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.WriteLine(e.StackTrace);
            }

        }
    }
}