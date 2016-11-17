using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Einstien
{
    enum ClueType
    {
        Unknown,
        Solution,
        Indirect,
        RuleOut,
        BuildOn,
        Attached,
        Detached
    }

    class Clue
    {
        public Attribute Val1 { get; set; }
        public Attribute Val2 { get; set; }

        public bool Negative { get; set; }
        public ClueType ClueType { get; set; }
        
    }
}
