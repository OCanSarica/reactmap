using core.Bases;
using dal.Bases;
using dal.Entities;
using dal.Models;
using dal.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dal.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DalDbContext _Context;

        private readonly DbSet<User> _Entities;

        public UserRepository(DalDbContext _context)
        {
            _Context = _context;

            _Entities = _context.Set<User>();
        }

        public User Get(long _id)
        {
            return _Entities.SingleOrDefault(s => s.Id == _id);
        }

        public IEnumerable<User> GetAll()
        {
            return _Entities.AsEnumerable();
        }

        public IEnumerable<User> GetAll(Expression<Func<User, bool>> _predicate)
        {
            return _Entities.Where(_predicate);
        }

        public User GetFirst(Expression<Func<User, bool>> _predicate)
        {
            return _Entities.FirstOrDefault(_predicate);
        }

        public User Get(Expression<Func<User, bool>> _predicate)
        {
            return _Entities.SingleOrDefault(_predicate);
        }

        public User Add(User _entity)
        {
            if (_entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            return _Entities.Add(_entity).Entity;
        }

        public async Task<User> AddAsync(User _entity)
        {
            if (_entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            return (await _Entities.AddAsync(_entity)).Entity;
        }

        public void Update(User _entity)
        {
            if (_entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var _oldEntity = Get(_entity.Id);

            foreach (var _property in typeof(User).GetProperties())
            {
                var _newValue = _property.GetValue(_entity);

                _property.SetValue(_oldEntity, _newValue);
            }
        }

        public void Remove(User _entity)
        {
            if (_entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _Entities.Remove(_entity);
        }

        public void Remove(long _id) => _Entities.Remove(Get(_id));

        public List<UserRolePermissionDto> GetWithRolesAndPermissions(long _userId) =>
            _Context.UserRole.
                Where(x => x.UserId == _userId).
                Join(_Context.Role, x => x.RoleId, y => y.Id,
                    (x, y) => new
                    {
                        Role = y,
                    }).
                Join(_Context.RolePermission, ur => ur.Role.Id, rp => rp.RoleId,
                    (ur, rp) => new
                    {
                        ur.Role,
                        RolePermission = rp
                    }).
                Join(_Context.Permission,
                    rpu => rpu.RolePermission.PermissionId, p => p.Id,
                    (rpu, p) => new
                    {
                        rpu.Role,
                        Permission = p
                    }).
                Select(x => new UserRolePermissionDto
                {
                    Permission = x.Permission,
                    Role = x.Role
                }).
                ToList();
    }
}
