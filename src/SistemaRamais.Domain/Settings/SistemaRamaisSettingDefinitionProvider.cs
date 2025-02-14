using Volo.Abp.Settings;

namespace SistemaRamais.Settings;

public class SistemaRamaisSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(SistemaRamaisSettings.MySetting1));
    }
}
