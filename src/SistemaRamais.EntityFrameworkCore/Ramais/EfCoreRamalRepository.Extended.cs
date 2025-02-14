using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SistemaRamais.EntityFrameworkCore;

namespace SistemaRamais.Ramais
{
    public class EfCoreRamalRepository : EfCoreRamalRepositoryBase, IRamalRepository
    {
        public EfCoreRamalRepository(IDbContextProvider<SistemaRamaisDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}