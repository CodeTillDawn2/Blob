using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class ConfigurationsCache
{
    public List<ConfigurationData> Configurations { get; private set; } = new List<ConfigurationData>();

    public ConfigurationData GetConfigurationData(string typeName)
    {
        return Configurations.FirstOrDefault(c => c.ConfigurationType.FullName == typeName);
    }
}