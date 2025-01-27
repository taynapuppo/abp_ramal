using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace SistemaRamais.Ramais
{
    public abstract class RamalManagerBase : DomainService
    {
        protected IRamalRepository _ramalRepository;

        public RamalManagerBase(IRamalRepository ramalRepository)
        {
            _ramalRepository = ramalRepository;
        }

        public virtual async Task<Ramal> CreateAsync(
        string numero, string departamento, string responsavel, string email, string telefone)
        {
            Check.NotNullOrWhiteSpace(numero, nameof(numero));
            Check.Length(numero, nameof(numero), RamalConsts.NumeroMaxLength, RamalConsts.NumeroMinLength);
            Check.NotNullOrWhiteSpace(departamento, nameof(departamento));
            Check.Length(departamento, nameof(departamento), RamalConsts.DepartamentoMaxLength, RamalConsts.DepartamentoMinLength);
            Check.NotNullOrWhiteSpace(responsavel, nameof(responsavel));
            Check.Length(responsavel, nameof(responsavel), RamalConsts.ResponsavelMaxLength, RamalConsts.ResponsavelMinLength);
            Check.NotNullOrWhiteSpace(email, nameof(email));
            Check.Length(email, nameof(email), RamalConsts.EmailMaxLength, RamalConsts.EmailMinLength);
            Check.NotNullOrWhiteSpace(telefone, nameof(telefone));
            Check.Length(telefone, nameof(telefone), RamalConsts.TelefoneMaxLength, RamalConsts.TelefoneMinLength);

            var ramal = new Ramal(
                GuidGenerator.Create(),
                numero, departamento, responsavel, email, telefone
             );

            return await _ramalRepository.InsertAsync(ramal);
        }

        public virtual async Task<Ramal> UpdateAsync(
            Guid id,
            string numero, string departamento, string responsavel, string email, string telefone, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(numero, nameof(numero));
            Check.Length(numero, nameof(numero), RamalConsts.NumeroMaxLength, RamalConsts.NumeroMinLength);
            Check.NotNullOrWhiteSpace(departamento, nameof(departamento));
            Check.Length(departamento, nameof(departamento), RamalConsts.DepartamentoMaxLength, RamalConsts.DepartamentoMinLength);
            Check.NotNullOrWhiteSpace(responsavel, nameof(responsavel));
            Check.Length(responsavel, nameof(responsavel), RamalConsts.ResponsavelMaxLength, RamalConsts.ResponsavelMinLength);
            Check.NotNullOrWhiteSpace(email, nameof(email));
            Check.Length(email, nameof(email), RamalConsts.EmailMaxLength, RamalConsts.EmailMinLength);
            Check.NotNullOrWhiteSpace(telefone, nameof(telefone));
            Check.Length(telefone, nameof(telefone), RamalConsts.TelefoneMaxLength, RamalConsts.TelefoneMinLength);

            var ramal = await _ramalRepository.GetAsync(id);

            ramal.Numero = numero;
            ramal.Departamento = departamento;
            ramal.Responsavel = responsavel;
            ramal.Email = email;
            ramal.Telefone = telefone;

            ramal.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _ramalRepository.UpdateAsync(ramal);
        }

    }
}