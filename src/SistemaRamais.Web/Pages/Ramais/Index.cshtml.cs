using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using SistemaRamais.Ramais;
using SistemaRamais.Shared;

namespace SistemaRamais.Web.Pages.Ramais
{
    public abstract class IndexModelBase : AbpPageModel
    {
        public string? NomeFilter { get; set; }
        public string? NumeroFilter { get; set; }
        public string? DepartamentoFilter { get; set; }
        public string? EmailFilter { get; set; }

        protected IRamaisAppService _ramaisAppService;

        public IndexModelBase(IRamaisAppService ramaisAppService)
        {
            _ramaisAppService = ramaisAppService;
        }

        public virtual async Task OnGetAsync()
        {

            await Task.CompletedTask;
        }
    }
}