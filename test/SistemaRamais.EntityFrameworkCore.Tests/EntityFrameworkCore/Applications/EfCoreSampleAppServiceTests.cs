using SistemaRamais.Samples;
using Xunit;

namespace SistemaRamais.EntityFrameworkCore.Applications;

[Collection(SistemaRamaisTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<SistemaRamaisEntityFrameworkCoreTestModule>
{

}
