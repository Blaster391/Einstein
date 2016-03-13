using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinstienPuzzle
{
    class AttributeValue
    {
        Attribute attribute;
        string description;

        public AttributeValue(Attribute a, String description)
        {
            attribute = a;
            attribute.addValue(this);
            this.description = description;
        }

        public string getDescription()
        {
            return description;
        }

        public Attribute getAttribute()
        {
            return attribute;
        }
    }
}
