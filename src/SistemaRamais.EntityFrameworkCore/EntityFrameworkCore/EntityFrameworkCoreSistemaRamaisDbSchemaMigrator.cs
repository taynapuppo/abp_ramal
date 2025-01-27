using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SistemaRamais.Data;
using Volo.Abp.DependencyInjection;

namespace SistemaRamais.EntityFrameworkCore;

public class EntityFrameworkCoreSistemaRamaisDbSchemaMigrator
    : ISistemaRamaisDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreSistemaRamaisDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the SistemaRamaisDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<SistemaRamaisDbContext>()
            .Database
            .MigrateAsync();
    }
}
