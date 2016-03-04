using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinstienPuzzle
{
    class Solution
    {
        enum valueStatus
        {
            UNKNOWN,
            FALSE,
            TRUE
        }

        valueStatus[,,] solutionTable;

        public void solve(Generator g)
        {
            g.generate();

            solutionTable = new valueStatus[g.getAttributesCount(), g.getAttributesCount(), getNumberOfTables(g.getAttributesCount())];


        }

        public int getNumberOfTables(int size)
        {
            if(size > 1)
            {
                return size*size - getNumberOfTables(size - 1);
            }
            else
            {
                return 1;
            }
        }

        public void printSolutionTable()
        {

        }


    }
}
