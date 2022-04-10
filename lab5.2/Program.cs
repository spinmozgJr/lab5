using System;
using System.IO;

namespace lab5._2
{
    class Program
    {
        private enum Errors : int
        {
            noError = 0,
            rootOfNegativeNumber = 1,
            overflow = 2,
            error = 3
        }
        static Errors f(double x, double y, ref double result)
        {
            try
            {
                if (x > double.MaxValue || y > double.MaxValue) 
                    return Errors.overflow;

                double problemPlace = Math.Cos(x + y);
                if (problemPlace < 0)
                    return Errors.rootOfNegativeNumber;

                result = Math.Sqrt(problemPlace);
                return Errors.noError;
            }
            catch (Exception)
            {
                return Errors.error;
            }
           
        }

        static void Main(string[] args)
        {
            //начальная точка, конечная точка, количество шагов
            double[][] x = {
                new double[] { 12, 14, 4 },
                new double[] { 2, 6, 10 },
                new double[] { -10, 10, 10 },
                new double[] { -1, 10, 20 },
            };

            //начальная точка, конечная точка, количество точек
            double[][] y = {
                new double[] { 1, 3, 6 },
                new double[] { 1, 10, 10 },
                new double[] { -10, -5, 6 },
                new double[] { 50, 100, 5 },
            };

            string function = "(cos(x + y))^(1 / 2)";

            using (StreamWriter writer = new StreamWriter("myProgram.log", false))
            {
                writer.WriteLine("\"Лабораторная 5\", вариант 11");
                writer.WriteLine($"{DateTime.Now}");
                writer.WriteLine($"Данная функция: {function}");
                writer.Write("Файлы с вычислениями: ");
                for (int i = 0; i < x.Length; i++)
                {
                    if (i == x.Length - 1)
                    {
                        writer.Write($"G{i + 1:0000}.dat.");
                        break;
                    }
                        
                    writer.Write($"G{i+1:0000}.dat, ");
                }
            }

            using (StreamWriter errorsWriter = new StreamWriter("myErrors.log", false))
            {
                for (int i = 0; i < x.GetLength(0); i++)
                {
                    using (StreamWriter valuesWriter = new StreamWriter($"G{i + 1:0000}.dat", false))
                    {
                        valuesWriter.WriteLine(function);

                        double stepX = (x[i][1] - x[i][0]) / x[i][2];
                        double stepY = (y[i][1] - y[i][0]) / y[i][2];
                        int countOfInterationsX = (int)x[i][2];
                        int countOfInterationsY = (int)y[i][2];
                        double valueX = x[i][0];
                        double valueY = y[i][0];

                        const int spaceForNumbers = 18;
                        valuesWriter.WriteLine(countOfInterationsX + " " + countOfInterationsY);
                        valuesWriter.Write($"x/y{"", spaceForNumbers}\t");
                        for (int k = 0; k < countOfInterationsY; k++) 
                        {
                            if (k == countOfInterationsY - 1)
                                valuesWriter.Write($"{valueY, -spaceForNumbers}\t");
                            else
                                valuesWriter.Write($"{valueY,-spaceForNumbers}\t");
                            valueY += stepY;
                        }
                        valuesWriter.WriteLine();
                        valueY = y[i][0];

                        for (int j = 0; j < countOfInterationsX; j++)
                        {
                            valuesWriter.Write($"{valueX, -spaceForNumbers}\t");
                            for (int k = 0; k < countOfInterationsY; k++)
                            {
                                double result = 0;
                                switch (f(valueX, valueY, ref result))
                                {
                                    case Errors.noError:
                                        if (k == countOfInterationsY - 1)
                                            valuesWriter.Write($"{result, -spaceForNumbers}\t");
                                        else
                                            valuesWriter.Write($"{result,-spaceForNumbers}\t");
                                        break;
                                    case Errors.rootOfNegativeNumber:
                                        errorsWriter.WriteLine($"X:{valueX} Y:{valueY} корень из отрицательного числа");
                                        if (k == countOfInterationsY - 1)
                                            valuesWriter.Write($"{"Null", -spaceForNumbers}\t");
                                        else
                                            valuesWriter.Write($"{"Null", -spaceForNumbers}\t");
                                        break;
                                    case Errors.overflow:
                                        errorsWriter.WriteLine($"X:{valueX} Y:{valueY} переполнение");
                                        if (k == countOfInterationsY - 1)
                                            valuesWriter.Write($"{"Null",-spaceForNumbers}\t");
                                        else
                                            valuesWriter.Write($"{"Null",-spaceForNumbers}\t");
                                        break;
                                    default:
                                        errorsWriter.WriteLine($"X:{valueX} Y:{valueY} ошибка");
                                        if (k == countOfInterationsY - 1)
                                            valuesWriter.Write($"{"Null",-spaceForNumbers}\t");
                                        else
                                            valuesWriter.Write($"{"Null",-spaceForNumbers}\t");
                                        break;
                                }
                                valueY += stepY;
                            }
                            valuesWriter.WriteLine();
                            valueX += stepX;
                        }
                    }
                }
            }

            DataReader.DataReader dataReader = new DataReader.DataReader();
            var files = dataReader.GetAllData("myProgram.log");
            Console.WriteLine(files);
        }
    }
}

