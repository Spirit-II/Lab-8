using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_8
{
        internal static class InputHelper
        {
            public static int ReadInt(string message)
            {
                while (true)
                {
                    Console.Write(message);

                    if (int.TryParse(Console.ReadLine(), out int value))
                    {
                        return value;
                    }

                    Console.WriteLine("Некорректное число.");
                }
            }

            public static decimal ReadDecimal(string message)
            {
                while (true)
                {
                    Console.Write(message);

                    if (decimal.TryParse(
                            Console.ReadLine(),
                            out decimal value)
                        && value >= 0)
                    {
                        return value;
                    }

                    Console.WriteLine(
                        "Введите неотрицательное число.");
                }
            }

            public static string ReadString(string message)
            {
                while (true)
                {
                    Console.Write(message);

                    string value = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        return value;
                    }

                    Console.WriteLine("Строка не может быть пустой.");
                }
            }

            public static DateTime ReadDate(string message)
            {
                while (true)
                {
                    Console.Write(message);

                    if (DateTime.TryParse(
                            Console.ReadLine(),
                            out DateTime value))
                    {
                        return value;
                    }

                    Console.WriteLine("Некорректная дата.");
                }
            }
        }
}
