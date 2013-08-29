namespace EnableFaking.Fody
{
    using System.Reflection;
    using Xunit;

    public class WeaverTests
    {
        private readonly Assembly assembly;

        public WeaverTests()
        {
            this.assembly = WeaverHelper.WeaveAssembly();
        }

#if(DEBUG)
        [Fact]
        public void PeVerify()
        {
            Verifier.Verify(this.assembly.CodeBase.Remove(0, 8));
        }
#endif
    }
}