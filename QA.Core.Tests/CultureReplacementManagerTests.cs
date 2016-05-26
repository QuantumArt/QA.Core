using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Core.Replacing;
using QA.Core.Tests.Stubs;

namespace QA.Core.Tests
{
    [TestClass]
    public class CultureReplacementManagerTests
    {
        [TestCategory("CultureReplacement: tests")]
        [TestMethod]
        public void Test_ReplacementProcessor()
        {
            var manager = new ReplacementProcessor(typeof(CultureDependentModel));

            Func<CultureDependentModel> factory = () => new CultureDependentModel
            {
                Title = "russian",
                TitleEng = "english",
                TitleFr = "francais",
                TitleInv = null,
                Cost = 31,
                CostInDollars = 1
            };

            var model = factory();
            manager.Process(model, "ru-ru");
            Assert.AreEqual(model.Title, model.TitleInv);
            Assert.AreEqual(model.Cost, model.Cost);

            model = factory();
            manager.Process(model, "en-us");
            Assert.AreEqual(model.TitleEng, model.TitleInv);
            Assert.AreEqual(model.CostInDollars, model.Cost);

            model = factory();
            manager.Process(model, "fr-fr");
            Assert.AreEqual(model.TitleFr, model.TitleInv);

            model = factory();
            manager.Process(model, "lt-lt");
            Assert.AreEqual(null, model.TitleInv);
        }


        [TestCategory("CultureReplacement: tests")]
        [TestMethod]
        public void Test_ReplacementProcessor_Of_T()
        {
            var manager = new ReplacementProcessor<CultureDependentModel>();

            Func<CultureDependentModel> factory = () => new CultureDependentModel
            {
                Title = "russian",
                TitleEng = "english",
                TitleFr = "francais",
                TitleInv = null,
                Cost = 31,
                CostInDollars = 1
            };

            var model = factory();
            manager.Process(model, "ru-ru");
            Assert.AreEqual(model.Title, model.TitleInv);
            Assert.AreEqual(model.Cost, model.Cost);

            model = factory();
            manager.Process(model, "en-us");
            Assert.AreEqual(model.TitleEng, model.TitleInv);
            Assert.AreEqual(model.CostInDollars, model.Cost);

            model = factory();
            manager.Process(model, "fr-fr");
            Assert.AreEqual(model.TitleFr, model.TitleInv);

            model = factory();
            manager.Process(model, "lt-lt");
            Assert.AreEqual(null, model.TitleInv);
        }

        [TestCategory("CultureReplacement: tests")]
        [TestMethod]
        public void Test_CultureReplacementExtension()
        {
            var manager = new ReplacementProcessor(typeof(CultureDependentModel));

            Func<CultureDependentObject> factory = () => new CultureDependentObject
            {
                Title = "russian",
                TitleEng = "english",
                TitleFr = "francais",
                TitleInv = null,
                Cost = 31,
                CostInDollars = 1
            };

            var model = factory();

            model.ProcessCultureReplacement("ru-ru");

            Assert.AreEqual(model.Title, model.TitleInv);
            Assert.AreEqual(model.Cost, model.Cost);

            model = factory();
            model.ProcessCultureReplacement("en-us");
            Assert.AreEqual(model.TitleEng, model.TitleInv);
            Assert.AreEqual(model.CostInDollars, model.Cost);

            model = factory();
            model.ProcessCultureReplacement("fr-fr");
            Assert.AreEqual(model.TitleFr, model.TitleInv);

            model = factory();
            model.ProcessCultureReplacement("lt-lt");
            Assert.AreEqual(null, model.TitleInv);
        }

        #region Benchmarks
        const int LoopCount = 100000;
        [TestCategory("CultureReplacement: benchmarks")]
        [TestMethod]
        public void Test_ReplacementProcessor_Benchmark()
        {
            var manager = new ReplacementProcessor(typeof(CultureDependentModel));

            Func<CultureDependentModel> factory = () => new CultureDependentModel
            {
                Title = "russian",
                TitleEng = "english",
                TitleFr = "francais",
                TitleInv = null,
                Cost = 31,
                CostInDollars = 1
            };

            var model = factory();

            DoStuff(model, manager);
        }
        [TestCategory("CultureReplacement: benchmarks")]
        [TestMethod]
        public void Test_ReplacementProcessor_Benchmark_swap()
        {
            var manager = new SwapReplacementProcessor(typeof(CultureDependentModel));

            Func<CultureDependentModel> factory = () => new CultureDependentModel
            {
                Title = "russian",
                TitleEng = "english",
                TitleFr = "francais",
                TitleInv = null,
                Cost = 31,
                CostInDollars = 1
            };

            var model = factory();

            DoStuff(model, manager);
        }

        [TestCategory("CultureReplacement: benchmarks")]
        [TestMethod]
        public void Test_CultureReplacementExtension_Benchmark()
        {
            Func<CultureDependentObject> factory = () => new CultureDependentObject
            {
                Title = "russian",
                TitleEng = "english",
                TitleFr = "francais",
                TitleInv = null,
                Cost = 31,
                CostInDollars = 1
            };

            var model = factory();

            DoStuff(model);
        }


        [TestCategory("CultureReplacement: benchmarks")]
        [TestMethod]
        public void Test_ReplacementProcessor_Of_T_Benchmark_Very_Complex_Object()
        {
            Func<CultureDependentComplexModel> factory = () => new CultureDependentComplexModel
            {
                Title = "russian",
                TitleEng = "english",
                TitleFr = "francais",
                TitleInv = null,
                Cost = 31,
                CostInDollars = 1,
                Children = new CultureDependentComplexObject[] 
                {
                    new CultureDependentComplexObject
                    {
                        Title = "12ru-ru",
                        TitleEng = "en-us3",
                        MyProperty = new CultureDependentComplexModel 
                        {
                            Title = "russian123",
                            Children = new CultureDependentComplexObject[] 
                            {
                                new CultureDependentComplexObject
                                {
                                    Title = "12ru-ru",
                                    TitleEng = "en-us3",
                                    MyProperty = new CultureDependentComplexModel 
                                    {
                                        Title = "rusqwsian123",                                          
                                    }
                                },
                                new CultureDependentComplexObject
                                {
                                    Title = "233ru-ru",
                                    TitleEng = "en-us1",
                                    MyProperty = new CultureDependentComplexModel 
                                    {
                                        Title = "ruswwwsian1233",
                                    }
                                },
                            } 
                        }
                    },
                    new CultureDependentComplexObject
                    {
                        Title = "233ru-ru",
                        TitleEng = "en-us1",
                        MyProperty = new CultureDependentComplexModel 
                        {
                            Title = "russian1233",
                        }
                    },
                }
            };

            var model = factory();
            var manager = new ReplacementProcessor<CultureDependentComplexModel>();

            DoStuff(model, manager);
        }

        [TestCategory("CultureReplacement: benchmarks")]
        [TestMethod]
        public void Test_ReplacementProcessor_Of_T_Benchmark_Very_Complex_Object_swap()
        {
            Func<CultureDependentComplexModel> factory = () => new CultureDependentComplexModel
            {
                Title = "russian",
                TitleEng = "english",
                TitleFr = "francais",
                TitleInv = null,
                Cost = 31,
                CostInDollars = 1,
                Children = new CultureDependentComplexObject[] 
                {
                    new CultureDependentComplexObject
                    {
                        Title = "12ru-ru",
                        TitleEng = "en-us3",
                        MyProperty = new CultureDependentComplexModel 
                        {
                            Title = "russian123",
                            Children = new CultureDependentComplexObject[] 
                            {
                                new CultureDependentComplexObject
                                {
                                    Title = "12ru-ru",
                                    TitleEng = "en-us3",
                                    MyProperty = new CultureDependentComplexModel 
                                    {
                                        Title = "rusqwsian123",                                          
                                    }
                                },
                                new CultureDependentComplexObject
                                {
                                    Title = "233ru-ru",
                                    TitleEng = "en-us1",
                                    MyProperty = new CultureDependentComplexModel 
                                    {
                                        Title = "ruswwwsian1233",
                                    }
                                },
                            } 
                        }
                    },
                    new CultureDependentComplexObject
                    {
                        Title = "233ru-ru",
                        TitleEng = "en-us1",
                        MyProperty = new CultureDependentComplexModel 
                        {
                            Title = "russian1233",
                        }
                    },
                }
            };

            var model = factory();
            var manager = new SwapReplacementProcessor(typeof(CultureDependentComplexModel));

            DoStuff(model, manager);
        }


        [TestCategory("CultureReplacement: benchmarks")]
        [TestMethod]
        public void Test_ReplacementProcessor_Of_T_Benchmark()
        {
            var manager = new ReplacementProcessor<CultureDependentModel>();

            Func<CultureDependentModel> factory = () => new CultureDependentModel
            {
                Title = "russian",
                TitleEng = "english",
                TitleFr = "francais",
                TitleInv = null,
                Cost = 31,
                CostInDollars = 1
            };

            var model = factory();

            DoStuff(model, manager);
        }


        private static void DoStuff(object model, IReplacementProcessor manager)
        {
            var st = new Stopwatch();
            st.Start();
            for (int i = 0; i < LoopCount; i++)
            {
                manager.Process(model, "ru-ru");
            }
            st.Stop();
            Console.WriteLine(string.Format("Made {0} replacements in {1} ms", LoopCount, st.ElapsedMilliseconds));
        }

        private static void DoStuff(CultureDependentObject model)
        {
            var st = new Stopwatch();
            st.Start();
            for (int i = 0; i < LoopCount; i++)
            {
                model.ProcessCultureReplacement("ru-ru");
            }
            st.Stop();
            Console.WriteLine(string.Format("Made {0} replacements in {1} ms", LoopCount, st.ElapsedMilliseconds));
        } 
        #endregion
    }
}
