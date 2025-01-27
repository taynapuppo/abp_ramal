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
    public class EditModalModel : EditModalModelBase
    {
        public EditModalModel(IRamaisAppService ramaisAppService)
            : base(ramaisAppService)
        {
        }
    }
}