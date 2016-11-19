using System.Collections.Generic;

namespace Einstein.Contracts.Objects
{
    public class Character
    {
        public Dictionary<string,Attribute> Attributes { get; set; }
        public Dictionary<string, List<Attribute>> Possibilities { get; set; }
    }
}
