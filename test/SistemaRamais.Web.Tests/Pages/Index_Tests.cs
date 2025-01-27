using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace SistemaRamais.Pages;

[Collection(SistemaRamaisTestConsts.CollectionDefinitionName)]
public class Index_Tests : SistemaRamaisWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
