using System;
using System.Collections.Generic;

namespace EinstienPuzzle
{
    internal class Solution
    {
        public enum ValueStatus
        {
            Unknown,
            False,
            True,
            Solution,
            Undefined
        }

        private Generator _gen = new Generator();
        private List<Clue> _possibleClues = new List<Clue>();

        private ValueStatus[,,] _solutionTable;

        public void Solve(Generator g)
        {
            _gen = g;
            //gen.generate();

            _solutionTable =
                new ValueStatus[_gen.GetCharactersCount(), _gen.GetCharactersCount(),
                    GetNumberOfTables(_gen.GetAttributesCount())];

            SetSolution();
            GetDirectClues();
        }

        public void SetSolution()
        {
            _solutionTable[_gen.GetTargetAttributeIndex(), _gen.GetTargetCharIndex(), 0] = ValueStatus.Solution;
        }

        public void GetDirectClues()
        {
        }

        public int GetNumberOfTables(int size)
        {
            if (size <= 2)
                return 1;
            return size - 1 + GetNumberOfTables(size - 1);
        }

        /*Okay bear with me a sec, possible future me
        This gets the index of the attribute that values need to go
        in table x

        Got it? Good because I don't either, YAY RECURSION
    */

        public int GetHorizonal(int x)
        {
            return GetHorizonal(x, _gen.GetAttributesCount());
        }

        public int GetVertical(int x)
        {
            return GetVertical(x, _gen.GetAttributesCount());
        }

        public int GetHorizonal(int x, int n)
        {
            if (x < n - 1)
                return 0;
            n--;
            return GetHorizonal(x - n, n) + 1;
        }

        public int GetVertical(int x, int n)
        {
            if (x < n - 1)
            {
                return x + 1;
            }
            n--;
            return GetVertical(x - n, n) + 1;
        }

        public ValueStatus GetStatusFromIndex(int x, int y, int z)
        {
            return _solutionTable[x, y, z];
        }

        public ValueStatus FindRelationship(AttributeValue val1, AttributeValue val2)
        {
            return ValueStatus.Undefined; //TODO Define This
        }

        public void PrintSolutionTable()
        {
            Console.WriteLine("######SOLVER TABLE#########");
            Console.WriteLine(" ");
            for (var k = 0; k < GetNumberOfTables(_gen.GetAttributesCount()); k++)
            {
                Console.Write("\t");
                for (var l = 0; l < _gen.GetCharactersCount(); l++)
                    Console.Write(_gen.GetAttributeValueFromTable(GetVertical(k), l).GetDescription() + "\t");
                Console.WriteLine("");

                for (var j = 0; j < _gen.GetCharactersCount(); j++)
                {
                    Console.Write(_gen.GetAttributeValueFromTable(GetHorizonal(k), j).GetDescription() + "\t");
                    for (var i = 0; i < _gen.GetCharactersCount(); i++)
                        Console.Write(_solutionTable[i, j, k] + " ");
                    Console.WriteLine("");
                }
                Console.WriteLine(" ");
                Console.Beep();
            }
        }
    }
}