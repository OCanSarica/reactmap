using core.Attributes;
using core.Bases;
using dal.Bases;
using dal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dal.Repositories
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly DalDbContext _Context;

        private DbSet<T> _Entities;

        public Repository(DalDbContext _context)
        {
            _Context = _context;

            _Entities = _context.Set<T>();
        }

        public T Get(long _id)
        {
            return _Entities.SingleOrDefault(s => s.Id == _id);
        }

        public IEnumerable<T> GetAll()
        {
            return _Entities.AsEnumerable();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> _predicate)
        {
            return _Entities.Where(_predicate);
        }

        public T GetFirst(Expression<Func<T, bool>> _predicate)
        {
            return _Entities.FirstOrDefault(_predicate);
        }

        public T Get(Expression<Func<T, bool>> _predicate)
        {
            return _Entities.SingleOrDefault(_predicate);
        }

        public T Add(T _entity)
        {
            if (_entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            return _Entities.Add(_entity).Entity;
        }

        public async Task<T> AddAsync(T _entity)
        {
            if (_entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            return (await _Entities.AddAsync(_entity)).Entity;
        }

        public void Update(T _entity)
        {
            if (_entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var _oldEntity = Get(_entity.Id);

            foreach (var _property in typeof(T).GetProperties())
            {
                var _newValue = _property.GetValue(_entity);

                var _columnAttribute = _property.GetCustomAttribute<ColumnAttribute>();

                if (_columnAttribute == null)
                {
                    _property.SetValue(_oldEntity, _newValue);
                }
                else if (!(_columnAttribute.Spatial && _newValue == null))
                {
                    if (!_columnAttribute.Identity)
                    {
                        _property.SetValue(_oldEntity, _newValue);
                    }
                }
            }
        }

        public void Remove(T _entity)
        {
            if (_entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _Entities.Remove(_entity);
        }

        public void Remove(long _id)
        {
            _Entities.Remove(Get(_id));
        }
    }
}
