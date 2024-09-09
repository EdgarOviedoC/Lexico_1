using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

/*
    1) Sobrecargar el constructor lexico para que reciba como argumento el nombre del archivo a  (Listo)
    2) Tener un contador de lineas 
    3) Agregar operador relacional  
        ==, >,>=, <, <=, <>, !=
    4) Agregar Operador logico
        ||, &&, !
*/
namespace Lexico_1
{
    public class Lexico : Token, IDisposable
    {
        StreamReader archivo;
        StreamWriter log;
        StreamWriter asm;
        int line = 0;
        StreamWriter error;

        //Constructor de la clase lexico
        public Lexico()
        {
            error = new StreamWriter("Errores.error");
            error.AutoFlush = true;

            log = new StreamWriter("prueba.log");
            asm = new StreamWriter("prueba.asm");
            log.AutoFlush = true;
            asm.AutoFlush = true;

            if (File.Exists("prueba.cpp"))
            {
                archivo = new StreamReader("prueba.cpp");
            }
            else
            {
                throw new Error("El archivo pueba.cpp no existe", error);
            }
        }

        //Constructor sobrecargado
        public Lexico(string nombreArchivo)
        {
            error = new StreamWriter("Errores.error");
            error.AutoFlush = true;

            if (Path.GetExtension(nombreArchivo) == ".cpp" && File.Exists(Path.ChangeExtension(nombreArchivo, ".cpp")))
            {
                archivo = new StreamReader(nombreArchivo);
                log = new StreamWriter(Path.ChangeExtension(nombreArchivo, ".log"));
                asm = new StreamWriter(Path.ChangeExtension(nombreArchivo, ".asm"));
                log.AutoFlush = true;
                asm.AutoFlush = true;
            }
            else
            {
                throw new Error("El archivo " + nombreArchivo + " No existe  tiene extension invalida", error);
            }
        }

        //Destructor de la clase lexico
        public void Dispose()
        {
            archivo.Close();
            log.Close();
            asm.Close();
        }
        //
        public void nexToken()
        {
            char c;
            string Buffer = "";

            while (char.IsWhiteSpace(c = (char)archivo.Read()))
            {
                ContadorLineas(c);
            }

            Buffer += c;

            if (char.IsLetter(c))
            {
                setClasificacion(Tipos.Caracter);

                while (char.IsLetterOrDigit(c = (char)archivo.Peek()))
                {
                    setClasificacion(Tipos.Identificador);
                    Buffer += c;
                    archivo.Read();
                }

                ContadorLineas(c);
            }
            else if (char.IsDigit(c))
            {
                setClasificacion(Tipos.Numero);

                while (char.IsDigit(c = (char)archivo.Peek()))
                {
                    Buffer += c;
                    archivo.Read();
                }

                ContadorLineas(c);
            }
            else if (c == ';')
            {
                setClasificacion(Tipos.FinSentencia);

                ContadorLineas(c);
            }
            else if (c == '{')
            {
                setClasificacion(Tipos.InicioBloque);

                ContadorLineas(c);
            }
            else if (c == '}')
            {
                setClasificacion(Tipos.FinBloque);

                ContadorLineas(c);
            }
            else if (c == '?')
            {
                setClasificacion(Tipos.OperadorTernario);

                ContadorLineas(c);
            }
            else if (c == '$')
            {
                setClasificacion(Tipos.Caracter);

                if (Char.IsDigit(c = (char)archivo.Peek()))
                {
                    setClasificacion(Tipos.Moneda);

                    while (char.IsDigit(c = (char)archivo.Peek()))
                    {
                        Buffer += c;
                        archivo.Read();
                    }
                }

                ContadorLineas(c);
            }
            else if (c == '=')
            {
                setClasificacion(Tipos.Asignacion);

                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    Buffer += c;
                    archivo.Read();
                }

                ContadorLineas(c);
            }
            else if (c == '|')
            {
                setClasificacion(Tipos.Caracter);

                if ((c = (char)archivo.Peek()) == '|')
                {
                    setClasificacion(Tipos.OperadorLogico);
                    Buffer += c;
                    archivo.Read();
                }

                ContadorLineas(c);
            }
            else if (c == '&')
            {
                setClasificacion(Tipos.Caracter);

                if ((c = (char)archivo.Peek()) == '&')
                {
                    setClasificacion(Tipos.OperadorLogico);
                    Buffer += c;
                    archivo.Read();
                }

                ContadorLineas(c);
            }
            else if (c == '!')
            {
                setClasificacion(Tipos.OperadorLogico);

                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    Buffer += c;
                    archivo.Read();
                }

                ContadorLineas(c);
            }
            else if (c == '<')
            {
                setClasificacion(Tipos.OperadorRelacional);

                if ((c = (char)archivo.Peek()) == '=' || c == '>')
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    Buffer += c;
                    archivo.Read();
                }

                ContadorLineas(c);
            }
            else if (c == '>')
            {
                setClasificacion(Tipos.OperadorRelacional);

                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.OperadorRelacional);
                    Buffer += c;
                    archivo.Read();
                }

                ContadorLineas(c);
            }
            else if (c == '+')
            {
                setClasificacion(Tipos.OperadorTermino);

                if ((c = (char)archivo.Peek()) == '+' || c == '=')
                {
                    setClasificacion(Tipos.IncrementoTermino);
                    Buffer += c;
                    archivo.Read();
                }

                ContadorLineas(c);
            }
            else if (c == '-')
            {
                setClasificacion(Tipos.OperadorTermino);

                if ((c = (char)archivo.Peek()) == '-' || c == '=')
                {
                    setClasificacion(Tipos.IncrementoTermino);
                    Buffer += c;
                    archivo.Read();
                }
                else if (c == '>')
                {
                    setClasificacion(Tipos.Puntero);
                    Buffer += c;
                    archivo.Read();
                }

                ContadorLineas(c);
            }
            else if (c == '*' || c == '/' || c == '%')
            {
                setClasificacion(Tipos.OperadorFactor);

                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.IncrementoFactor);
                    Buffer += c;
                    archivo.Read();
                }

                ContadorLineas(c);
            }
            else if (char.IsWhiteSpace(c))
            {
                ContadorLineas(c);
            }
            else
            {
                setClasificacion(Tipos.Caracter);

                ContadorLineas(c);
            }

            if (!finArchivo())
            {
                setContenido(Buffer);
                log.WriteLine("{0}  °°°°  {1}", getContenido(), getClasificacion());
                Console.WriteLine("Escribiendo en log : " + getContenido());
            }
        }

        public bool finArchivo()
        {
            return archivo.EndOfStream;
        }

        public void ContadorLineas(int c)
        {
            if (c == 10)
            {
                line++;
            }
        }

        public void EscribeCantidadLineas()
        {
            log.WriteLine("Total de lineas {0}", line);
            Console.WriteLine("Total de lineas {0}", line);
        }
    }
}