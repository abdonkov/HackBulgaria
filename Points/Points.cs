using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points
{
    class Points
    {
        static int[] PointInput()
        {
            bool validPoint = false;
            int[] point = { 1 };
            while (!validPoint)
            {
                Console.Write("Starting point: ");
                try
                {
                    point = Console.ReadLine().Split(new char[] { '(', ',', ' ', ')' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                    validPoint = true;
                }
                catch
                {
                    Console.WriteLine("Invalid input! Please use one of the formats: (x, y) | x, y | x y");
                }

                if (point.Length < 2 && validPoint)
                {
                    Console.WriteLine("Invalid input! Not enough integers on input!");
                    validPoint = false;
                }
            }

            return point;
        }

        static void Main(string[] args)
        {
            int[] point = PointInput();

            Console.Write("String: ");
            string directionString = Console.ReadLine();

            int curX = point[0];
            int curY = point[1];

            bool reversed = false;
            for (int i = 0; i < directionString.Length; i++)
            {
                char action = directionString[i];
                switch (action)
                {
                    case '<': { curX += reversed ? 1 : -1; } break;
                    case '>': { curX += reversed ? -1 : 1; } break;
                    case '^': { curY += reversed ? 1 : -1; } break;
                    case 'v': { curY += reversed ? -1 : 1; } break;
                    case '~': { reversed = !reversed; } break;
                    default: { } break;
                }
            }
            Console.WriteLine("({0}, {1})", curX, curY);
            Console.ReadKey();
        }
    }
}
