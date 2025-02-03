using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaRamais.Ramais;
using SistemaRamais.Application.Contracts.Ramais;

namespace SistemaRamais.Web.Pages.Relatorios
{
    public class IndexModel : PageModel
    {
        private readonly RamaisAppService _ramaisAppService;

        public List<RamalRelatorioDto> Ramais { get; set; } = new();

        public IndexModel(RamaisAppService ramaisAppService)
        {
            _ramaisAppService = ramaisAppService;
        }

        public async Task OnGetAsync()
        {
            Ramais = await _ramaisAppService.GetRelatorioAsync();
        }
    }
}
