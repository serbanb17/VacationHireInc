// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;

namespace VacationHireInc.WebApi.Interfaces
{
    public interface IObjectCache<T>
    {
        bool TryGet(string key, out T obj);
        bool Add(string key, DateTime expiration, T obj);
    }
}
