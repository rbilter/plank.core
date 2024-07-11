using log4net;

namespace Plank.Core.Services
{
    public sealed class PlankLogger<T> : ILogger 
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(T));

        public void ErrorMessage(object message)
        {
            _logger.Error(message);
        }

        public void ErrorMessage(object message, Exception exception)
        {
            _logger.Error(message, exception);
        }

        public void InfoMessage(object message)
        {
            _logger.Info(message);
        }

        public void InfoMessage(object message, Exception exception)
        {
            _logger.Info(message, exception);
        }
    }
}