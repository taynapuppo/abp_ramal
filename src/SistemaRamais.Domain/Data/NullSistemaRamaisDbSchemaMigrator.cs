using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SistemaRamais.Data;

/* This is used if database provider does't define
 * ISistemaRamaisDbSchemaMigrator implementation.
 */
public class NullSistemaRamaisDbSchemaMigrator : ISistemaRamaisDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
