using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SistemaRamais.Ramais
{
    public partial interface IRamalRepository : IRepository<Ramal, Guid>
    {

        Task DeleteAllAsync(
            string? filterText = null,
            string? nome = null,
            string? numero = null,
            string? departamento = null,
            string? email = null,
            CancellationToken cancellationToken = default);
        Task<List<Ramal>> GetListAsync(
                    string? filterText = null,
                    string? nome = null,
                    string? numero = null,
                    string? departamento = null,
                    string? email = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? nome = null,
            string? numero = null,
            string? departamento = null,
            string? email = null,
            CancellationToken cancellationToken = default);
    }
}