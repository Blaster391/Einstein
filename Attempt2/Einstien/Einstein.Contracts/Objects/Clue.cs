namespace Einstein.Contracts.Objects
{
    public enum ClueType
    {
        Unknown,
        Solution,
        Indirect,
        RuleOut,
        BuildOn,
        Attached,
        Detached
    }

    public class Clue
    {
        public Attribute Val1 { get; set; }
        public Attribute Val2 { get; set; }

        public bool Negative { get; set; }
        public ClueType ClueType { get; set; }
        
    }
}
