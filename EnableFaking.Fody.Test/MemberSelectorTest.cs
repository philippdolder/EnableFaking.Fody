namespace EnableFaking.Fody
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FluentAssertions;
    using Mono.Cecil;
    using Xunit;

    public class MemberSelectorTest
    {
        private readonly MemberSelector testee;
        private readonly ModuleDefinition moduleDefinition;

        public MemberSelectorTest()
        {
            this.testee = new MemberSelector();

            string assemblyPath = Assembly.GetExecutingAssembly().CodeBase
                .Replace("file:///", string.Empty)
                .Replace("EnableFaking.Fody.Test", "AssemblyToProcess");

            this.moduleDefinition = ModuleDefinition.ReadModule(assemblyPath);
        }

        [Fact]
        public void SelectsPublicMethods()
        {
            IEnumerable<MethodDefinition> result = this.testee.Select(this.GetTestClass()).ToList();

            result.Should().OnlyContain(_ => _.IsPublic);
        }

        [Fact]
        public void SkipsPrivateMethods()
        {
            IEnumerable<MethodDefinition> result = this.testee.Select(this.GetTestClass()).ToList();

            result.Should().NotContain(_ => _.IsPrivate);
        }

        [Fact]
        public void SkipsProtectedMethods()
        {
            IEnumerable<MethodDefinition> result = this.testee.Select(this.GetTestClass()).ToList();

            result.Should().NotContain(_ => _.IsFamily);
        }

        [Fact]
        public void SkipsConstructors()
        {
            IEnumerable<MethodDefinition> result = this.testee.Select(this.GetTestClass()).ToList();

            result.Should().NotContain(_ => _.IsConstructor);
        }

        [Fact]
        public void SkipsVirtualMethods()
        {
            IEnumerable<MethodDefinition> result = this.testee.Select(this.GetTestClass()).ToList();

            result.Should().NotContain(_ => _.IsVirtual && _.IsNewSlot);
        }

        [Fact]
        public void SkipsStaticMethods()
        {
            IEnumerable<MethodDefinition> result = this.testee.Select(this.GetTestClass()).ToList();

            result.Should().NotContain(_ => _.IsStatic);
        }

        private TypeDefinition GetTestClass()
        {
            return this.moduleDefinition.GetTypes().Single(_ => _.FullName == "AssemblyToProcess.ClassWithMembers");
        }
    }
}