using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages; // Namespace correto para PageModel
using SistemaRamais.Ramais;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace SistemaRamais.Web.Pages.Graficos
{
    public class IndexModel : PageModel // Herdando de PageModel
    {
        private readonly IRepository<Ramal, Guid> _ramalRepository;

        public IndexModel(IRepository<Ramal, Guid> ramalRepository)
        {
            _ramalRepository = ramalRepository;
        }

        public async Task<IActionResult> OnGet() // Método assíncrono para pegar os dados ao carregar a página
        {
            // Buscando os dados de ramais no banco de dados
            var ramais = await _ramalRepository.GetListAsync(); // Usando GetListAsync para buscar todos os ramais

            // Agrupando os ramais por mês de criação
            var dadosPorMes = ramais
                .GroupBy(r => r.CreationTime.ToString("yyyy-MM")) // Agrupando por ano e mês
                .Select(group => new
                {
                    MesAno = group.Key,
                    TotalRamais = group.Count()
                })
                .OrderBy(x => x.MesAno) // Ordenando pelos meses
                .ToList();

            // Preparando os dados para o gráfico
            var labels = dadosPorMes.Select(x => x.MesAno).ToArray();
            var totalRamais = dadosPorMes.Select(x => x.TotalRamais).ToArray();

            var dadosGrafico = new
            {
                labels = labels,
                datasets = new[]
                {
                    new
                    {
                        label = "Total de Ramais por Mês",
                        data = totalRamais,
                        backgroundColor = "rgba(75, 192, 192, 0.2)",
                        borderColor = "rgba(75, 192, 192, 1)",
                        borderWidth = 1
                    }
                }
            };

            // Passando os dados para a view
            ViewData["DadosGrafico"] = JsonSerializer.Serialize(dadosGrafico);

            return Page(); // Retorna a página Razor
        }
    }
}
