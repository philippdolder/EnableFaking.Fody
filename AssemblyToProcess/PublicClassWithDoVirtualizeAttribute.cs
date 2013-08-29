namespace AssemblyToProcess
{
    [DoVirtualize]
    public class PublicClassWithDoVirtualizeAttribute
    {
        public PublicClassWithDoVirtualizeAttribute(string name, int value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; private set; }

        public int Value { get; private set; }
    }
}