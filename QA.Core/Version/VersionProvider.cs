// Owners: Karlov Nikolay, Abretov Alexey

using System;

namespace QA.Core
{
    /// <summary>
    /// Провайдер текущей версии приложения
    /// </summary>
    public static class VersionProvider
    {
        private static Func<string> _versionProvider;

        /// <summary>
        /// Установить
        /// </summary>
        /// <param name="versionProvider"></param>
        public static void SetVersionProvider(Func<string> versionProvider)
        {
            _versionProvider = versionProvider;
        }

        /// <summary>
        /// Получить текущую версию
        /// </summary>
        public static string CurrentVersion
        {
            get
            {
                if (_versionProvider != null)
                {
                    return _versionProvider.Invoke();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
