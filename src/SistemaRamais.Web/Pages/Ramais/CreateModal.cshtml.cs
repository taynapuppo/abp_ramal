using SistemaRamais.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaRamais.Ramais;

namespace SistemaRamais.Web.Pages.Ramais
{
    public abstract class CreateModalModelBase : SistemaRamaisPageModel
    {

        [BindProperty]
        public RamalCreateViewModel Ramal { get; set; }

        protected IRamaisAppService _ramaisAppService;

        public CreateModalModelBase(IRamaisAppService ramaisAppService)
        {
            _ramaisAppService = ramaisAppService;

            Ramal = new();
        }

        public virtual async Task OnGetAsync()
        {
            Ramal = new RamalCreateViewModel();

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            await _ramaisAppService.CreateAsync(ObjectMapper.Map<RamalCreateViewModel, RamalCreateDto>(Ramal));
            return NoContent();
        }
    }

    public class RamalCreateViewModel : RamalCreateDto
    {
    }
}