// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.DataLayer.Interfaces
{
    public interface IRepository<T> where T : BaseModel
    {
        T Get(Guid id);
        int GetCount();

        /// <summary>
        /// Return sublist of the sorted sorted by Id elements
        /// </summary>
        /// <param name="pageId">
        /// Sublist id, 0 indexed
        /// </param>
        /// <param name="pageSize">
        /// Sublist length
        /// </param>
        IEnumerable<T> GetPage(int pageId, int pageSize);

        IEnumerable<T> GetByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
