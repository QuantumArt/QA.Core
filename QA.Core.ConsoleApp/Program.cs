using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using QA.Core.Replacing;
using QA.Core.Tests.Stubs;

namespace QA.Core.ConsoleApp
{
    class Program
    {
        const int LoopCount = 10000000;

        static void Main(string[] args)
        {
            //var manager = new ReplacementProcessor(typeof(CultureDependentModel));
            Console.WriteLine(CultureInfo.CurrentUICulture.Name);
            Func<CultureDependentModel> factory = () => new CultureDependentModel
            {
                Title = "russian",
                TitleEng = "english",
                TitleFr = "francais",
                TitleInv = null,
                Cost = 31,
                CostInDollars = 1,
            };

            var model = factory();

            DoStuff(model, new ReplacementProcessor<CultureDependentModel>());
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

    }
}
