using log4net;
using Plank.Core.Entities;

namespace Plank.Core.Services
{
    public sealed class PlankLogger<TEntity> : ILogger
        where TEntity : IEntity
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(TEntity));

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