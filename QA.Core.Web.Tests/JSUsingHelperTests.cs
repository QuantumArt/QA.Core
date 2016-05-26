//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using QA.Configuration;
//using QA.Core.Engine;

//namespace QA.Core.Web.Tests
//{
//    [TestClass]
//    public class JSUsingHelperTests
//    {
//        [TestMethod]
//        public void GetScriptTest()
//        {
//            string result = JSUsingHelper.GetScript(typeof(GlobalTest));

//            Assert.AreEqual(
//                "var Global = { ErrorMessages: { InternalErrorMessage: \"Внутренняя ошибка.\",UnknowErrorMessage: \"Неизвестная ошибка.\", }, };",
//                result);

//            result = JSUsingHelper.GetScript(typeof(GlobalTest1));

//            Assert.AreEqual(
//                "var Global = { ErrorMessages: { InternalErrorMessage: \"Внутренняя ошибка.\",UnknowErrorMessage: \"Неизвестная ошибка.\",Inner: { InternalErrorMessage: \"Внутренняя ошибка.\",UnknowErrorMessage: \"Неизвестная ошибка.\", }, }, };",
//                result);

//            result = JSUsingHelper.GetScript(typeof(ItemStateTest));

//            Assert.AreEqual("var Enums = { ItemState: { Created: \"Created\",Published: \"Published\",Approved: \"Approved\",None: \"None\", }, };",
//                result);
//        }
//    }

//    public partial class GlobalTest
//    {
//        #region ErrorMessages
//        [JSUsing("Global", null)]
//        public partial class ErrorMessages
//        {
//            /// <summary>
//            /// Внутренняя ошибка.
//            /// </summary>
//            [JSUsing("ErrorMessages", null)]
//            public const string InternalErrorMessage = @"Внутренняя ошибка.";
//            /// <summary>
//            /// Неизвестная ошибка.
//            /// </summary>
//            [JSUsing("ErrorMessages", null)]
//            public const string UnknowErrorMessage = @"Неизвестная ошибка.";
//        }
//        #endregion
//    }

//    [JSUsing("Enums", null)]
//    public enum ItemStateTest
//    {
//        /// <summary>
//        /// Создан
//        /// </summary>
//        [System.ComponentModel.Description("Created")]
//        [JSUsing("ItemState", null)]
//        Created,

//        /// <summary>
//        /// Опубликован
//        /// </summary>
//        [System.ComponentModel.Description("Published")]
//        [JSUsing("ItemState", null)]
//        Published,

//        /// <summary>
//        /// Подтверждено
//        /// </summary>
//        [System.ComponentModel.Description("Approved")]
//        [JSUsing("ItemState", null)]
//        Approved,

//        /// <summary>
//        /// Без статуса
//        /// </summary>
//        [System.ComponentModel.Description("None")]
//        [JSUsing("ItemState", null)]
//        None
//    }

//    public partial class GlobalTest1
//    {
//        #region ErrorMessages
//        [JSUsing("Global", null)]
//        public partial class ErrorMessages
//        {
//            /// <summary>
//            /// Внутренняя ошибка.
//            /// </summary>
//            [JSUsing("ErrorMessages", null)]
//            public const string InternalErrorMessage = @"Внутренняя ошибка.";
//            /// <summary>
//            /// Неизвестная ошибка.
//            /// </summary>
//            [JSUsing("ErrorMessages", null)]
//            public const string UnknowErrorMessage = @"Неизвестная ошибка.";

//            #region ErrorMessages
//            [JSUsing("ErrorMessages", null)]
//            public partial class ErrorMessages1
//            {
//                /// <summary>
//                /// Внутренняя ошибка.
//                /// </summary>
//                [JSUsing("Inner", null)]
//                public const string InternalErrorMessage = @"Внутренняя ошибка.";
//                /// <summary>
//                /// Неизвестная ошибка.
//                /// </summary>
//                [JSUsing("Inner", null)]
//                public const string UnknowErrorMessage = @"Неизвестная ошибка.";
//            }
//            #endregion
//        }
//        #endregion
//    }
//}
