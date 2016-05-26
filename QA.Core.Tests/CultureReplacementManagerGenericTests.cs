using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Core.Replacing;
using QA.Core.Tests.Stubs;

namespace QA.Core.Tests
{
    [TestClass]
    public abstract class ReplacementManagerTests<T>
        where T : IReplacementProcessor
    {
        protected abstract T GetProcessor();

        [TestCategory("CultureReplacement: tests")]
        [TestMethod]
        public void Test_ReplacementProcessor_Abstract()
        {
            var manager = GetProcessor();

            Func<CultureDependentComplexModel> factory = () => new CultureDependentComplexModel
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
        public void ReplacementProcessor_ChildObject()
        {
            var manager = GetProcessor();

            Func<CultureDependentComplexModel> factory = () => new CultureDependentComplexModel
            {
                Title = "russian",
                TitleEng = "english",
                TitleFr = "francais",
                TitleInv = null,
                Cost = 31,
                CostInDollars = 1,
                Child = new CultureDependentComplexObject
                {
                    Title = "ru-ru",
                    TitleEng = "en-us"
                }
            };

            var model = factory();
            manager.Process(model, "ru-ru");
            Assert.AreEqual(model.Title, model.TitleInv, GetMessage());
            Assert.AreEqual(model.Child.Title, model.Child.TitleInv, GetMessage());
        }

        [TestCategory("CultureReplacement: tests")]
        [TestMethod]
        public void ReplacementProcessor_ChildObject_With_Nested()
        {
            var manager = GetProcessor();

            Func<CultureDependentComplexModel> factory = () => new CultureDependentComplexModel
            {
                Title = "russian",
                Child = new CultureDependentComplexObject
                {
                    Title = "ru-ru",
                    TitleEng = "en-us",
                    MyProperty = new CultureDependentComplexModel
                    {
                        Title = "russian123",
                    }
                }
            };

            var model = factory();
            manager.Process(model, "ru-ru");
            Assert.AreEqual(model.Title, model.TitleInv, GetMessage());
            Assert.AreEqual(model.Child.Title, model.Child.TitleInv, GetMessage());
            Assert.AreEqual(model.Child.MyProperty.Title, model.Child.MyProperty.TitleInv, GetMessage());
        }

        [TestCategory("CultureReplacement: tests")]
        [TestMethod]
        public void ReplacementProcessor_Array_Of_Model()
        {
            var manager = GetProcessor();

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
                    },
                    new CultureDependentComplexObject
                    {
                        Title = "233ru-ru",
                        TitleEng = "en-us1",
                    },
                }
            };

            var model = factory();
            manager.Process(model, "ru-ru");
            Assert.AreEqual(model.Title, model.TitleInv, GetMessage());
            Assert.AreEqual(model.Children[0].Title, model.Children[0].TitleInv, GetMessage());
            Assert.AreEqual(model.Children[1].Title, model.Children[1].TitleInv, GetMessage());
        }

        [TestCategory("CultureReplacement: tests")]
        [TestMethod]
        public void ReplacementProcessor_Array_Of_Model_With_Nested()
        {
            var manager = GetProcessor();

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
            manager.Process(model, "ru-ru");
            Assert.AreEqual(model.Title, model.TitleInv);
            Assert.AreEqual(model.Children[0].Title, model.Children[0].TitleInv);
            Assert.AreEqual(model.Children[0].MyProperty.Title, model.Children[0].MyProperty.TitleInv);
            Assert.AreEqual(model.Children[1].Title, model.Children[1].TitleInv);
            Assert.AreEqual(model.Children[1].MyProperty.Title, model.Children[1].MyProperty.TitleInv);
        }

        [TestCategory("CultureReplacement: tests")]
        [TestMethod]
        public void ReplacementProcessor_Array_Of_Model_With_Nested_Collections()
        {
            var manager = GetProcessor();

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
            manager.Process(model, "ru-ru");
            Assert.AreEqual(model.Title, model.TitleInv);
            Assert.AreEqual(model.Children[0].Title, model.Children[0].TitleInv);
            Assert.AreEqual(model.Children[0].MyProperty.Title, model.Children[0].MyProperty.TitleInv);

            Assert.AreEqual(model.Children[0].MyProperty.Children[0].Title, model.Children[0].MyProperty.Children[0].TitleInv);
            Assert.AreEqual(model.Children[0].MyProperty.Children[0].MyProperty.Title, model.Children[0].MyProperty.Children[0].MyProperty.TitleInv);
            Assert.AreEqual(model.Children[0].MyProperty.Children[1].Title, model.Children[0].MyProperty.Children[1].TitleInv);
            Assert.AreEqual(model.Children[0].MyProperty.Children[1].MyProperty.Title, model.Children[0].MyProperty.Children[1].MyProperty.TitleInv);

            Assert.AreEqual(model.Children[1].Title, model.Children[1].TitleInv);
            Assert.AreEqual(model.Children[1].MyProperty.Title, model.Children[1].MyProperty.TitleInv);
        }

        [TestCategory("CultureReplacement: tests")]
        [TestMethod]
        public void ReplacementProcessor_List_Of_Model()
        {
            var manager = GetProcessor();

            Func<CultureDependentComplexModel> factory = () => new CultureDependentComplexModel
            {
                Title = "russian",
                TitleEng = "english",
                TitleFr = "francais",
                TitleInv = null,
                Cost = 31,
                CostInDollars = 1,
                Items = new List<CultureDependentComplexObject> 
                {
                    new CultureDependentComplexObject
                    {
                        Title = "ru-ru222",
                        TitleEng = "en-us123"
                    },
                    new CultureDependentComplexObject
                    {
                        Title = "ru-ru222",
                        TitleEng = "en-us123"
                    },
                }
            };

            var model = factory();
            manager.Process(model, "ru-ru");
            Assert.AreEqual(model.Title, model.TitleInv);
            Assert.AreEqual(model.Items[0].Title, model.Items[0].TitleInv);
            Assert.AreEqual(model.Items[1].Title, model.Items[1].TitleInv);
        }

        private string GetMessage([CallerMemberName]string caller = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
        {
            return string.Format("\nFailed test '{0}' is inherited by '{1}'. \nTest is defined in {2} [{3}]", caller, this, path, line);
        }
    }
       
    [TestClass]
    public class SimpleProcessor : ReplacementManagerTests<ReplacementProcessor>
    {
        protected override ReplacementProcessor GetProcessor()
        {
            return new ReplacementProcessor(typeof(CultureDependentComplexModel));
        }

    }
    [TestClass]
    public class ComplexProcessor : ReplacementManagerTests<SwapReplacementProcessor>
    {
        protected override SwapReplacementProcessor GetProcessor()
        {
            return new SwapReplacementProcessor(typeof(CultureDependentComplexModel));
        }

    }
}
