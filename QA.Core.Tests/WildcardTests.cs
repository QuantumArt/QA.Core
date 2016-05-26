using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QA.Core.Engine.Tests
{
    [TestClass]
    public class WildcardMatcherTests : IWildcardMatcherTestsBase<WildcardMatcher>
    {
        [TestMethod]
        public void WWWMyTestMethod()
        {

        }
        [TestCategory("WildcardMatcher")]
        [TestMethod]
        public override void Test_WildCardMatcher()
        {
            base.Test_WildCardMatcher();
        }
        [TestCategory("WildcardMatcher")]
        [TestMethod]
        public override void Test_WildCardMatcher_Issue01_Incorrect()
        {
            base.Test_WildCardMatcher_Issue01_Incorrect();
        }
        [TestCategory("WildcardMatcher")]
        [TestMethod]
        public override void Test_WildCardMatcher_Issue02_Incorrect_one_letter()
        {
            base.Test_WildCardMatcher_Issue02_Incorrect_one_letter();
        }
        [TestCategory("WildcardMatcher")]
        [TestMethod]
        public override void Test_WildCardMatcher_BatchBench()
        {
            base.Test_WildCardMatcher_BatchBench();
        }
        [TestMethod]
        public override void Test_WildCardMatcher_SeparateBench()
        {
            base.Test_WildCardMatcher_SeparateBench();
        }
    }

    [TestClass]
    public class FastWildcardMatcherTests : IWildcardMatcherTestsBase<FastWildcardMatcher>
    {
        [TestCategory("FastWildcardMatcher")]
        [TestMethod]
        public override void Test_WildCardMatcher()
        {
            base.Test_WildCardMatcher();
        }
        [TestCategory("FastWildcardMatcher")]
        [TestMethod]
        public override void Test_WildCardMatcher_Issue01_Incorrect()
        {
            base.Test_WildCardMatcher_Issue01_Incorrect();
        }
        [TestCategory("FastWildcardMatcher")]
        [TestMethod]
        public override void Test_WildCardMatcher_Issue02_Incorrect_one_letter()
        {
            base.Test_WildCardMatcher_Issue02_Incorrect_one_letter();
        }
        [TestCategory("FastWildcardMatcher")]
        [TestMethod]
        public override void Test_WildCardMatcher_BatchBench()
        {
            base.Test_WildCardMatcher_BatchBench();
        }
        [TestMethod]
        public override void Test_WildCardMatcher_SeparateBench()
        {
            base.Test_WildCardMatcher_SeparateBench();
        }
    }
}
