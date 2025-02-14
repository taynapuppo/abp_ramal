using Volo.Abp.Application.Dtos;
using System;

namespace SistemaRamais.Ramais
{
    public abstract class GetRamaisInputBase : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? Nome { get; set; }
        public string? Numero { get; set; }
        public string? Departamento { get; set; }
        public string? Email { get; set; }

        public GetRamaisInputBase()
        {

        }
    }
}