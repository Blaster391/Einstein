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
            Generator g = new Generator();
            g.generate();
            g.printSolution();
            s.solve(g);
            s.printSolutionTable();

            Console.WriteLine(g.getTargetAttributeIndex() + " " + g.getTargetCharIndex());
            Console.WriteLine(" ");
            Console.WriteLine(g.getSolutionValue().getDescription()+ " " + g.getTargetValue().getDescription());
          


            Console.ReadLine();
            return 0;
        }
    }
}
