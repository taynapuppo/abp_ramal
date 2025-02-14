using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace SistemaRamais.Ramais
{
    public abstract class RamalUpdateDtoBase : IHasConcurrencyStamp
    {
        [Required]
        [StringLength(RamalConsts.NomeMaxLength, MinimumLength = RamalConsts.NomeMinLength)]
        public string Nome { get; set; } = null!;
        [Required(AllowEmptyStrings = true)]
        [StringLength(RamalConsts.NumeroMaxLength, MinimumLength = RamalConsts.NumeroMinLength)]
        public string Numero { get; set; } = null!;
        [Required(AllowEmptyStrings = true)]
        [StringLength(RamalConsts.DepartamentoMaxLength, MinimumLength = RamalConsts.DepartamentoMinLength)]
        public string Departamento { get; set; } = null!;
        [Required(AllowEmptyStrings = true)]
        [StringLength(RamalConsts.EmailMaxLength, MinimumLength = RamalConsts.EmailMinLength)]
        public string Email { get; set; } = null!;

        public string ConcurrencyStamp { get; set; } = null!;
    }
}