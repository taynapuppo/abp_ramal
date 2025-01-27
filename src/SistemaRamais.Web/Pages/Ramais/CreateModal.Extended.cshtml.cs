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
    public class CreateModalModel : CreateModalModelBase
    {
        public CreateModalModel(IRamaisAppService ramaisAppService)
            : base(ramaisAppService)
        {
        }
    }
}