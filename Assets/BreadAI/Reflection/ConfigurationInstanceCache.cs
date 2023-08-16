using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ConfigurationInstanceCache
{
    public List<ConfigurationData> Configurations { get; private set; } = new List<ConfigurationData>();

    public ConfigurationBase GetConfigurationInstance(string typeName)
    {
        return Configurations.FirstOrDefault(c => c.ConfigurationInstance.GetType().FullName == typeName)?.ConfigurationInstance;
    }
}