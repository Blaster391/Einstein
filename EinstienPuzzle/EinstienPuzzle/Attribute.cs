using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinstienPuzzle
{
    class Attribute
    {

        List<AttributeValue> values = new List<AttributeValue>();
        
        public void addValue(AttributeValue val)
        {
            values.Add(val);
        } 

        public AttributeValue getValue(int index)
        {
            return values[index];
        }

        public int getValueCount()
        {
            return values.Count();
        }

        public AttributeValue[] getCopyOfAttibutes()
        {
            AttributeValue[] array = new AttributeValue[values.Count]; 
            values.CopyTo(array);
            return array;
        }
    }
}
