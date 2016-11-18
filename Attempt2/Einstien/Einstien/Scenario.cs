using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Einstien
{
    class Scenario
    {
        private static Random _rnd = new Random();
        public Character TargetCharacter { get; set; }
        public List<Character> Characters { get; set; }
        public List<Clue> Clues { get; set; }
        public string TargetType { get; set; }
        public string StartingType { get; set; }
        public Dictionary<string, List<Attribute>> AttributesDictionary = new Dictionary<string, List<Attribute>>();
        public int TotalPossibleAttributes { get; set; }

        public void GenerateCharacters(List<Attribute> attributes, int numOfCharacters, string startingType, string targetType)
        {
            StartingType = startingType;
            TargetType = targetType;

            //Organise attributes into unique lists
            var distinctTypes = attributes.Select(x => x.Type).Distinct();
            Dictionary<string, List<Attribute>> typesDictionary = distinctTypes.ToDictionary(type => type, type => attributes.Where(x => x.Type == type).ToList());

            //PrintPossibleAttributes(typesDictionary);

            //Generate characters with random attributes
            Characters = new List<Character>();
            for (int i = 0; i < numOfCharacters; i++)
            {
                var character = new Character {Attributes = new Dictionary<string, Attribute>()};
                foreach (string type in typesDictionary.Keys)
                {
                    var index = _rnd.Next(typesDictionary[type].Count);
                    character.Attributes.Add( type,typesDictionary[type][index]);
                    typesDictionary[type].RemoveAt(index);
                }
                Characters.Add(character);
            }

            //Populate Attributes Dictionary
            foreach (var character in Characters)
            {
                foreach (var attribute in character.Attributes.Values)
                {
                    var attributeType = attribute.Type;
                    if (AttributesDictionary.ContainsKey(attributeType))
                    {
                        AttributesDictionary[attributeType].Add(attribute);
                    }
                    else
                    {
                        AttributesDictionary.Add(attributeType, new List<Attribute>());
                        AttributesDictionary[attributeType].Add(attribute);
                    }
                }
            }
            //PrintAttributesTable();

            //Calaculate Total Possible Attributes for each character
            TotalPossibleAttributes = 0;
            foreach (var attribute in AttributesDictionary.Values)
            {
                TotalPossibleAttributes += attribute.Count;
            }

            //Initialize Elimination Table - Each character has default values
            foreach (var character in Characters)
            {
                character.Possibilities = new Dictionary<string, List<Attribute>>(AttributesDictionary);
                //Clone lists
                foreach (var key in AttributesDictionary.Keys)
                {
                    character.Possibilities[key] = new List<Attribute>(character.Possibilities[key]);
                }
            }

            //Initialize Starting Known Attribute
            foreach (var character in Characters)
            {
                character.Possibilities[startingType] = new List<Attribute> { character.Attributes[startingType] };
            }
            //PrintCharacterPossibilities();

            //Select Target Character
            int targetCharacterIndex = _rnd.Next(numOfCharacters);
            TargetCharacter = Characters[targetCharacterIndex];

            //Generate a list of all possible clues
            var possibleClues = new List<Clue>();
            foreach (var character in Characters)
            {
                foreach (var i in character.Attributes.Keys)
                {
                    foreach (var j in character.Attributes.Keys)
                    {
                        if (j != i)
                        {
                            var clue = new Clue
                            {
                                Val1 = character.Attributes[i],
                                Val2 = character.Attributes[j]
                            };
                            if (character == TargetCharacter && ((clue.Val1.Type == targetType && clue.Val2.Type == startingType) || (clue.Val2.Type == targetType && clue.Val1.Type == startingType)))
                            {
                                clue.ClueType = ClueType.Solution;
                            }
                            else if ((character == TargetCharacter) &&
                                        ((clue.Val1.Type == targetType && clue.Val2.Type != startingType) ||
                                        (clue.Val2.Type == targetType && clue.Val1.Type != startingType) ||
                                        (clue.Val1.Type != targetType && clue.Val2.Type == startingType) ||
                                        (clue.Val2.Type != targetType && clue.Val1.Type == startingType)))
                            {
                                clue.ClueType = ClueType.BuildOn;
                            }
                            else if ((character != TargetCharacter) &&
                                        ((clue.Val1.Type == targetType && clue.Val2.Type != startingType) ||
                                        (clue.Val2.Type == targetType && clue.Val1.Type != startingType) ||
                                        (clue.Val1.Type != targetType && clue.Val2.Type == startingType) ||
                                        (clue.Val2.Type != targetType && clue.Val1.Type == startingType)))
                            {
                                clue.ClueType = ClueType.Indirect;
                            }
                            else if (character == TargetCharacter)
                            {
                                clue.ClueType = ClueType.Attached;
                            }
                            else if (character != TargetCharacter &&
                                        (clue.Val1.Type == targetType && clue.Val2.Type == startingType) ||
                                        (clue.Val2.Type == targetType && clue.Val1.Type == startingType))
                            {
                                clue.ClueType = ClueType.RuleOut;
                            }
                            else
                            {
                                clue.ClueType = ClueType.Detached;
                            }
                            var duplicateCheck =
                                possibleClues.Where(x => x.Val1.Type == clue.Val2.Type)
                                    .Where(x => x.Val2.Type == clue.Val1.Type)
                                    .Where(x => x.Val1.Name == clue.Val2.Name)
                                    .Where(x => x.Val2.Name == clue.Val1.Name)
                                    .ToList();
                            if (duplicateCheck.Count == 0)
                            {
                            possibleClues.Add(clue);
                            }
                        }
                    }
                }
            }

            //PrintCharacters();
            //PrintListOfClues(possibleClues);

            //Add clues until solvable
            int sentinel = 0;
            Clues = new List<Clue>();
            var targetAttribute = TargetCharacter.Attributes.FirstOrDefault(x => x.Key == targetType).Value;
            bool targetAttributeMentioned = false;
            bool startingAttributeMentioned = false;
            while ((!startingAttributeMentioned || !targetAttributeMentioned || TargetCharacter.Possibilities[targetType].Count > 1 || TargetCharacter.Possibilities[startingType].Count > 1) && sentinel < 1000 && possibleClues.Count > 0)
            {
                sentinel++;
                int selectedClueIndex = _rnd.Next(possibleClues.Count);
                var selectedClue = possibleClues[selectedClueIndex];
                if (selectedClue.ClueType != ClueType.Solution)
                {
                    Clues.Add(possibleClues[selectedClueIndex]);
                }
                possibleClues.Remove(selectedClue);

                
                foreach (var clue in Clues)
                {
                    bool newChar = true;
                    foreach (var character in Characters)
                    {
                        if ((clue.Val1.Type == startingType && clue.Val1.Name == TargetCharacter.Attributes[startingType].Name) || (clue.Val2.Type == startingType && clue.Val2.Name == TargetCharacter.Attributes[startingType].Name))
                        {
                            startingAttributeMentioned = true;
                        }

                        if ((clue.Val1.Type == targetType && clue.Val1.Name == TargetCharacter.Attributes[targetType].Name) || (clue.Val2.Type == targetType && clue.Val2.Name == TargetCharacter.Attributes[targetType].Name))
                        {
                            targetAttributeMentioned = true;
                        }

                        if (character.Possibilities[clue.Val1.Type].Contains(clue.Val1) == false)
                        {
                            if (character.Possibilities[clue.Val2.Type].Contains(clue.Val2))
                            {
                                character.Possibilities[clue.Val2.Type].Remove(clue.Val2);
                                newChar = false;
                            }
                        }
                        else if(character.Possibilities[clue.Val1.Type].Count == 1)
                        {
                            character.Possibilities[clue.Val2.Type] = new List<Attribute>() {clue.Val2};
                            newChar = false;
                        }

                        if (character.Possibilities[clue.Val2.Type].Contains(clue.Val2) == false)
                        {
                            if (character.Possibilities[clue.Val1.Type].Contains(clue.Val1))
                            {
                                character.Possibilities[clue.Val1.Type].Remove(clue.Val1);
                                newChar = false;
                            }
                        }
                        else if(character.Possibilities[clue.Val2.Type].Count == 1)
                        {
                            character.Possibilities[clue.Val1.Type] = new List<Attribute>() { clue.Val1 };
                            newChar = false;
                        }
                    }

                    //if (newChar)
                    //{
                    //    foreach(var character in Characters)
                    //    {
                    //        var count = 0;
                    //        foreach(var possibility in character.Possibilities.Values)
                    //        {
                    //            count += possibility.Count;
                    //        }
                    //        if(count == TotalPossibleAttributes)
                    //        {
                    //            character.Possibilities[clue.Val1.Type] = new List<Attribute>() { clue.Val1 };
                    //            character.Possibilities[clue.Val2.Type] = new List<Attribute>() { clue.Val2 };
                    //            break;
                    //        }
                    //    }
                    //}
                }

            }

            //PrintCharacterPossibilities();
            //PrintCharacters();
            Console.WriteLine("THE MURDER WEAPON WAS: " + targetAttribute.Name + "\n");
            PrintClues();

            if (possibleClues.Count == 0)
            {
                Console.WriteLine("Failed!");
            }
        }

        

        private void PrintPossibleAttributes(Dictionary<string, List<Attribute>> typesDictionary)
        {
            Console.WriteLine("##ATTRIBUTES##");
            foreach (string type in typesDictionary.Keys)
            {
                Console.WriteLine(type);
                foreach (Attribute attribute in typesDictionary[type])
                {
                    Console.WriteLine(attribute.Name);
                }
                Console.WriteLine("");
            }
        }

        private void PrintListOfClues(List<Clue> clues)
        {
            Console.WriteLine("##POSSIBLE CLUES##");
            foreach (Clue clue in clues)
            {
                Console.WriteLine("Person with " + clue.Val1.Type + " : " + clue.Val1.Name + " \nHas " + clue.Val2.Type + " : " + clue.Val2.Name + "\nClue Type: " + clue.ClueType);
                Console.WriteLine("");
            }
        }

        private void PrintCharacters()
        {
            Console.WriteLine("##CHARACTERS##");
            foreach (var character in Characters)
            {
                if (character == TargetCharacter)
                {
                    Console.WriteLine("TARGET CHARACTER");
                }
                foreach (var attribute in character.Attributes.Keys)
                {
                    Console.WriteLine(attribute + " : " + character.Attributes[attribute].Name);
                }
                Console.WriteLine("");
            }
        }

        private void PrintAttributesTable()
        {
            foreach (var key in AttributesDictionary.Keys)
            {
                Console.WriteLine("KEY: " + key);
                foreach (var value in AttributesDictionary[key])
                {
                    Console.WriteLine(value.Name);
                }
                Console.WriteLine("");
            }
        }

        private void PrintCharacterPossibilities()
        {
            foreach (var character in Characters)
            {
                Console.WriteLine("###Character Possibities###");
                foreach (var key in character.Possibilities.Keys)
                {
                    Console.WriteLine("KEY: " + key);
                    foreach (var value in character.Possibilities[key])
                    {
                        Console.WriteLine(value.Name);
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("");
            }
        }

        private void PrintClues()
        {
            Console.WriteLine("###CLUES###");
            foreach (var clue in Clues)
            {
                Console.WriteLine("The person with a " + clue.Val1.Type + " of " + clue.Val1.Name + " has a " + clue.Val2.Type + " of " + clue.Val2.Name);
            }
        }
    }
}
