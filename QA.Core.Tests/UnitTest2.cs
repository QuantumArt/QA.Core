using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Core.Logger;

namespace QA.Core.Tests
{
    [TestClass]
    public class UnitTest2
    {
        public UnitTest2()
        {
            Logger = new TextWriterLogger(Console.Out);
        }

        [TestMethod]
        public void ObjectDumper_Samples()
        {
            int age = 12;
            string name = "ivan";
            IEnumerable<string> items = new string[] { "apple", "onion" };

            var str1 = ObjectDumper.DumpObject(new
            {
                age,
                name,
                items
            });

            var str2 = ObjectDumper.DumpObject(new
            {
                Age = age,
                Message ="sadxasxas asdxc asd asd asd "
            });

            Logger.Info(str1);
            Logger.Info(str2);


        }

        [TestMethod]
        public void ObjectDumper_Lazylogging_sample()
        {
            int age = 12;
            string name = "ivan";
            IEnumerable<string> items = new string[] { "apple", "onion" };

            // вэтом случае значение будет вычисляться только в том случае, если включен уровень журналирования Info
            Logger.Info(() => ObjectDumper.DumpObject(new
            {
                age,
                name,
                items
            }));

        }

        public  ILogger Logger { get; private set; }
    }
}
