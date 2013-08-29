namespace EnableFaking.Fody
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FluentAssertions;
    using Mono.Cecil;
    using Xunit;

    public class TypeSelectorTest
    {
        private readonly TypeSelector testee;
        private readonly ModuleDefinition moduleDefinition;

        public TypeSelectorTest()
        {
            this.testee = new TypeSelector();

            string assemblyPath = Assembly.GetExecutingAssembly().CodeBase
                .Replace("file:///", string.Empty)
                .Replace("EnableFaking.Fody.Test", "AssemblyToProcess");

            this.moduleDefinition = ModuleDefinition.ReadModule(assemblyPath);
        }

        [Fact]
        public void SelectsPublicLogicClasses()
        {
            IEnumerable<TypeDefinition> result = this.testee.Select(this.moduleDefinition).ToList();

            result.Should().Contain(_ => _.FullName == "AssemblyToProcess.PublicLogic");
        }

        [Fact]
        public void SelectsPublicClassesWithDoVirtualizeAttribute()
        {
            var result = this.testee.Select(this.moduleDefinition).ToList();

            result.Should().Contain(_ => _.FullName == "AssemblyToProcess.PublicClassWithDoVirtualizeAttribute");
        }

        [Fact]
        public void SkipsClassesImplementingAnInterface()
        {
            IEnumerable<TypeDefinition> result = this.testee.Select(this.moduleDefinition).ToList();

            result.Should().NotContain(_ => _.FullName == "AssemblyToProcess.PublicLogicImplementingInterface");
        }

        [Fact]
        public void SkipsContainers()
        {
            IEnumerable<TypeDefinition> result = this.testee.Select(this.moduleDefinition).ToList();

            result.Should().NotContain(_ => _.FullName == "AssemblyToProcess.PublicImmutableContainer");
            result.Should().NotContain(_ => _.FullName == "AssemblyToProcess.PublicContainer");
        }

        [Fact]
        public void SkipsInterfaces()
        {
            IEnumerable<TypeDefinition> result = this.testee.Select(this.moduleDefinition).ToList();

            result.Should().NotContain(_ => _.FullName == "AssemblyToProcess.ILogicInterface");
        }

        [Fact]
        public void SkipsEnums()
        {
            IEnumerable<TypeDefinition> result = this.testee.Select(this.moduleDefinition).ToList();

            result.Should().NotContain(_ => _.FullName == "AssemblyToProcess.PublicEnum");
        }

        [Fact]
        public void SkipsStructs()
        {
            IEnumerable<TypeDefinition> result = this.testee.Select(this.moduleDefinition).ToList();

            result.Should().NotContain(_ => _.FullName == "AssemblyToProcess.PublicStruct");
        }
    }
}