﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(Guid Id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetList(Expression<Func<T, bool>> where);
        IQueryable<T> _GetList(Expression<Func<T, bool>> where);
        IQueryable<T> _GetList(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        Task<bool> Save();
    }
}
