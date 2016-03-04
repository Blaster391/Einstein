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

            Console.ReadLine();
            return 0;
        }
    }
}
