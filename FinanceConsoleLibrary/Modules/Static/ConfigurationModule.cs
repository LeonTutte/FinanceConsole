using IniParser;
using IniParser.Model;

namespace FinanceConsoleLibrary.Modules.Static;

public static class ConfigurationModule
{
    public static IniData GetConfiguration()
    {
        IniData data;
        var parser = new FileIniDataParser();
        try
        {
            data = parser.ReadFile("settings.ini");
            return data;
        }
        catch (FileNotFoundException)
        {
            LogModule.WriteError("settings file no found");
        }
        catch (FieldAccessException)
        {
            LogModule.WriteError("settings file not accessible");
        }
        return new IniData();
    }
}