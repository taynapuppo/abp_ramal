using SistemaRamais.Samples;
using Xunit;

namespace SistemaRamais.EntityFrameworkCore.Domains;

[Collection(SistemaRamaisTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<SistemaRamaisEntityFrameworkCoreTestModule>
{

}
