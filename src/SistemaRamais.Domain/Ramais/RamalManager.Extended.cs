using System;
using Volo.Abp.Domain.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace SistemaRamais.Ramais
{
    public class RamalManager : RamalManagerBase
    {
        //<suite-custom-code-autogenerated>
        public RamalManager(IRamalRepository ramalRepository, ILookupNormalizer lookupNormalizer)
        : base(ramalRepository, lookupNormalizer)  
    {
    }
        //</suite-custom-code-autogenerated>

        //Write your custom code...
    }
}