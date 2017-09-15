using AirQualityPublish.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.OpenAccess;

namespace AirQualityPublish.Model.Repositories
{
    public class Repository<TEntity> where TEntity : IEntity
    {
        private OpenAccessContext db;

        public Repository(OpenAccessContext context)
        {
            db = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return db.GetAll<TEntity>();
        }

        public void Add(TEntity entity)
        {
            db.Add(entity);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            db.Add(entities);
        }

        public void Delete(TEntity entity)
        {
            db.Delete(entity);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            db.Delete(entities);
        }
    }
}
