using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Core.Replacing;

namespace QA.Core.Tests.Stubs
{
    public class CultureDependentModel
    {
        [DependentValue("TitleInv", "ru-ru")]
        public string Title { get; set; }

        [CultureDependent]
        public string TitleInv { get; set; }

        [DependentValue("TitleInv", "en-us")]
        public string TitleEng { get; set; }

        [DependentValue("TitleInv", "fr-fr")]
        public string TitleFr { get; set; }

        [CultureDependent]
        [DependentValue("Cost", "ru-ru")]
        public int Cost { get; set; }

        [DependentValue("Cost", "en-us")]
        public int CostInDollars { get; set; }
    }

    public class CultureDependentComplexModel : CultureDependentModel
    {       
        [CultureDependentMember]
        public CultureDependentComplexObject Child { get; set; }

        [CultureDependentMember]
        public CultureDependentComplexObject[] Children { get; set; }

        [CultureDependentMember]
        public List<CultureDependentComplexObject> Items { get; set; }
    }

    public class CultureDependentObject : ICultureDependent
    {
        [DependentValue("TitleInv", "ru-ru")]
        public string Title { get; set; }

        [CultureDependent]
        public string TitleInv { get; set; }

        [DependentValue("TitleInv", "en-us")]
        public string TitleEng { get; set; }

        [DependentValue("TitleInv", "fr-fr")]
        public string TitleFr { get; set; }

        [CultureDependent]
        [DependentValue("Cost", "ru-ru")]
        public int Cost { get; set; }

        [DependentValue("Cost", "en-us")]
        public int CostInDollars { get; set; }
    }

    public class CultureDependentComplexObject : CultureDependentObject, ICultureDependent
    {        
        [CultureDependentMember]
        public CultureDependentComplexModel MyProperty { get; set; }
    }

}
