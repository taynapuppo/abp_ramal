using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaRamais.Ramais;

namespace SistemaRamais.Web.Pages
{
    public class IndexModel : SistemaRamaisPageModel
    {
        private readonly RamaisAppServiceBase _ramaisAppService;

        public int TotalRamais { get; set; }
        public int TotalDepartamentos { get; set; }
        public int TotalUsuariosAtivos { get; set; }

        public IndexModel(RamaisAppServiceBase ramaisAppService)
        {
            _ramaisAppService = ramaisAppService;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            // Chama os métodos do serviço para obter as estatísticas
            TotalRamais = await _ramaisAppService.GetTotalRamaisAsync();
            TotalDepartamentos = await _ramaisAppService.GetTotalDepartamentosAsync();
            TotalUsuariosAtivos = await _ramaisAppService.GetTotalUsuariosAtivosAsync();

            // Retorna a página com as estatísticas preenchidas
            return Page();
        }
    }
}
