using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace SistemaRamais.Ramais
{
    public abstract class RamalBase : FullAuditedAggregateRoot<Guid>
    {
        public string? Nome { get; set; }
        public string? Numero { get; set; }
        public string? Departamento { get; set; }
    
        public string? Email { get; set; }

        public string? NormalizedName { get; set; }
        public string? NormalizedEmail { get; set; }
        public string? NormalizedDepartamento { get; set; }

        protected RamalBase()
        {

        }

        public RamalBase(Guid id, string nome, string numero, string departamento, string email, string normalizedName, string normalizedEmail, string normalizedDepartamento)
        {

            Id = id;
            Check.NotNull(nome, nameof(nome));
            Check.Length(nome, nameof(nome), RamalConsts.NomeMaxLength, RamalConsts.NomeMinLength);
            Check.NotNull(numero, nameof(numero));
            Check.Length(numero, nameof(numero), RamalConsts.NumeroMaxLength, RamalConsts.NumeroMinLength);
            Check.NotNull(departamento, nameof(departamento));
            Check.Length(departamento, nameof(departamento), RamalConsts.DepartamentoMaxLength, RamalConsts.DepartamentoMinLength);
            Check.NotNull(email, nameof(email));
            Check.Length(email, nameof(email), RamalConsts.EmailMaxLength, RamalConsts.EmailMinLength);

            Nome = nome;
            Numero = numero;
            Departamento = departamento;
            Email = email;
            NormalizedName = normalizedName;
            NormalizedEmail = normalizedEmail;
            NormalizedDepartamento = normalizedDepartamento;

        }

    }
}