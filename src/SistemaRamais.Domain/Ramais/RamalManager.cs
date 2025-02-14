using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;
using Microsoft.AspNetCore.Identity;

namespace SistemaRamais.Ramais
{
    public abstract class RamalManagerBase : DomainService
    {
        private readonly IRamalRepository _ramalRepository;
        private readonly ILookupNormalizer _lookupNormalizer;

        public RamalManagerBase(IRamalRepository ramalRepository, ILookupNormalizer lookupNormalizer)
        {
            _ramalRepository = ramalRepository;
            _lookupNormalizer = lookupNormalizer;
        }

        public virtual async Task<Ramal> CreateAsync(
            string nome, string numero, string departamento, string email,
            string normalizedName, string normalizedEmail, string normalizedDepartamento)
        {
            Check.NotNullOrWhiteSpace(nome, nameof(nome));
            Check.Length(nome, nameof(nome), RamalConsts.NomeMaxLength, RamalConsts.NomeMinLength);
            Check.NotNullOrWhiteSpace(numero, nameof(numero));
            Check.Length(numero, nameof(numero), RamalConsts.NumeroMaxLength, RamalConsts.NumeroMinLength);
            Check.NotNullOrWhiteSpace(departamento, nameof(departamento));
            Check.Length(departamento, nameof(departamento), RamalConsts.DepartamentoMaxLength, RamalConsts.DepartamentoMinLength);
            Check.NotNullOrWhiteSpace(email, nameof(email));
            Check.Length(email, nameof(email), RamalConsts.EmailMaxLength, RamalConsts.EmailMinLength);

            var ramal = new Ramal(
                GuidGenerator.Create(),
                nome, numero, departamento, email,
                normalizedName, normalizedEmail, normalizedDepartamento
            );

            return await _ramalRepository.InsertAsync(ramal);
        }


        public virtual async Task<Ramal> UpdateAsync(
            Guid id,
            string nome, string numero, string departamento, string email,
            string normalizedName, string normalizedEmail, string normalizedDepartamento,
            [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(nome, nameof(nome));
            Check.Length(nome, nameof(nome), RamalConsts.NomeMaxLength, RamalConsts.NomeMinLength);
            Check.NotNullOrWhiteSpace(numero, nameof(numero));
            Check.Length(numero, nameof(numero), RamalConsts.NumeroMaxLength, RamalConsts.NumeroMinLength);
            Check.NotNullOrWhiteSpace(departamento, nameof(departamento));
            Check.Length(departamento, nameof(departamento), RamalConsts.DepartamentoMaxLength, RamalConsts.DepartamentoMinLength);
            Check.NotNullOrWhiteSpace(email, nameof(email));
            Check.Length(email, nameof(email), RamalConsts.EmailMaxLength, RamalConsts.EmailMinLength);

            var ramal = await _ramalRepository.GetAsync(id);

            ramal.Nome = nome;
            ramal.Numero = numero;
            ramal.Departamento = departamento;
            ramal.Email = email;
            ramal.NormalizedName = normalizedName;
            ramal.NormalizedEmail = normalizedEmail;
            ramal.NormalizedDepartamento = normalizedDepartamento;

            ramal.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _ramalRepository.UpdateAsync(ramal);
        }
    }
}
