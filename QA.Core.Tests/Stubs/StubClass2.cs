using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Core.Tests.Stubs
{
    public class StubClass2 : StubClass1
    {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
        public StubClass1 ComplexProperty { get; set; }
        public override string MyProperty3
        {
            get
            {
                return MyProperty3_Backward_Stub;
            }
            set
            {
                MyProperty3_Backward_Stub = value;
            }
        }

        public string MyProperty3_Backward_Stub { get; set; }

        public int? NullableInt{ get; set; }

        internal string Method()
        {
            throw new NotImplementedException();
        }
    }
}
