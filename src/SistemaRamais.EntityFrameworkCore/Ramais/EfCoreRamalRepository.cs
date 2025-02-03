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
using FuzzySharp;

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
            // Aplica os filtros iniciais
            var query = ApplyFilter((await GetQueryableAsync()), filterText, nome, numero, departamento, email);

            // Ordena os resultados, caso o sorting seja especificado
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? RamalConsts.GetDefaultSorting(false) : sorting);

            // Limita os resultados antes de carregar em memória
            var limitedQuery = query.Take(500); // Limita para 500 resultados, ajuste conforme necessário

            // Carrega os resultados limitados em memória
            var ramaisList = await limitedQuery.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);

            // Verifica se nome não está nulo ou vazio
            if (!string.IsNullOrWhiteSpace(nome))
            {
                // Log para verificar o nome que está sendo passado
                Console.WriteLine($"Aplicando busca fuzzy para o nome: {nome}");

                // Aplica o fuzzy search apenas nos resultados limitados
                ramaisList = ramaisList
                    .Where(r =>
                    {
                        var ratio = Fuzz.WeightedRatio(r.NormalizedName, nome);
                        Console.WriteLine($"Comparando '{r.NormalizedName}' com '{nome}' - Razão fuzzy: {ratio}");
                        return ratio > 50;
                    })
                    .ToList();
            }

            return ramaisList;
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

            if (!string.IsNullOrWhiteSpace(nome))
            {
                var nomeNormalized = nome.ToLower();
                query = query.Where(r => EF.Functions.ToTsVector("portuguese", r.NormalizedName)
                                        .Matches(EF.Functions.PlainToTsQuery("portuguese", nomeNormalized)));
            }

            query = query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.NormalizedName!.Contains(filterText!) || e.Numero!.Contains(filterText!) || e.Departamento!.Contains(filterText!) || e.NormalizedEmail!.Contains(filterText!))
                .WhereIf(!string.IsNullOrWhiteSpace(nome), e => e.NormalizedName.Contains(nome))
                .WhereIf(!string.IsNullOrWhiteSpace(numero), e => e.Numero.Contains(numero))
                .WhereIf(!string.IsNullOrWhiteSpace(departamento), e => e.Departamento.Contains(departamento))
                .WhereIf(!string.IsNullOrWhiteSpace(email), e => e.NormalizedEmail.Contains(email));

            return query;
        }
    }
}