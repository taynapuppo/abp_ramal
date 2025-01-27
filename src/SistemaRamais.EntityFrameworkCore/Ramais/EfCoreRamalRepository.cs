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
            string? numero = null,
            string? departamento = null,
            string? responsavel = null,
            string? email = null,
            string? telefone = null,
            CancellationToken cancellationToken = default)
        {

            var query = await GetQueryableAsync();

            query = ApplyFilter(query, filterText, numero, departamento, responsavel, email, telefone);

            var ids = query.Select(x => x.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Ramal>> GetListAsync(
            string? filterText = null,
            string? numero = null,
            string? departamento = null,
            string? responsavel = null,
            string? email = null,
            string? telefone = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, numero, departamento, responsavel, email, telefone);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? RamalConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? numero = null,
            string? departamento = null,
            string? responsavel = null,
            string? email = null,
            string? telefone = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, numero, departamento, responsavel, email, telefone);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Ramal> ApplyFilter(
            IQueryable<Ramal> query,
            string? filterText = null,
            string? numero = null,
            string? departamento = null,
            string? responsavel = null,
            string? email = null,
            string? telefone = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Numero!.Contains(filterText!) || e.Departamento!.Contains(filterText!) || e.Responsavel!.Contains(filterText!) || e.Email!.Contains(filterText!) || e.Telefone!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(numero), e => e.Numero.Contains(numero))
                    .WhereIf(!string.IsNullOrWhiteSpace(departamento), e => e.Departamento.Contains(departamento))
                    .WhereIf(!string.IsNullOrWhiteSpace(responsavel), e => e.Responsavel.Contains(responsavel))
                    .WhereIf(!string.IsNullOrWhiteSpace(email), e => e.Email.Contains(email))
                    .WhereIf(!string.IsNullOrWhiteSpace(telefone), e => e.Telefone.Contains(telefone));
        }
    }
}