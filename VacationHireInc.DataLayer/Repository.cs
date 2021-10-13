// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VacationHireInc.DataLayer.Interfaces;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.DataLayer
{
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        private AppDbContext _appDbContext;

        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public T Get(Guid id)
        {
            return _appDbContext.Set<T>().Find(id);
        }

        public int GetCount()
        {
            return _appDbContext.Set<T>().Count();
        }

        public IEnumerable<T> GetPage(int pageId, int pageSize)
        {
            if (pageSize == 0)
                return new List<T>();
            return _appDbContext.Set<T>().OrderBy(x => x.Id).Skip(pageId * pageSize).Take(pageSize).ToList();
        }

        public IEnumerable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _appDbContext.Set<T>().Where(expression).ToList();
        }

        public void Create(T entity)
        {
            entity.Id = Guid.NewGuid();
            _appDbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _appDbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            var entityLocal = Get(entity.Id);
            _appDbContext.Set<T>().Remove(entityLocal);
        }
    }
}
