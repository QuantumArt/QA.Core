using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web;

namespace QA.Core.Web
{
    /// <summary>
    /// Преобразования имени файла контента (css, js) и версионирование
    /// </summary>
    public static class ContentMinification
    {
        // паттерн для замены
        private static Regex _scriptPattern = new Regex(@"\.js$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex _styleSheetPattern = new Regex(@"\.css$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        #region Конфигурирование
        /// <summary>
        /// Ключ для конфигурационного файла.
        /// </summary>
        public const string UseMinifiedContentConfigurationKey = "ContentMinification.Enabled";

        /// <summary>
        /// Ключ для конфигурационного файла.
        /// </summary>
        public const string MinifyScriptNamesConfigurationKey = "ContentMinification.IgnoreScripts";

        /// <summary>
        /// Ключ для конфигурационного файла.
        /// </summary>
        public const string MinifyCSSNamesConfigurationKey = "ContentMinification.IgnoreCss";

        /// <summary>
        /// Ключ для конфигурационного файла.
        /// </summary>
        public const string UseCustomVersionConfigurationKey = "ContentMinification.UseCustomVersion";

        /// <summary>
        /// Ключ для конфигурационного файла.
        /// </summary>
        public const string CustomVersionConfigurationKey = "ContentMinification.CustomVersion";

        /// <summary>
        /// Ключ для конфигурационного файла.
        /// </summary>
        public const string UseVersioningConfigurationKey = "ContentMinification.UseVersioning";

        /// <summary>
        /// Настройка преобразователя имен java-скриптов.
        /// </summary>
        public static bool ShouldMinify { get; set; }

        public static bool IgnoreScripts { get; set; }

        public static bool IgnoreCSS { get; set; }

        /// <summary>
        /// Использовать или нет стандартный механизм версионирования
        /// </summary>
        public static bool UseCustomVersion { get; set; }

        /// <summary>
        /// Текущая версия
        /// </summary>
        public static string CustomVersion { get; set; }

        /// <summary>
        /// Версионирование
        /// </summary>
        public static bool UseVersioning { get; set; }

        /// <summary>
        /// Берем информацию из конфигурационного файла. 
        /// <remarks></remarks>
        /// </summary>
        public static void Configure()
        {
            bool result = false;
            if (bool.TryParse(ConfigurationManager
                .AppSettings[ContentMinification.UseMinifiedContentConfigurationKey] ?? string.Empty,
                out result))
            {
                ShouldMinify = result;
            }

            if (bool.TryParse(ConfigurationManager
                .AppSettings[ContentMinification.MinifyScriptNamesConfigurationKey] ?? string.Empty,
                out result))
            {
                IgnoreScripts = result;
            }

            if (bool.TryParse(ConfigurationManager
                .AppSettings[ContentMinification.MinifyCSSNamesConfigurationKey] ?? string.Empty,
               out result))
            {
                IgnoreCSS = result;
            }

            if (bool.TryParse(ConfigurationManager
               .AppSettings[ContentMinification.UseCustomVersionConfigurationKey] ?? string.Empty,
              out result))
            {
                UseCustomVersion = result;
            }

            if (bool.TryParse(ConfigurationManager
              .AppSettings[ContentMinification.UseVersioningConfigurationKey] ?? string.Empty,
             out result))
            {
                UseVersioning = result;
            }

            if (UseCustomVersion)
            {
                CustomVersion = (string)ConfigurationManager.AppSettings[ContentMinification.CustomVersionConfigurationKey];
            }
        }
        #endregion

        /// <summary>
        /// Преобразование имени файла контента. Например, '*.js' -> '*.min.js'. 
        /// </summary>
        /// <param name="forceMinify">Преобразовать имя принудительно</param>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetMinified(this string path, bool isDebug)
        {
            // Предобразование url вида:
            // /js/core/qa.validation.js -> /js/core/qa.validation.min.js
            // /content/core/style.css -> /content/core/style.min.css
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            string result = path;
            
            if (ShouldMinify)
            {
                if (!IgnoreScripts)
                {
                    result = _scriptPattern.Replace(result, x => string.Format(".min{0}", x.Value));
                }

                if (!IgnoreCSS)
                {
                    result = _styleSheetPattern.Replace(result, x => string.Format(".min{0}", x.Value));
                } 
            }

            return result;
        }

        /// <summary>
        /// Конвертация относительного (виртуального) пути к контенту с преобразованием минифицирования
        /// </summary>
        /// <param name="path">The virtual path of the content</param>
        /// <returns></returns>
        public static string MinifiedContent(this UrlHelper urlHelper, string path)
        {
            var url = urlHelper.Content(path.GetMinified(urlHelper.RequestContext.HttpContext.IsDebuggingEnabled));

            if (UseVersioning)
            {
                // версионирование
                var version = CustomVersion ?? VersionProvider.CurrentVersion;
                if (urlHelper.RequestContext.HttpContext.IsDebuggingEnabled)
                {
                    // версионирование вида
                    // /content/styles/site.css?version=123456789
                    if (!string.IsNullOrEmpty(version))
                    {
                        var separator = (url.Contains("?") ? "&" : "?");

                        url = string.Format("{0}{2}version={1}", url, version, separator);
                    }
                }
                else
                {
                    // версионирование вида
                    // /versioned/123456789/content/styles/site.css
                    if (!string.IsNullOrEmpty(version))
                    {
                        url = string.Format("{0}versioned/{1}{2}{3}",
                            (url.StartsWith("/") ? "/" : ""), version, (url.StartsWith("/") ? "" : "/"), url);
                    }
                }
            }

            return url;
        }

        /// <summary>
        /// Получение абсолютного пути
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Uri GetBaseUrl(this UrlHelper url)
        {
            Uri contextUri = new Uri(url.RequestContext.HttpContext.Request.Url, url.RequestContext.HttpContext.Request.RawUrl);
            UriBuilder realmUri = new UriBuilder(contextUri) { Path = url.RequestContext.HttpContext.Request.ApplicationPath, Query = null, Fragment = null };
            return realmUri.Uri;
        }

        /// <summary>
        /// Получение абсолютного пути
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Uri GetBaseUrl(HttpRequestBase request)
        {
            Uri contextUri = new Uri(request.Url, request.RawUrl);
            UriBuilder realmUri = new UriBuilder(contextUri) { Path = request.ApplicationPath, Query = null, Fragment = null };
            return realmUri.Uri;
        }
        /// <summary>
        /// Получение абсолютного пути к действию контроллера
        /// </summary>
        /// <param name="url"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static string ActionAbsolute(this UrlHelper url, string actionName, string controllerName)
        {
            return new Uri(GetBaseUrl(url), url.Action(actionName, controllerName)).AbsoluteUri;
        }

        /// <summary>
        /// Получение абсолютного пути к контенту
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ContentAbsolute(this UrlHelper urlHelper, string path)
        {
            return new Uri(GetBaseUrl(urlHelper), urlHelper.Content(path)).AbsoluteUri;
        }

        /// <summary>
        /// Получение абсолютного пути к контенту (с преобразоваением)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ContentMinifiedAbsolute(this UrlHelper urlHelper, string path)
        {
            return new Uri(GetBaseUrl(urlHelper), urlHelper.MinifiedContent(path)).AbsoluteUri;
        }
    }
}
