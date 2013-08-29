namespace EnableFaking.Fody
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FluentAssertions;
    using Mono.Cecil;
    using Xunit;

    public class MemberVirtualizerTest
    {
        private readonly MemberVirtualizer testee;
        private readonly ModuleDefinition moduleDefinition;

        public MemberVirtualizerTest()
        {
            string assemblyPath = Assembly.GetExecutingAssembly().CodeBase
                .Replace("file:///", string.Empty)
                .Replace("EnableFaking.Fody.Test", "AssemblyToProcess");

            this.moduleDefinition = ModuleDefinition.ReadModule(assemblyPath);

            this.testee = new MemberVirtualizer();
        }

        [Fact]
        public void MarksMembersAsVirtual()
        {
            IEnumerable<MethodDefinition> members = this.GetMembersToVirtualize().ToList();

            this.testee.Virtualize(members);

            members.Should().OnlyContain(_ => _.IsVirtual);
        }

        [Fact]
        public void MarksMembersAsNewSlot()
        {
            IEnumerable<MethodDefinition> members = this.GetMembersToVirtualize().ToList();

            this.testee.Virtualize(members);

            members.Should().OnlyContain(_ => _.IsNewSlot);
        }

        private IEnumerable<MethodDefinition> GetMembersToVirtualize()
        {
            return this.moduleDefinition.GetTypes().Single(_ => _.FullName == "AssemblyToProcess.ClassToVirtualizeMembers").Methods;
        }
    }
}