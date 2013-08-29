namespace EnableFaking.Fody
{
    using System.Collections.Generic;
    using Mono.Cecil;

    public class MemberVirtualizer
    {
        public void Virtualize(IEnumerable<MethodDefinition> members)
        {
            foreach (MethodDefinition member in members)
            {
                member.IsVirtual = true;
                member.IsNewSlot = true;
            }
        }
    }
}