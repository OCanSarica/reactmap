using core.Bases;
using dal.Bases;
using dal.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace dal.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DalDbContext _Context;

        public UnitOfWork(DalDbContext _context) => 
            _Context = _context;
     
        public IRepository<T> GetRepository<T>() where T : EntityBase => 
            new Repository<T>(_Context);

        public IUserRepository GetUserRepository() => 
            new UserRepository(_Context);
        
        public void Save() => 
            _Context.SaveChanges();
    }
}
