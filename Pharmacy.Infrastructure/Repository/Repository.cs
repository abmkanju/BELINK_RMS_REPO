using Pharmacy.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private PharmacyContext cxt = null;
        private DbSet<T> Entity = null;
        public Repository(PharmacyContext cxt)
        {
            this.cxt = cxt;
            Entity = this.cxt.Set<T>();
        }
        public void Delete(object id)
        {
            T obj = Entity.Find(id);
            Entity.Remove(obj);
        }
        public IEnumerable<T> GetAll()
        {
            try
            {
                return Entity.ToList();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public List<T> GetAllList()
        {
            try
            {
                return Entity.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public T GetById(object id)
        {
            return Entity.Find(id);
        }
        public T Insert(T obj)
        {
            return Entity.Add(obj);
        }

        public void InsertRange(List<T> obj)
        {
            Entity.AddRange(obj);
        }
        public void Update(T obj)
        {
            Entity.Attach(obj);
            cxt.Entry(obj).State = EntityState.Modified;
        }
        public void Save()
        {
            cxt.SaveChanges();
        }

        public bool Any()
        {
            return Entity.Any();
        }
    }
}