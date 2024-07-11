namespace Plank.Core.Services
{
    public interface ILogger
    {
        void ErrorMessage(object message);

        void ErrorMessage(object message, Exception exception);

        void InfoMessage(object message);

        void InfoMessage(object message, Exception exception);
    }
}