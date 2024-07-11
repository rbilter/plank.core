﻿namespace Plank.Core.Services
{
    public interface IService<T> : IReadService<T>, IWriteService<T> where T : new()
    {
    }
}