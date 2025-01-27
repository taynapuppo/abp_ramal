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
        public string Numero { get; set; }
        public string Departamento { get; set; }
        public string Responsavel { get; set; }
        public string Email { get; set; }

        public string Telefone { get; set; }

        protected RamalBase()
        {

        }

        public RamalBase(Guid id, string numero, string departamento, string responsavel, string email, string telefone)
        {

            Id = id;
            Check.NotNull(numero, nameof(numero));
            Check.Length(numero, nameof(numero), RamalConsts.NumeroMaxLength, RamalConsts.NumeroMinLength);
            Check.NotNull(departamento, nameof(departamento));
            Check.Length(departamento, nameof(departamento), RamalConsts.DepartamentoMaxLength, RamalConsts.DepartamentoMinLength);
            Check.NotNull(responsavel, nameof(responsavel));
            Check.Length(responsavel, nameof(responsavel), RamalConsts.ResponsavelMaxLength, RamalConsts.ResponsavelMinLength);
            Check.NotNull(email, nameof(email));
            Check.Length(email, nameof(email), RamalConsts.EmailMaxLength, RamalConsts.EmailMinLength);
            Check.NotNull(telefone, nameof(telefone));
            Check.Length(telefone, nameof(telefone), RamalConsts.TelefoneMaxLength, RamalConsts.TelefoneMinLength);


            Numero = numero;
            Departamento = departamento;
            Responsavel = responsavel;
            Email = email;
            Telefone = telefone;

        }

    }
}