namespace Assets.Scripts.Crypts.Flyweights
{
    public class CharacterOrigin
    {
        public int Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public CharacterOrigin(int code, string name, string description)
        {
            Code = code;
            Name = name;
            Description = description;
        }
    }
}