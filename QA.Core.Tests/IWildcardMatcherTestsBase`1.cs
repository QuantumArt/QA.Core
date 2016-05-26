using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QA.Core.Engine.Tests
{

    public abstract class IWildcardMatcherTestsBase<T>
        where T : class, IWildcardMatcher
    {

        protected IWildcardMatcher Create(WildcardMatchingOption option, params string[] items)
        {
            return (IWildcardMatcher)Activator.CreateInstance(typeof(T), option, items);
        }

        [TestMethod]
        public virtual void Test_WildCardMatcher()
        {
            IWildcardMatcher matcher = Create(WildcardMatchingOption.StartsWith, "bee.ru",
                "*.bee.ru",
                "*.stage.bee.ru",
                "stage.bee.ru",
                "stage.*.ru",
                "*");

            Assert.AreEqual("*.bee.ru", matcher.MatchLongest("msc.bee.ru"));
            Assert.AreEqual("stage.bee.ru", matcher.MatchLongest("stage.bee.ru"));
            Assert.AreEqual("*.bee.ru", matcher.MatchLongest("msc.bee.ru"));
            Assert.AreEqual("stage.*.ru", matcher.MatchLongest("stage.123.ru"));
            Assert.AreEqual("stage.*.ru", matcher.MatchLongest("stage.1232344.ru"));
            Assert.AreEqual("*.bee.ru", matcher.MatchLongest("msc.bee.ru"));
            Assert.AreEqual("*", matcher.MatchLongest("ee.ru"));

            Assert.AreEqual("*.bee.ru", matcher.MatchLongest("moskovskaya-obl.bee.ru"));
        }

        [TestMethod]
        public virtual void Test_WildCardMatcher_BatchBench()
        {
            IWildcardMatcher matcher = Create(WildcardMatchingOption.FullMatch, "bee.ru",
                "*.bee.ru",
                "*.stage.bee.ru",
                "stage.bee.ru",
                "stage.bee.ru",
                "stage.*.ru.*",
                "*");

            for (int i = 0; i < 10000; i++)
            {
                matcher.MatchLongest("stage.123.ru");
            }
        }

        [TestMethod]
        public virtual void Test_WildCardMatcher_SeparateBench()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                IWildcardMatcher matcher = Create(WildcardMatchingOption.FullMatch, "bee.ru" + r.Next(),
               "*.bee.ru",
               "*.stage.b*ee.ru",
               "sta*ge.bee.ru*",
               "*sta*ge.bee.ru",
               "stage.bee.r*u",
               "stage.*.ru",
               "*");

                matcher.MatchLongest("stage.123.ruaefawefawefawe awef awefr awef awef awef awef aef awef awef wef awe .awefd awe.fa.ef.asef");
            }
            sw.Stop();
            Console.WriteLine(new { sw.ElapsedMilliseconds, t = this.GetType().Name });
        }

        [TestMethod]
        public virtual void Test_WildCardMatcher_Issue01_Incorrect()
        {
            IWildcardMatcher matcher = Create(WildcardMatchingOption.FullMatch,
                "bee.ru",
                "*.bee.ru",
                "stage.bee.ru"
            );

            Assert.AreEqual("*.bee.ru", matcher.MatchLongest("msc.bee.ru"));
            Assert.AreEqual("*.bee.ru", matcher.MatchLongest("www.bee.ru"));
            Assert.AreEqual(null, matcher.MatchLongest("bee.ru.artq.com"));

        }


        [TestMethod]
        public virtual void Test_WildCardMatcher_Issue02_Incorrect_one_letter()
        {
            IWildcardMatcher matcher = Create(WildcardMatchingOption.FullMatch,
                "bee.ru",
                "*.bee.ru",
                "f.bee.ru"
            );

            Assert.AreEqual("*.bee.ru", matcher.MatchLongest("msc.bee.ru"));
            Assert.AreEqual("f.bee.ru", matcher.MatchLongest("f.bee.ru"));
            Assert.AreEqual(null, matcher.MatchLongest("bee.ru.artq.com"));

        }
    }
}
