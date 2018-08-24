using System.ComponentModel;
#pragma warning disable 1591

namespace QA.Core.Web.Qp
{
    public enum QpLanguage : byte
    {
        [Description("ru-RU")]
        Default = 0,

        [Description("en-US")]
        English = 1,

        [Description("ru-RU")]
        Russian = 2
    }
}
