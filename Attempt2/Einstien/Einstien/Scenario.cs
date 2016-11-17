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

        public void GenerateCharacters(List<Attribute> attributes, int numOfCharacters, string startingType, string targetType)
        {
            StartingType = startingType;
            TargetType = targetType;

            //Organise attributes into unique lists
            var distinctTypes = attributes.Select(x => x.Type).Distinct();
            Dictionary<string, List<Attribute>> typesDictionary = distinctTypes.ToDictionary(type => type, type => attributes.Where(x => x.Type == type).ToList());

            PrintPossibleAttributes(typesDictionary);

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
            PrintAttributesTable();

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
                character.Possibilities[startingType] = new List<Attribute> {character.Attributes[startingType]};
            }
            PrintCharacterPossibilities();

            //Select Target Character
            int targetCharacterIndex = _rnd.Next(numOfCharacters);
            TargetCharacter = Characters[targetCharacterIndex];

            //Generate a list of all possible clues
            var possibleClues = new List<Clue>();
            foreach (var character in Characters)
            {
                var generatedTypes = new List<string>();
                foreach (var i in character.Attributes.Keys)
                {
                    if (!generatedTypes.Contains(i))
                    {
                        foreach (var j in character.Attributes.Keys)
                        {
                            if (!generatedTypes.Contains(j) && (j != i))
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
                                possibleClues.Add(clue);
                            }
                            generatedTypes.Add(j);
                        }
                    }
                    generatedTypes.Add(i);
                }
            }

            PrintCharacters();
            PrintListOfClues(possibleClues);

            //Add clues until solvable
            int sentinel = 0;
            Clues = new List<Clue>();
            var targetAttribute = TargetCharacter.Attributes.FirstOrDefault(x => x.Key == targetType).Value;
            while (TargetCharacter.Possibilities[targetType].Count > 1 && sentinel < 1000 && possibleClues.Count > 0)
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
                    foreach (var character in Characters)
                    {
                        if (character.Possibilities[clue.Val1.Type].Contains(clue.Val1) == false)
                        {
                            if (character.Possibilities[clue.Val2.Type].Contains(clue.Val2))
                            {
                                character.Possibilities[clue.Val2.Type].Remove(clue.Val2);
                            }
                        }
                    }
                }

            }
            PrintCharacterPossibilities();

            //while (targetAttribute.Known == false && sentinel < 1000 && possibleClues.Count > 0) //TODO "while solution not found"
            //{
            //    int selectedClueIndex = _rnd.Next(possibleClues.Count);
            //    if (possibleClues[selectedClueIndex].ClueType != ClueType.Solution)
            //    {
            //        Clues.Add(possibleClues[selectedClueIndex]);
            //    }
            //    possibleClues.RemoveAt(selectedClueIndex);

            //    foreach (var clue in Clues)
            //    {
            //       // clue.Val1.Known = true;
            //       // clue.Val2.Known = true;

            //        //foreach (var type in typesDictionary.Values)
            //        //{
            //        //    if (type.Where(x => !x.Known).ToList().Count == 1)
            //        //    {
            //        //        type.First(x => !x.Known).Known = true;
            //        //    }
            //        //}

            //        foreach (var character in Characters)
            //        {

            //        }

            //        //var character = possibleCharacters.Where(x => x.Attributes.Any(x => x.Name == clue.Val1.Name && x.Type == clue.Val1.Type)).FirstOrDefault();
            //        //character.Attributes()

            //        //if (clue.ClueType == ClueType.Solution)
            //        //{
            //        //    //Ignore solution for now
            //        //}
            //        //else if (clue.ClueType == ClueType.RuleOut)
            //        //{
            //        //    var typeText = "";
            //        //    var nameText = "";
            //        //    if (clue.Val1.Type == startingType || clue.Val1.Type == targetType)
            //        //    {
            //        //        typeText = clue.Val1.Type;
            //        //        nameText = clue.Val1.Name;
            //        //    }
            //        //    else
            //        //    {
            //        //        typeText = clue.Val2.Type;
            //        //        nameText = clue.Val2.Name;
            //        //    }
            //        //    var charToRemove = possibleCharacters.Where(x => x.Attributes.Any(x => x.Name == nameText && x.Type == typeText)).FirstOrDefault();
            //        //    possibleCharacters.Remove(charToRemove);
            //        //}
            //    }

            //    sentinel++;
            //}

            PrintCharacters();
            PrintClues();
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
                Console.WriteLine(clue.Val1.Type + " - " + clue.Val1.Name + " has " + clue.Val2.Type + " - " + clue.Val2.Name);
            }
        }
    }
}
