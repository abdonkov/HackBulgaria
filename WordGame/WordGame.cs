using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGame
{
    class WordGame
    {
        static char[,] TableInput()
        {
            bool invalidInput = true;
            char[,] table = new char[1, 1];
            while (invalidInput)
            {
                Console.WriteLine("Read table from:\n1. File\n2. Console");
                string inputWay = Console.ReadLine();

                if (inputWay == "1")
                {
                    Console.WriteLine("File must be in format:\na1, a2, a3\nb1, b2, b3\nWhere a1,a2,...,b3 are characters.\nNote: Commas aren't necessary!");
                    Console.WriteLine("Please write filename with the file extention. Example: file.txt");
                    string filename = Console.ReadLine();
                    try
                    {
                        string[] lines = File.ReadAllLines(filename);
                        int columns = 0;
                        for (int i = 0; i < lines.Length; i++)
                        {
                            char[] curLineChars = lines[i].Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(char.Parse).ToArray();

                            if (i == 0)
                            {
                                table = new char[lines.Length, curLineChars.Length];
                                columns = curLineChars.Length;
                            }

                            if (columns != curLineChars.Length) throw new Exception();

                            for (int j = 0; j < columns; j++)
                            {
                                table[i, j] = curLineChars[j];
                            }
                        }
                        Console.WriteLine("File red successfully!");
                        invalidInput = false;
                    }
                    catch
                    {
                        Console.WriteLine("File not found or invalid file format!");
                    }
                }
                else if (inputWay == "2")
                {
                    try
                    {
                        Console.Write("Table rows: ");
                        int rows = int.Parse(Console.ReadLine());
                        Console.Write("Table colomns: ");
                        int cols = int.Parse(Console.ReadLine());

                        table = new char[rows, cols];

                        Console.WriteLine("Input must be in format:\na1, a2, a3\nb1, b2, b3\nWhere a1,a2,...,b3 are characters.\nNote: Commas aren't necessary!");
                        Console.WriteLine("Input table:");

                        for (int i = 0; i < rows; i++)
                        {
                            char[] curLineChars = Console.ReadLine().Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(char.Parse).ToArray();

                            if (cols != curLineChars.Length) throw new Exception();

                            for (int j = 0; j < cols; j++)
                            {
                                table[i, j] = curLineChars[j];
                            }
                        }

                        Console.WriteLine("Table input successful!");
                        invalidInput = false;
                    }
                    catch
                    {
                        Console.WriteLine("Table input is invalid!");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option! Please write just 1 or 2");
                }
            }

            return table;
        }

        static string WordInput()
        {
            bool invalidInput = true;
            string word = "";
            while(invalidInput)
            {
                Console.WriteLine("Word to search: ");
                word = Console.ReadLine();
                if (word.All(Char.IsLetter) && word != "")
                {
                    invalidInput = false;
                }
                else Console.WriteLine("Invalid input!");
            }

            return word;
        }

        static int DirectionCount(char[,] table, string word, int startRow, int startCol, int rowChange, int colChange)
        {
            bool found = true;
            for (int k = 0; k < word.Length; k++)
            {
                if (table[startRow + rowChange * k, startCol + colChange * k] != word[k])
                {
                    found = false;
                    break;
                }
            }
            if (found) return 1;
            else return 0;
        }

        static int WordCounter(char[,] table, string word, bool palindrom)
        {
            int rows = table.GetLength(0);
            int cols = table.GetLength(1);
            int wordL = word.Length;
            int timesFound = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (table[i, j] == word[0])
                    {
                        //Right direction word search
                        if (wordL <= cols - j)
                        {
                            timesFound += DirectionCount(table, word, i, j, 0, 1);
                        }
                        //Left direction word search
                        if (wordL <= j + 1 && !palindrom)
                        {
                            timesFound += DirectionCount(table, word, i, j, 0, -1);
                        }
                        //Up direction word search
                        if (wordL <= i + 1 && !palindrom)
                        {
                            timesFound += DirectionCount(table, word, i, j, -1, 0);
                        }
                        //Down direction word search
                        if (wordL <= rows - i)
                        {
                            timesFound += DirectionCount(table, word, i, j, 1, 0);
                        }
                        //Up-Right direction word search
                        if (wordL <= i + 1 && wordL <= cols - j && !palindrom)
                        {
                            timesFound += DirectionCount(table, word, i, j, -1, 1);
                        }
                        //Up-Left direction word search
                        if (wordL <= i + 1 && wordL <= j + 1 && !palindrom)
                        {
                            timesFound += DirectionCount(table, word, i, j, -1, -1);
                        }
                        //Down-Right direction word search
                        if (wordL <= rows - i && wordL <= cols - j)
                        {
                            timesFound += DirectionCount(table, word, i, j, 1, 1);
                        }
                        //Down-Left direction word search
                        if (wordL <= rows - i && wordL <= j + 1)
                        {
                            timesFound += DirectionCount(table, word, i, j, 1, -1);
                        }
                    }
                }
            }

            return timesFound;
        }

        static bool IsPalindrom(string word)
        {
            bool pal = true;
            int wordL = word.Length;
            for (int i = 0; i < wordL / 2; i++)
            {
                if (word[i] != word[wordL - i - 1])
                {
                    pal = false;
                    break;
                }
            }
            return pal;
        }

        static void Main(string[] args)
        {
            char[,] table = TableInput();
            string word = WordInput();
            bool palindrom = IsPalindrom(word);
            int timesFound = WordCounter(table, word, palindrom);

            Console.WriteLine("{0} is found {1} time(s).", word, timesFound);
            
            Console.ReadKey();
        }
    }
}
