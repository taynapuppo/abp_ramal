using Xunit;

namespace SistemaRamais.EntityFrameworkCore;

[CollectionDefinition(SistemaRamaisTestConsts.CollectionDefinitionName)]
public class SistemaRamaisEntityFrameworkCoreCollection : ICollectionFixture<SistemaRamaisEntityFrameworkCoreFixture>
{

}
