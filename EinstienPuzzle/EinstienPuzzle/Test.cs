using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinstienPuzzle
{
    class Test
    {
        public static int Main()
        {
            Solution s = new Solution();
            s.solve(new Generator());
            s.printSolutionTable();

            Console.WriteLine(s.getHorizonal(0,3));
            Console.WriteLine(s.getHorizonal(1, 3));
            Console.WriteLine(s.getHorizonal(2, 3));

            Console.WriteLine(" ");

            Console.WriteLine(s.getHorizonal(0,4) + " " + s.getVertical(0, 4));
            Console.WriteLine(s.getHorizonal(1, 4) + " " + s.getVertical(1, 4));
            Console.WriteLine(s.getHorizonal(2, 4) + " " + s.getVertical(2, 4));
            Console.WriteLine(s.getHorizonal(3, 4) + " " + s.getVertical(3, 4));
            Console.WriteLine(s.getHorizonal(4, 4) + " " + s.getVertical(4, 4));
            Console.WriteLine(s.getHorizonal(5, 4) + " " + s.getVertical(5, 4));

            Console.ReadLine();
            return 0;
        }
    }
}
