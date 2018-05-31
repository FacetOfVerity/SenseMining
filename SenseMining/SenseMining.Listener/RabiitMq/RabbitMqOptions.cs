namespace SenseMining.Listener.RabiitMq
{
    /// <summary>
    /// Настройки RabbitMQ
    /// </summary>
    public class RabbitMqOptions
    {
        /// <summary>
        /// Имя обменника
        /// </summary>
        public string ExchangeName { get; set; }
        /// <summary>
        /// Адрес хоста RabbitMQ
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// Имя очереди
        /// </summary>
        public string QueueName { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}
