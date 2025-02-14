using System.Threading.Tasks;

namespace SistemaRamais.Data;

public interface ISistemaRamaisDbSchemaMigrator
{
    Task MigrateAsync();
}
