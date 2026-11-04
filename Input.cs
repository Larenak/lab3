using System;
using System.Globalization;
using System.IO;

namespace Lab3;

internal static class Input
{
    public static string ReadLine(string prompt)
    {
        Console.Write(prompt);
        string? text = Console.ReadLine();

        if (text == null)
        {
            throw new EndOfStreamException();
        }

        return text;
    }

    public static string ReadText(string prompt)
    {
        while (true)
        {
            string text = ReadLine(prompt).Trim();

            if (text.Length > 0)
            {
                return text;
            }

            Console.WriteLine("Введите непустую строку.");
        }
    }

    public static int ReadInt(string prompt, int minimum =
        int.MinValue, int maximum = int.MaxValue)
    {
        while (true)
        {
            string text = ReadLine(prompt);

            if (int.TryParse(text, out int value) && value >=
                minimum && value <= maximum)
            {
                return value;
            }

            Console.WriteLine(
                $"Введите целое: {minimum} … {maximum}.");
        }
    }

    public static int ReadNonzeroInt(string prompt)
    {
        while (true)
        {
            int value = ReadInt(prompt);

            if (value != 0)
            {
                return value;
            }

            Console.WriteLine(
                "Делитель не должен быть равен нулю.");
        }
    }

    public static decimal ReadPositiveDecimal(string prompt)
    {
        while (true)
        {
            string text = ReadLine(prompt).Replace(',', '.');

            if (decimal.TryParse(text,
                NumberStyles.AllowDecimalPoint |
                NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture, out decimal value)
                    && value > 0)
            {
                return value;
            }

            Console.WriteLine(
                "Введите число больше 0 (точка или запятая).");
        }
    }
}
