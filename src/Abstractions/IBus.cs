﻿namespace Abstractions
{
    public interface IBus
    {
        void Publish<T>(T message) where T : IEvent;
    }
}
