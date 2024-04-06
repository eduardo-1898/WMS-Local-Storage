using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class Repository<TEntity>
        where TEntity : class
    {
        private readonly DbContext _context;
        protected readonly DbSet<TEntity> Entities;

        public Repository(DbContext context)
        {
            _context = context;
            Entities = context.Set<TEntity>();
        }


        public virtual void Add(TEntity entity)
        {
            Entities.Add(entity);
        }

        public virtual void AddRange(TEntity[] entities)
        {
            Entities.AddRange(entities);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            Entities.AddRange(entities);
        }

        public virtual void Edit(TEntity entity)
        {
            var isDetached = _context.Entry(entity).State == EntityState.Detached;
            if (isDetached)
                Entities.Attach(entity).State = EntityState.Modified;
        }


        public virtual void Remove(TEntity entity)
        {
            Entities.Remove(entity);
        }

        public virtual void Remove(Func<TEntity, bool> predicate)
        {
            IEnumerable<TEntity> records = from x in Entities.Where(predicate) select x;

            foreach (TEntity record in records)
            {
                Entities.Remove(record);
            }
        }

        public virtual IEnumerable<TEntity> GetAll(string[] includeProperties = null)
        {
            IQueryable<TEntity> query = Entities;

            if (includeProperties != null)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));


            return query;
        }

        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, string[] includeProperties = null,
            Func<TEntity, TEntity> select = null)
        {

            var query = Entities.Where(predicate);

            if (includeProperties != null)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return select != null ? query.Select(select) : query;
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            IList<Func<IOrderedQueryable<TEntity>, IOrderedQueryable<TEntity>>> thenOrderBy = null,
            string[] includeProperties = null,
            int? page = null,
            int? pageSize = null,
            Expression<Func<TEntity, bool>> adicionalFilter = null,
            Func<TEntity, TEntity> select = null)
        {
            IQueryable<TEntity> query = Entities;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (adicionalFilter != null)
            {
                query = query.Where(adicionalFilter);
            }

            if (includeProperties != null)
                query = includeProperties.Aggregate(query,
                    (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
            {
                var queryOrder = orderBy(query);
                if (thenOrderBy != null)
                {
                    queryOrder = thenOrderBy.Aggregate(queryOrder, (current, tempThenOrderBy) => tempThenOrderBy(current));
                }

                query = queryOrder;
            }
            if (page != null && pageSize != null)
            {
                //var start = page * pageSize;
                query = query
                    .Skip(page.Value)
                    .Take(pageSize.Value);
            }

            return select != null ? query.Select(select) : query;
        }


        /// <summary>
        /// The first record matching the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <param name="includeProperties"></param>
        /// <returns>A single record containing the first record matching the specified criteria</returns>
        public virtual TEntity First(Expression<Func<TEntity, bool>> predicate, string[] includeProperties = null, Func<TEntity, TEntity> select = null)
        {

            var query = Entities.Where(predicate);

            if (includeProperties != null)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return select != null ? query.Select(select).FirstOrDefault() : query.FirstOrDefault();
        }

        public virtual long Count(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, bool>> adicionalFilter = null)
        {
            IQueryable<TEntity> query = Entities;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (adicionalFilter != null)
            {
                query = query.Where(adicionalFilter);
            }

            return query.Count();
        }
    }
}
