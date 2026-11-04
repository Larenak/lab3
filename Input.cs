using System;
using System.IO;

namespace Lab3;

public static class Input
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
        string text;

        while (true)
        {
            text = ReadLine(prompt).Trim();

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
        string text;
        int value;

        while (true)
        {
            text = ReadLine(prompt);

            if (int.TryParse(text, out value) && value >=
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
        int value;

        while (true)
        {
            value = ReadInt(prompt);

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
        string text;
        decimal value;

        while (true)
        {
            text = ReadLine(prompt);

            if (decimal.TryParse(text, out value) &&
                value > 0)
            {
                return value;
            }

            Console.WriteLine(
                "Введите число больше 0.");
        }
    }
}
