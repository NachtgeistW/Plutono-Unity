namespace Assets.Scripts.Util
{
    using Microsoft.Extensions.Configuration;

    public static class IniFile
    {
        public static IConfigurationRoot FromPath(string path) =>
            new ConfigurationBuilder().AddIniFile(path).Build();
    }
}