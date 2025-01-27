using Microsoft.AspNetCore.Builder;
using SistemaRamais;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("SistemaRamais.Web.csproj"); 
await builder.RunAbpModuleAsync<SistemaRamaisWebTestModule>(applicationName: "SistemaRamais.Web");

public partial class Program
{
}
