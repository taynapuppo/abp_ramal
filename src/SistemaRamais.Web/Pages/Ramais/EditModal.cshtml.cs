using SistemaRamais.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using SistemaRamais.Ramais;

namespace SistemaRamais.Web.Pages.Ramais
{
    public abstract class EditModalModelBase : SistemaRamaisPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public RamalUpdateViewModel Ramal { get; set; }

        protected IRamaisAppService _ramaisAppService;

        public EditModalModelBase(IRamaisAppService ramaisAppService)
        {
            _ramaisAppService = ramaisAppService;

            Ramal = new();
        }

        public virtual async Task OnGetAsync()
        {
            var ramal = await _ramaisAppService.GetAsync(Id);
            Ramal = ObjectMapper.Map<RamalDto, RamalUpdateViewModel>(ramal);

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _ramaisAppService.UpdateAsync(Id, ObjectMapper.Map<RamalUpdateViewModel, RamalUpdateDto>(Ramal));
            return NoContent();
        }
    }

    public class RamalUpdateViewModel : RamalUpdateDto
    {
    }
}