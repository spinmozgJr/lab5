using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DataReader
{
    public class DataReader
    {
        private List<string> GetFilesName(string pathOfInitFile)
        {
            List<string> files = new List<string>();
            using (StreamReader reader = new StreamReader(pathOfInitFile))
            {
                string text = reader.ReadToEnd();
                string[] lines = text.Split(new char[] { ' ', ':', '\n', '\r', ',' });
                Regex regex = new Regex(@"G\d*.dat");
                foreach (var word in lines)
                {
                    Match match = regex.Match(word);
                    if (match.Success)
                    {
                        files.Add(word);
                    }
                }
            }
            return files;
        }

        private double[][] ReadDataFromFile(string pathOfDataFile)
        {
            double[][] data = new double[3][];
            using (StreamReader reader = new StreamReader(pathOfDataFile))
            {
                string text = reader.ReadLine();
                text = reader.ReadLine();
                string[] lines = text.Split(new char[] { ' ', '\t', '\r', ';'});

                int x, y;
                if (false == int.TryParse(lines[0], out x))
                    Console.WriteLine("Ошибка при считывании количества точек для х для аргументов функции");
                if (false == int.TryParse(lines[1], out y))
                    Console.WriteLine("Ошибка при считывании количества точек для y для аргументов функции");


                double[] valuesX = new double[x];
                double[] valuesY = new double[y];
                double[] values = new double[x * y];

                text = reader.ReadToEnd();
                lines = text.Split(new char[] { '\t'});
                
                int cntX = 0;
                int cntY = 0;
                int cntValues = 0;
                
                for (int i = 1; i < lines.Length; i++)
                {
                    lines[i] = lines[i].TrimStart(new char[] { '\r', '\n'});
                    if (lines[i] == "")
                    {
                        continue;
                    }
                        
                    //get y values
                    if (cntY < y)
                    {
                        if (double.TryParse(lines[i], out valuesY[cntY]))
                        {
                            cntY++;
                        }
                        else
                        {
                            valuesY[cntY] = double.NaN;
                            cntY++;
                        }

                        continue;
                    }
                    
                    //get x values
                    if (i % (y + 1) == 0 && cntX < x)
                    {
                        if (double.TryParse(lines[i], out valuesX[cntX]))
                        {
                            cntX++;
                        }
                        else
                        {
                            valuesX[cntX] = double.NaN;
                            cntX++;
                        }
                        continue;
                    }

                    //get function values
                    if (double.TryParse(lines[i], out values[cntValues])) 
                    {
                        cntValues++;
                    }
                    else
                    {
                        values[cntValues] = double.NaN;
                        cntValues++;
                    }   
                }
                data[0] = valuesX;
                data[1] = valuesY;
                data[2] = values;

                if (cntX != x || cntY != y || cntValues != x * y)
                {
                    Console.WriteLine($"В файле {pathOfDataFile} недостаточно данных для считывания");
                }
                    
            }

            return data;
        }

        public List<double[][]> GetAllData(string pathOfInitFile)
        {
            List<string> files = GetFilesName(pathOfInitFile);
            List<double[][]> data = new List<double[][]>();
            foreach (var file in files)
            {
                data.Add(ReadDataFromFile(file));
            }
            
            return data;
        }
    }
}
