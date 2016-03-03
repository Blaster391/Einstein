﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinstienPuzzle
{
    class Generator
    {
        Random rnd = new Random();

        Attribute targetAttribute;
        AttributeValue targetValue;
        List<Character> characters = new List<Character>();
        List<Attribute> attributes = new List<Attribute>();
        AttributeValue[,] table;


        public static int Main()
        {
            Generator g = new Generator();
 
            g.generate();
          
            Console.WriteLine("######SOLUTION TABLE#########");
            Console.WriteLine(" ");

            for (int i = 0; i < g.attributes.Count; i++)
            {
                for (int j = 0; j < g.characters.Count; j++)
                {
                    Console.Write(g.table[i, j].getDescription() + "\t");
                }
                Console.WriteLine(" ");
            }

            Console.WriteLine(" ");
            Console.WriteLine("Target is: " + g.targetValue.getDescription());
            Console.WriteLine("Character Index is " + g.getTargetCharIndex());

            Console.ReadLine();
            return 0;
        }


        public void generate() {

            generatePlaceholderAttributes();
            generateTable();
            generateTarget();

        }

        void generatePlaceholderAttributes()
        {
            Character char1 = new Character();
            Character char2 = new Character();
            Character char3 = new Character();
            characters.Add(char1);
            characters.Add(char2);
            characters.Add(char3);

            Attribute name = new Attribute();
            AttributeValue barry = new AttributeValue(name, "Barry");
            AttributeValue jeff = new AttributeValue(name, "Jeff");
            AttributeValue jacob = new AttributeValue(name, "Jacob");
            attributes.Add(name);

            Attribute weapon = new Attribute();
            setTargetAttribute(weapon);
            AttributeValue chainsaw = new AttributeValue(weapon, "Chainsaw");
            AttributeValue axe = new AttributeValue(weapon, "Axe");
            AttributeValue knife = new AttributeValue(weapon, "Knife");
            attributes.Add(weapon);

            Attribute hat= new Attribute();
            setTargetAttribute(weapon);
            AttributeValue tophat = new AttributeValue(hat, "Top Hat");
            AttributeValue trilby = new AttributeValue(hat, "Trilby");
            AttributeValue beanie = new AttributeValue(hat, "Beanie");
            attributes.Add(hat);
        }

        void generateTable()
        {
            table = new AttributeValue[attributes.Count, characters.Count];
            for(int i = 0; i < attributes.Count; i++)
            {
                AttributeValue[] attributesValuesArray = attributes[i].getCopyOfAttibutes().OrderBy(x => rnd.Next()).ToArray();
                for (int j = 0; j < characters.Count; j++)
                {
                    table[i, j] = attributesValuesArray[j];
                }
            }
        }

        public void generateTarget()
        {
            int randomIndex = rnd.Next(0, targetAttribute.getValueCount());
            targetValue = targetAttribute.getValue(randomIndex);

        }

        void setTargetAttribute(Attribute target)
        {
            this.targetAttribute = target;
        }

        Character getTargetChar()
        {
            return characters[getTargetCharIndex()];
        }

        int getTargetCharIndex()
        {
            int attributeIndex = 0;
            while (attributes[attributeIndex] != targetAttribute)
            {
                attributeIndex++;
            }
            int charIndex = 0;
            while (table[attributeIndex, charIndex] != targetValue)
            {
                charIndex++;
            }
            return charIndex;
        }

    }
}
