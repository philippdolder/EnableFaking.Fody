namespace EnableFaking.Fody
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Cecil;

    public class ModuleWeaver
    {
        private readonly TypeSelector typeSelector;

        private readonly MemberSelector memberSelector;
        private readonly MemberVirtualizer memberVirtualizer;
        private readonly CallMapper callMapper;

        public ModuleWeaver()
            : this(new TypeSelector(), new MemberSelector(), new MemberVirtualizer(), new CallMapper())
        {
            this.LogInfo = m => { };
        }

        public ModuleWeaver(
            TypeSelector typeSelector,
            MemberSelector memberSelector,
            MemberVirtualizer memberVirtualizer,
            CallMapper callMapper)
        {
            this.typeSelector = typeSelector;
            this.memberSelector = memberSelector;
            this.memberVirtualizer = memberVirtualizer;
            this.callMapper = callMapper;
        }

        public ModuleDefinition ModuleDefinition { get; set; }

        public Action<string> LogInfo { get; set; }

        public void Execute()
        {
            IEnumerable<TypeDefinition> selectedTypes = this.typeSelector.Select(this.ModuleDefinition);
            IEnumerable<MethodDefinition> selectedMembers = selectedTypes.SelectMany(type => this.memberSelector.Select(type)).ToList();

            this.memberVirtualizer.Virtualize(selectedMembers);
            this.callMapper.MapCallsToVirtual(selectedMembers, this.ModuleDefinition);
        }
    }
}