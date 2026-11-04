using System;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace Lab3;

internal static class FileTasks
{
    public static void RunBinaryFilter()
    {
        string directory = PrepareDirectory();
        string source = Path.Combine(directory, "numbers.bin");
        string destination = Path.Combine(directory,
            "filtered.bin");
        int count = Input.ReadInt("Количество чисел (1–10000): ",
            1, 10000);
        int minimum = Input.ReadInt(
            "Минимальное случайное число: ");
        int maximum = Input.ReadInt(
            "Максимальное случайное число: ", minimum);
        int firstDivisor = Input.ReadNonzeroInt(
            "Введите m (не 0): ");
        int secondDivisor = Input.ReadNonzeroInt(
            "Введите n (не 0): ");
        FillBinaryFile(source, count, minimum, maximum);
        FilterBinaryFile(source, destination, firstDivisor,
            secondDivisor);
        Console.WriteLine("Исходные числа:");
        PrintBinaryFile(source);
        Console.WriteLine(
            "Числа, делящиеся на m и не делящиеся на n:");
        PrintBinaryFile(destination);
        Console.WriteLine($"Результат: {destination}");
    }

    public static void FillBinaryFile(string path, int count,
        int minimum, int maximum)
    {
        using var writer = new BinaryWriter(File.Create(path));

        for (var index = 0; index < count; index++)
        {
            var value = (int)Random.Shared.NextInt64(
                minimum, (long)maximum + 1);
            writer.Write(value);
        }
    }

    public static void FilterBinaryFile(string source,
        string destination, int firstDivisor, int secondDivisor)
    {
        using var reader = new BinaryReader(File.OpenRead(
            source));
        using var writer = new BinaryWriter(File.Create(
            destination));

        while (
            reader.BaseStream.Position <
            reader.BaseStream.Length)
        {
            int value = reader.ReadInt32();

            if ((long)value % firstDivisor == 0 &&
                (long)value % secondDivisor != 0)
            {
                writer.Write(value);
            }
        }
    }

    private static void PrintBinaryFile(string path)
    {
        using var reader = new BinaryReader(File.OpenRead(path));
        var hasValues = false;

        while (
            reader.BaseStream.Position <
            reader.BaseStream.Length)
        {
            Console.Write($"{reader.ReadInt32()} ");
            hasValues = true;
        }

        if (!hasValues)
        {
            Console.Write("Нет подходящих чисел.");
        }

        Console.WriteLine();
    }

    public static void RunBaggageSearch()
    {
        string path = Path.Combine(PrepareDirectory(),
            "baggage.bin");
        FillBaggageFile(path);
        decimal limit = Input.ReadPositiveDecimal(
            "Введите m, кг: ");
        var serializer = new XmlSerializer(typeof(Passenger[]));
        using var reader = new BinaryReader(File.OpenRead(path));
        using var textReader = new StringReader(
            reader.ReadString());
        var passengers = (Passenger[])(
            serializer.Deserialize(textReader)
            ?? throw new InvalidDataException(
                "В файле нет сведений о пассажирах."));
        var found = false;

        for (var index = 0; index < passengers.Length; index++)
        {
            BaggageItem[] items = passengers[index].Items;
            Console.WriteLine(
                $"Пассажир {index + 1}: {items.Length} мест");

            foreach (BaggageItem item in items)
            {
                Console.WriteLine(
                    $"  {item.Name}: {item.Mass} кг");
            }

            if (items.Length == 1 && items[0].Mass < limit)
            {
                Console.WriteLine(
                    $"Подходит пассажир {index + 1}.");
                found = true;
            }
        }

        Console.WriteLine(
            found
                ? "Подходящий пассажир имеется."
                : "Подходящего пассажира нет.");
        Console.WriteLine($"Файл: {path}");
    }

    public static void FillBaggageFile(string path)
    {
        int count = Input.ReadInt(
            "Количество пассажиров (1–100): ", 1, 100);
        var passengers = new Passenger[count];

        for (var index = 0; index < count; index++)
        {
            int itemCount = Input.ReadInt(
                $"Багаж пассажира {index + 1} (0–100): ", 0,
                100);
            var items = new BaggageItem[itemCount];

            for (var itemIndex = 0; itemIndex < itemCount;
                itemIndex++)
            {
                string name = Input.ReadText(
                    $"Багаж {itemIndex + 1}, название: ");
                decimal mass = Input.ReadPositiveDecimal(
                    "Масса, кг: ");
                items[itemIndex] = new BaggageItem(name, mass);
            }

            passengers[index] = new Passenger(items);
        }

        var serializer = new XmlSerializer(typeof(Passenger[]));
        using var textWriter = new StringWriter(
            CultureInfo.InvariantCulture);
        serializer.Serialize(textWriter, passengers);
        using var writer = new BinaryWriter(File.Create(path));
        writer.Write(textWriter.ToString());
    }

    public static void RunDifference()
    {
        string path = Path.Combine(PrepareDirectory(),
            "one-per-line.txt");
        ReadRandomFileSettings(out int count, out int minimum,
            out int maximum);
        FillTextFile(path, count, 1, minimum, maximum);
        Console.WriteLine("Исходный файл:");
        Console.WriteLine(File.ReadAllText(path));
        long difference = FindDifference(path);
        Console.WriteLine(
            $"Максимум минус минимум: {difference}");
        Console.WriteLine($"Файл: {path}");
    }

    public static void RunMinimum()
    {
        string path = Path.Combine(PrepareDirectory(),
            "several-per-line.txt");
        ReadRandomFileSettings(out int count, out int minimum,
            out int maximum);
        int perLine = Input.ReadInt(
            "Количество чисел в строке (2–100): ", 2, 100);
        FillTextFile(path, count, perLine, minimum, maximum);
        Console.WriteLine("Исходный файл:");
        Console.WriteLine(File.ReadAllText(path));
        Console.WriteLine(
            $"Минимальный элемент: {FindMinimum(path)}");
        Console.WriteLine($"Файл: {path}");
    }

    private static void ReadRandomFileSettings(out int count,
        out int minimum, out int maximum)
    {
        count = Input.ReadInt("Количество чисел (1–10000): ", 1,
            10000);
        minimum = Input.ReadInt("Минимальное случайное число: ");
        maximum = Input.ReadInt("Максимальное случайное число: ",
            minimum);
    }

    public static void FillTextFile(string path, int count,
        int perLine, int minimum, int maximum)
    {
        using var writer = new StreamWriter(path);

        for (var index = 0; index < count; index++)
        {
            var value = (int)Random.Shared.NextInt64(
                minimum, (long)maximum + 1);
            writer.Write(value);

            if ((index + 1) % perLine == 0 || index == count - 1)
            {
                writer.WriteLine();
            }
            else
            {
                writer.Write(' ');
            }
        }
    }

    public static long FindDifference(string path)
    {
        using var reader = new StreamReader(path);
        string? line = reader.ReadLine();

        if (line == null)
        {
            throw new InvalidDataException(
                "Файл не содержит чисел.");
        }

        int minimum = int.Parse(line,
            CultureInfo.InvariantCulture);
        int maximum = minimum;

        while ((line = reader.ReadLine()) != null)
        {
            int value = int.Parse(line,
                CultureInfo.InvariantCulture);

            if (value < minimum)
            {
                minimum = value;
            }

            if (value > maximum)
            {
                maximum = value;
            }
        }

        return (long)maximum - minimum;
    }

    public static int FindMinimum(string path)
    {
        using var reader = new StreamReader(path);
        int minimum = int.MaxValue;
        var found = false;
        string? line;

        while ((line = reader.ReadLine()) != null)
        {
            string[] numbers = line.Split((char[]?)null,
                StringSplitOptions.RemoveEmptyEntries);

            foreach (string number in numbers)
            {
                int value = int.Parse(number,
                    CultureInfo.InvariantCulture);

                if (value < minimum)
                {
                    minimum = value;
                }

                found = true;
            }
        }

        if (!found)
        {
            throw new InvalidDataException(
                "Файл не содержит чисел.");
        }

        return minimum;
    }

    public static void RunLineFilter()
    {
        string directory = PrepareDirectory();
        string source = Path.Combine(directory,
            "source-lines.txt");
        string destination = Path.Combine(directory,
            "filtered-lines.txt");
        int count = Input.ReadInt("Количество строк (1–1000): ",
            1, 1000);

        using (var writer = new StreamWriter(source))
        {
            for (var index = 0; index < count; index++)
            {
                writer.WriteLine(Input.ReadLine(
                    $"Строка {index + 1}: "));
            }
        }

        string symbol = Input.ReadLine(
            "Один символ (регистр учитывается): ");

        while (symbol.Length != 1)
        {
            Console.WriteLine("Нужно ввести ровно один символ.");
            symbol = Input.ReadLine("Введите символ: ");
        }

        int copied = FilterLines(source, destination, symbol[0]);
        Console.WriteLine($"Скопировано строк: {copied}");
        Console.WriteLine("Содержимое нового файла:");
        Console.WriteLine(File.ReadAllText(destination));
        Console.WriteLine($"Результат: {destination}");
    }

    public static int FilterLines(string source,
        string destination, char symbol)
    {
        using var reader = new StreamReader(source);
        using var writer = new StreamWriter(destination);
        var copied = 0;
        string? line;

        while ((line = reader.ReadLine()) != null)
        {
            if (line.Length > 0 && line[0] == symbol)
            {
                writer.WriteLine(line);
                copied++;
            }
        }

        return copied;
    }

    private static string PrepareDirectory()
    {
        string path = Path.Combine(Environment.CurrentDirectory,
            "data");
        Directory.CreateDirectory(path);
        return path;
    }
}
