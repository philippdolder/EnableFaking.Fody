namespace EnableFaking.Fody
{
    using System.Collections.Generic;
    using Mono.Cecil;

    public class MemberSelector
    {
        public IEnumerable<MethodDefinition> Select(TypeDefinition type)
        {
            var membersToProcess = new List<MethodDefinition>();
            foreach (MethodDefinition member in type.Methods)
            {
                if (member.IsPublic 
                    && !member.IsStatic 
                    && !member.IsConstructor
                    && !member.IsVirtual)
                {
                    membersToProcess.Add(member);
                }
            }

            return membersToProcess;
        }
    }
}