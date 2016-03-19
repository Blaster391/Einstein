using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinstienPuzzle
{
    class Solution
    {
        public enum valueStatus
        {
            UNKNOWN,
            FALSE,
            TRUE,
            SOLUTION,
            UNDEFINED
        }

        valueStatus[,,] solutionTable;
        Generator gen = new Generator();
        List<Clue> possibleClues = new List<Clue>();

        public void solve(Generator g)
        {
            gen = g;
            //gen.generate();

            solutionTable = new valueStatus[gen.getCharactersCount(), gen.getCharactersCount(), getNumberOfTables(gen.getAttributesCount())];

            setSolution();
        }

        public void setSolution()
        {

            solutionTable[gen.getTargetAttributeIndex(), gen.getTargetCharIndex(), 0] = valueStatus.SOLUTION;
        }

        public int getNumberOfTables(int size)
        {
            if(size <= 2)
            {
                return 1;
            }
            else
            {
                return size - 1 + getNumberOfTables(size - 1); 
            }
        }

        /*Okay bear with me a sec, possible future me
        This gets the index of the attribute that values need to go
        in table x

        Got it? Good because I don't either, YAY RECURSION
    */
        public int getHorizonal(int x)
        {
            return getHorizonal(x, gen.getAttributesCount());
        }

        public int getVertical(int x)
        {
            return getVertical(x, gen.getAttributesCount());
        }

        public int getHorizonal(int x, int n)
        {
            if (x < (n-1))
            {
                return 0;
            }
            else
            {
                n--;
                return getHorizonal(x - n, n) + 1;
            }
        }

        public int getVertical(int x, int n)
        {
            if (x < n - 1)
            {
                return x + 1;
            }
            else
            {
                n--;
                return getVertical(x - n, n) + 1;
            }
        }

        public valueStatus getStatusFromIndex(int x, int y, int z)
        {
            return solutionTable[x, y, z];
        }

        public valueStatus findRelationship(AttributeValue val1, AttributeValue val2)
        {

            return valueStatus.UNDEFINED; //TODO Define This
        }

        public void printSolutionTable()
        {
            Console.WriteLine("######SOLVER TABLE#########");
            Console.WriteLine(" ");
            for (int k = 0; k < getNumberOfTables(gen.getAttributesCount()); k++)
            {
                Console.Write("\t");
                for (int l = 0; l < gen.getCharactersCount(); l++)
                {
                    Console.Write(gen.getAttributeValueFromTable(getVertical(k), l).getDescription() + "\t");
                }
                Console.WriteLine("");

                for (int j = 0; j < gen.getCharactersCount(); j++)
                {
                    Console.Write(gen.getAttributeValueFromTable(getHorizonal(k), j).getDescription() + "\t");
                    for (int i = 0; i < gen.getCharactersCount(); i++)
                    {
                        Console.Write(solutionTable[i, j, k] + " ");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine(" ");
                Console.Beep();
            }
        }


    }
}
