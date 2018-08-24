using System;
#pragma warning disable 1591

namespace QA.Core.Performance
{
    /// <summary>
    /// Конфигурация системы защиты от нестабильной работы сервисов
    /// </summary>
    public class RapidFailConfig
    {
        public RapidFailConfig()
        {
            // значения по умолчанию
            MaxErrorCount = 20;
            TimeInterval = TimeSpan.FromMilliseconds(5000);
            TimeoutThreshold = 1500;
            ThresholdRatio = 0.2;
            MaxActiveRequestsCount = 50;
            MinActiveRequestsCount = 3;
            MinRequestsCount = 10;
            IsEnabled = true;
        }

        /// <summary>
        /// Максимально допустимое кол-во ошибок сервиса за указанный период (TimeInterval).
        /// При достижении этой величины обращения к сервису прекращаются. Пользователь получает отказ. Однако, по прошествии некоторого времени обращения восстанавливаются.
        /// </summary>
        public int MaxErrorCount { get; set; }

        /// <summary>
        /// Период времени, за который просматривается статистика работы сайта.
        /// Статистика обращения, не входящих в этот интервал, не учитывается
        /// </summary>
        public TimeSpan TimeInterval { get; set; }

        /// <summary>
        /// Пороговая величина времени отклика от сервиса.
        /// Эта величина не является таймаутом на подключение! Рекомендуется указывать пороговую величину, составляющую не более трети от таймаута на подключение. Оптимальное значение – 2 секунды
        /// </summary>
        public long TimeoutThreshold { get; set; }

        /// <summary>
        /// Предельно допустимая доля «долгих ответов» (не укладывающихся в TimeoutThreshold) от сервиса за указанный период времени.
        /// Данный параметр учитывает статистику времен ответа от сервиса, и если Кол-во «долгих» ответов превышает данную величину, то обращения к сервису прекращаются.  Период сбора статистики указан выше (TimeInterval).
        /// </summary>
        public double ThresholdRatio { get; set; }

        /// <summary>
        /// Ограничение кол-ва одновременных подключений к сервису.
        /// После превышения этой величины сайт перестает обращаться к сервису, возвращая пользователю отказ
        /// </summary>
        public int MaxActiveRequestsCount { get; set; }

        /// <summary>
        /// Признак того, что система защиты включена
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Минимальное количесиво одновременных транзакций, начиная с которого применяется проверка нагрузки.
        /// Если количество транзакцийменьше указанного, то правила не применяются
        /// </summary>
        public int MinActiveRequestsCount { get; set; }

        /// <summary>
        /// Минимальное количесиво обработанных запросов в указанном интервале, начиная с которого применяется проверка нагрузки.
        /// Если количество меньше указанного, то правила не применяются
        /// </summary>
        public int MinRequestsCount { get; set; }

        /// <summary>
        /// Включить диагностические логирование
        /// </summary>
        public bool Log { get; set; }
    }
}
