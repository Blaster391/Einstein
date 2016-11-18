using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Einstien
{
    class Program
    {
        static void Main(string[] args)
        {
            var attributes = new List<Attribute>()
            {
                new Attribute()
                {
                    Type = "Name",
                    Name = "Garry",
                    Description = "blah"
                },
                new Attribute()
                {
                    Type = "Name",
                    Name = "Craig",
                    Description = "blah"
                },
                new Attribute()
                {
                    Type = "Name",
                    Name = "Matt",
                    Description = "blah"
                },
                new Attribute()
                {
                    Type = "Name",
                    Name = "Will",
                    Description = "Blind"
                },
                new Attribute()
                {
                    Type = "Age",
                    Name = "10",
                    Description = "blah"
                },
                new Attribute()
                {
                    Type = "Age",
                    Name = "25",
                    Description = "blah"
                },
                new Attribute()
                {
                    Type = "Age",
                    Name = "40",
                    Description = "blah"
                },
                new Attribute()
                {
                    Type = "Age",
                    Name = "50",
                    Description = "blah"
                },
                new Attribute()
                {
                    Type = "Weapon",
                    Name = "Knife",
                    Description = "blah"
                }
                ,
                new Attribute()
                {
                    Type = "Weapon",
                    Name = "Sword",
                    Description = "blah"
                },
                new Attribute()
                {
                    Type = "Weapon",
                    Name = "Rifle",
                    Description = "blah"
                },
                new Attribute()
                {
                    Type = "Hair Colour",
                    Name = "Red",
                    Description = "blah"
                },
                new Attribute()
                {
                    Type = "Hair Colour",
                    Name = "Green",
                    Description = "blah"
                },
                new Attribute()
                {
                    Type = "Hair Colour",
                    Name = "Blue",
                    Description = "blah"
                },
                new Attribute()
                {
                    Type = "Nationality",
                    Name = "English",
                    Description = "blah"
                },
                new Attribute()
                {
                    Type = "Nationality",
                    Name = "Canadian",
                    Description = "blah"
                },
                new Attribute()
                {
                    Type = "Nationality",
                    Name = "Irish",
                    Description = "blah"
                }
            };

            var scenario = new Scenario();
            scenario.GenerateCharacters(attributes, 3, "Name", "Weapon");
            bool solved = false;
            while (!solved)
            {
                var input = Console.ReadLine();
                if(input == scenario.TargetCharacter.Attributes["Name"].Name)
                {
                    Console.WriteLine("CORRECT");
                    solved = true;
                }
                else
                {
                    Console.WriteLine("WRONG TRY AGAIN");
                }
            }
            Console.ReadLine();
        }
    }
}
