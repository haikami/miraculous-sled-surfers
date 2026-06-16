using System;

namespace SledSurfers.Scripts.Events
{
    public interface IChannel<T>
    {
        event Action<T> OnRaised;
    }
    
    public interface IChannel
    {
        event Action OnRaised;
    }

    public class Channel : IChannel
    {
        public event Action OnRaised;
        public void Raise() => OnRaised?.Invoke();
    }
    
    public class Channel<T> : IChannel<T>
    {
        public event Action<T> OnRaised;
        public void Raise(T payload) => OnRaised?.Invoke(payload);
    }
}