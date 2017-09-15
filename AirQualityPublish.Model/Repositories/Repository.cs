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
        protected OpenAccessContext DB { get; set; }

        public Repository(OpenAccessContext context)
        {
            DB = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return DB.GetAll<TEntity>();
        }

        public void Add(TEntity entity)
        {
            DB.Add(entity);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            DB.Add(entities);
        }

        public void Delete(TEntity entity)
        {
            DB.Delete(entity);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            DB.Delete(entities);
        }
    }
}
