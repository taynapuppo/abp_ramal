using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaRamais.Ramais
{
    public interface IRamalSearchService
    {
        Task<List<(string Ramal, string Nome, int Score)>> BuscarNomeAsync(string query);
    }
}
