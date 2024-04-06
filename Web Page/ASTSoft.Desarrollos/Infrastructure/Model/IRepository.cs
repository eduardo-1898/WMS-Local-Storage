using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Model
{
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void AddRange(TEntity[] entities);
        void AddRange(IEnumerable<TEntity> entities);
        void Edit(TEntity entity);
        void Remove(TEntity entity);
        void Remove(Func<TEntity, bool> predicate);
    }
}
