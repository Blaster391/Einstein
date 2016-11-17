using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Einstien
{
    class Character
    {
        public Dictionary<string,Attribute> Attributes { get; set; }
        public Dictionary<string, List<Attribute>> Possibilities { get; set; }
    }
}
