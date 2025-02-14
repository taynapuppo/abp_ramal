using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaRamais.Ramais;

namespace SistemaRamais.Web.Pages
{
    public class IndexModel : SistemaRamaisPageModel
    {
        private readonly RamaisAppServiceBase _ramaisAppService;

        public IndexModel(RamaisAppServiceBase ramaisAppService)
        {
            _ramaisAppService = ramaisAppService;
        }

    }
}
