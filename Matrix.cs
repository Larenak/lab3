using System;
using System.Text;

namespace Lab3;

internal sealed class Matrix
{
    private readonly long[,] _values;

    public int Rows
    {
        get
        {
            return _values.GetLength(0);
        }
    }

    public int Columns
    {
        get
        {
            return _values.GetLength(1);
        }
    }

    public Matrix(int rows, int columns)
        : this(new long[rows, columns])
    {
        for (var column = 0; column < columns; column++)
        {
            for (var row = 0; row < rows; row++)
            {
                _values[row, column] = Input.ReadInt(
                    $"Элемент [{row + 1}, {column + 1}]: ");
            }
        }
    }

    public Matrix(int size, Random random)
        : this(new long[size, size])
    {
        for (var row = 0; row < size; row++)
        {
            for (var column = 0; column < size; column++)
            {
                if (row + column < size - 1)
                {
                    _values[row, column] = random.Next(-12,
                        4566);
                }
                else
                {
                    _values[row, column] = random.Next(-1024,
                        1025);
                }
            }
        }
    }

    public Matrix(int size)
        : this(new long[size, size])
    {
        long value = 1;

        for (var diagonal = 2 * size - 2; diagonal >= size - 1;
            diagonal--)
        {
            for (var row = size - 1; row >= 0; row--)
            {
                int column = diagonal - row;

                if (column < size)
                {
                    _values[row, column] = value;
                    value++;
                }
            }
        }
    }

    private Matrix(long[,] values)
    {
        _values = values;
    }

    public Matrix FindMaximumSubmatrix(out long maximumSum,
        out int bestRow, out int bestColumn)
    {
        if (Rows < 3 || Columns < 3)
        {
            throw new InvalidOperationException(
                "Нужен массив не меньше 3 × 3.");
        }

        maximumSum = long.MinValue;
        bestRow = 0;
        bestColumn = 0;

        for (var row = 0; row <= Rows - 3; row++)
        {
            for (var column = 0; column <= Columns - 3; column++)
            {
                long sum = 0;

                for (var offsetRow = 0; offsetRow < 3;
                    offsetRow++)
                {
                    for (var offsetColumn = 0; offsetColumn < 3;
                        offsetColumn++)
                    {
                        sum += _values[row + offsetRow, column +
                            offsetColumn];
                    }
                }

                if (sum > maximumSum)
                {
                    maximumSum = sum;
                    bestRow = row;
                    bestColumn = column;
                }
            }
        }

        var result = new Matrix(new long[3, 3]);

        for (var row = 0; row < 3; row++)
        {
            for (var column = 0; column < 3; column++)
            {
                result._values[row, column] = _values[bestRow +
                    row, bestColumn + column];
            }
        }

        return result;
    }

    public static Matrix operator ~(Matrix matrix)
    {
        var result = new Matrix(new long[matrix.Columns,
            matrix.Rows]);

        for (var row = 0; row < matrix.Rows; row++)
        {
            for (var column = 0; column < matrix.Columns;
                column++)
            {
                result._values[column, row] = matrix._values[row,
                    column];
            }
        }

        return result;
    }

    public static Matrix operator +(Matrix left, Matrix right)
    {
        if (left.Rows != right.Rows || left.Columns !=
            right.Columns)
        {
            throw new ArgumentException(
                "Размеры матриц должны совпадать.");
        }

        var result = new Matrix(new long[left.Rows,
            left.Columns]);

        for (var row = 0; row < left.Rows; row++)
        {
            for (var column = 0; column < left.Columns; column++)
            {
                result._values[row, column] = left._values[row,
                    column] + right._values[row, column];
            }
        }

        return result;
    }

    public static Matrix operator *(int multiplier,
        Matrix matrix)
    {
        var result = new Matrix(new long[matrix.Rows,
            matrix.Columns]);

        for (var row = 0; row < matrix.Rows; row++)
        {
            for (var column = 0; column < matrix.Columns;
                column++)
            {
                result._values[row, column] = multiplier *
                    matrix._values[row, column];
            }
        }

        return result;
    }

    public override string ToString()
    {
        var result = new StringBuilder();

        for (var row = 0; row < Rows; row++)
        {
            for (var column = 0; column < Columns; column++)
            {
                result.Append($"{_values[row, column],14}");
            }

            result.AppendLine();
        }

        return result.ToString();
    }
}
