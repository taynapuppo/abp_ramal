using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SistemaRamais.Ramais
{
    public abstract class RamalDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Nome { get; set; } = null!;
        public string Numero { get; set; } = null!;
        public string Departamento { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string ConcurrencyStamp { get; set; } = null!;

    }
}