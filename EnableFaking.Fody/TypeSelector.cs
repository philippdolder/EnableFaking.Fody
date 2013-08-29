namespace EnableFaking.Fody
{
    using System.Collections.Generic;
    using System.Linq;

    using Mono.Cecil;

    public class TypeSelector
    {
        public IEnumerable<TypeDefinition> Select(ModuleDefinition moduleDefinition)
        {
            var typesToProcess = new List<TypeDefinition>();
            foreach (TypeDefinition type in moduleDefinition.GetTypes())
            {
                if (CanVirtualize(type))
                {
                    if ((!IsContainer(type) && !ImplementsInterfaces(type)) 
                        || HasDoVirtualizeAttribute(type))
                    {
                        typesToProcess.Add(type);
                    }
                }
            }

            return typesToProcess;
        }

        private static bool CanVirtualize(TypeDefinition type)
        {
            return IsPublicClass(type)
                   && IsExtensible(type);
        }

        private static bool HasDoVirtualizeAttribute(TypeDefinition type)
        {
            return type.CustomAttributes.Any(_ => _.AttributeType.Name == "DoVirtualizeAttribute");
        }

        private static bool IsPublicClass(TypeDefinition type)
        {
            return type.IsPublic 
                && type.IsClass
                && !type.IsNested;
        }

        private static bool IsExtensible(TypeDefinition type)
        {
            return !type.IsSealed;
        }

        private static bool IsContainer(TypeDefinition type)
        {
            return type.Methods.All(_ => _.IsGetter || _.IsSetter || _.IsConstructor);
        }

        private static bool ImplementsInterfaces(TypeDefinition type)
        {
            return type.HasInterfaces;
        }
    }
}