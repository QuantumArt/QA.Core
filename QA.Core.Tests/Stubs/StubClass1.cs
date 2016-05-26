// Owners: Karlov Nikolay, Abretov Alexey        

namespace QA.Core.Tests.Stubs
{
    public class StubClass1
    {
        public object MyProperty1 { get; set; }
        public int MyProperty2 { get; set; }
        public StubClass1 Child { get; set; }
        public virtual string MyProperty3 { get; set; }
        public virtual string MyProperty4 { get; set; }
    }
}
