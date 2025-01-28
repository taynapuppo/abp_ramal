using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SistemaRamais.EntityFrameworkCore;

namespace SistemaRamais.Ramais
{
    public abstract class EfCoreRamalRepositoryBase : EfCoreRepository<SistemaRamaisDbContext, Ramal, Guid>
    {
        public EfCoreRamalRepositoryBase(IDbContextProvider<SistemaRamaisDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
            string? nome = null,
            string? numero = null,
            string? departamento = null,
            string? email = null,
            CancellationToken cancellationToken = default)
        {

            var query = await GetQueryableAsync();

            query = ApplyFilter(query, filterText, nome, numero, departamento, email);

            var ids = query.Select(x => x.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Ramal>> GetListAsync(
            string? filterText = null,
            string? nome = null,
            string? numero = null,
            string? departamento = null,
            string? email = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, nome, numero, departamento, email);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? RamalConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? nome = null,
            string? numero = null,
            string? departamento = null,
            string? email = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, nome, numero, departamento, email);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Ramal> ApplyFilter(
            IQueryable<Ramal> query,
            string? filterText = null,
            string? nome = null,
            string? numero = null,
            string? departamento = null,
            string? email = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Nome!.Contains(filterText!) || e.Numero!.Contains(filterText!) || e.Departamento!.Contains(filterText!) || e.Email!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(nome), e => e.Nome.Contains(nome))
                    .WhereIf(!string.IsNullOrWhiteSpace(numero), e => e.Numero.Contains(numero))
                    .WhereIf(!string.IsNullOrWhiteSpace(departamento), e => e.Departamento.Contains(departamento))
                    .WhereIf(!string.IsNullOrWhiteSpace(email), e => e.Email.Contains(email));
        }
    }
}