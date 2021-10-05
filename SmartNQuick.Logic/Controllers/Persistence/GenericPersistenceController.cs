﻿//@BaseCode
//MdStart
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.Controllers.Persistence
{
#if ACCOUNT_ON
    using SmartNQuick.Logic.Modules.Security;
    using System.Reflection;

    [Authorize(AllowModify = true)]
#endif
    internal abstract partial class GenericPersistenceController<C, E> : GenericController<C, E>
        where C : Contracts.IIdentifiable
        where E : Entities.IdentityEntity, Contracts.ICopyable<C>, C, new()
    {
        public override bool IsTransient => false;
        public DbSet<E> Set() => Context.Set<C, E>();
        public IQueryable<E> QueryableSet() => Context.Set<C, E>();
        protected GenericPersistenceController(DataContext.IContext context) : base(context)
        {
        }
        protected GenericPersistenceController(ControllerObject other) : base(other)
        {
        }

        #region Count
        public override Task<int> CountAsync()
        {
            return Context.CountAsync<C, E>();
        }
        public override Task<int> CountByAsync(string predicate)
        {
            return Context.CountByAsync<C, E>(predicate);
        }
        #endregion Count

        #region Query
        public override async Task<C> GetByIdAsync(int id)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.GetBy).ConfigureAwait(false);
#endif
            var result = await Context.GetByIdAsync<C, E>(id).ConfigureAwait(false);

            if (result == null)
            {
                throw new Exception($"Invalid id '{id}'.");
            }
            result = BeforeReturn(result);
            return await BeforeReturnAsync(result).ConfigureAwait(false);
        }
        public override async Task<IEnumerable<C>> GetAllAsync()
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.GetAll).ConfigureAwait(false);
#endif
            return (await Context.GetAllAsync<C, E>()
                                 .ConfigureAwait(false))
                                 .Select(e => BeforeReturn(e));
        }
        public override async Task<IEnumerable<C>> QueryAllAsync(string predicate)
        {
#if ACCOUNT_ON
            await CheckAuthorizationAsync(GetType(), MethodBase.GetCurrentMethod(), AccessType.Query).ConfigureAwait(false);
#endif
            return (await Context.QueryAllAsync<C, E>(predicate)
                                 .ConfigureAwait(false))
                                 .Select(e => BeforeReturn(e));
        }
        #endregion Query

        #region Insert
        internal override Task<E> ExecuteInsertEntityAsync(E entity)
        {
            return Context.InsertAsync<C, E>(entity);
        }
        #endregion Insert

        #region Update
        internal override Task<E> ExecuteUpdateEntityAsync(E entity)
        {
            return Context.UpdateAsync<C, E>(entity);
        }
        #endregion Update

        #region Delete
        internal override Task ExecuteDeleteEntityAsync(E entity)
        {
            return Context.DeleteAsync<C, E>(entity.Id);
        }
        #endregion Delete
    }
}
//MdEnd