using A.I.S.A_;

namespace A.I.S.A.Tests
{
    public class CommonAISATests
    {
        [Fact]
        public void TestAISACLI()
        {
            AISA a = new AISA();
            a.GPT("This is a test!");
        }
    }
}