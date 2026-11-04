using System;
using System.IO;
using System.Text;

namespace Lab3;

internal static class Program
{
    private static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        try
        {
            RunMenu();
        }
        catch (EndOfStreamException)
        {
            Console.WriteLine("Ввод завершён.");
        }
    }

    private static void RunMenu()
    {
        while (true)
        {
            Console.WriteLine("\nЛабораторная №3. Вариант 3");
            Console.WriteLine("1 — Заполнение матриц");
            Console.WriteLine(
                "2 — Максимальный подмассив 3 × 3");
            Console.WriteLine("3 — Выражение Aᵀ + 2 * B + Cᵀ");
            Console.WriteLine(
                "4 — Отбор чисел из бинарного файла");
            Console.WriteLine("5 — Багаж пассажиров");
            Console.WriteLine(
                "6 — Разность максимума и минимума");
            Console.WriteLine("7 — Минимум в текстовом файле");
            Console.WriteLine(
                "8 — Строки с заданным первым символом");
            Console.WriteLine("0 — Выход");
            int choice = Input.ReadInt("Выберите задание: ", 0,
                8);

            try
            {
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        RunMatrixCreation();
                        break;
                    case 2:
                        RunSubmatrixSearch();
                        break;
                    case 3:
                        RunMatrixExpression();
                        break;
                    case 4:
                        FileTasks.RunBinaryFilter();
                        break;
                    case 5:
                        FileTasks.RunBaggageSearch();
                        break;
                    case 6:
                        FileTasks.RunDifference();
                        break;
                    case 7:
                        FileTasks.RunMinimum();
                        break;
                    case 8:
                        FileTasks.RunLineFilter();
                        break;
                }
            }
            catch (EndOfStreamException)
            {
                throw;
            }
            catch (IOException exception)
            {
                Console.WriteLine(
                    $"Ошибка файла: {exception.Message}");
            }
            catch (UnauthorizedAccessException exception)
            {
                Console.WriteLine(
                    $"Нет доступа к файлу: {exception.Message}");
            }
        }
    }

    private static void RunMatrixCreation()
    {
        int rows = Input.ReadInt(
            "Число строк первого массива (1–100): ", 1, 100);
        int columns = Input.ReadInt(
            "Столбцов в первом массиве (1–100): ", 1, 100);
        var first = new Matrix(rows, columns);
        Console.WriteLine($"Первый массив:\n{first}");
        int size = Input.ReadInt(
            "Размер квадратных матриц (1–100): ", 1, 100);
        var second = new Matrix(size, Random.Shared);
        var third = new Matrix(size);
        Console.WriteLine($"Второй массив:\n{second}");
        Console.WriteLine($"Третий массив:\n{third}");
    }

    private static void RunSubmatrixSearch()
    {
        int rows = Input.ReadInt("Число строк (3–100): ", 3,
            100);
        int columns = Input.ReadInt("Число столбцов (3–100): ",
            3, 100);
        var matrix = new Matrix(rows, columns);
        Console.WriteLine($"Исходный массив:\n{matrix}");
        Matrix result = matrix.FindMaximumSubmatrix(out long sum,
            out int row, out int column);
        Console.WriteLine(
            $"Начало: [{row + 1}, {column + 1}].\n{result}");
        Console.WriteLine($"Максимальная сумма: {sum}");
    }

    private static void RunMatrixExpression()
    {
        int rows = Input.ReadInt(
            "Число строк результата (1–100): ", 1, 100);
        int columns = Input.ReadInt(
            "Число столбцов результата (1–100): ", 1, 100);
        Console.WriteLine("Введите A (до транспонирования):");
        var first = new Matrix(columns, rows);
        Console.WriteLine("Введите B:");
        var second = new Matrix(rows, columns);
        Console.WriteLine("Введите C (до транспонирования):");
        var third = new Matrix(columns, rows);
        Matrix result = ~first + 2 * second + ~third;
        Console.WriteLine(
            $"A:\n{first}\nB:\n{second}\nC:\n{third}");
        Console.WriteLine($"Aᵀ + 2 * B + Cᵀ:\n{result}");
    }
}
