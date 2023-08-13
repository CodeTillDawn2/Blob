using System.Reflection;
using System;

[Serializable]
public class SimpleParameterInfo
{
    public string Name { get; set; }
    public string ParameterType { get; set; }

    public SimpleParameterInfo() { }

    public SimpleParameterInfo(ParameterInfo parameterInfo)
    {
        Name = parameterInfo.Name;
        ParameterType = parameterInfo.ParameterType.ToString();
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        SimpleParameterInfo other = (SimpleParameterInfo)obj;
        return Name == other.Name && ParameterType == other.ParameterType;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + (Name?.GetHashCode() ?? 0);
            hash = hash * 23 + (ParameterType?.GetHashCode() ?? 0);
            return hash;
        }
    }
}
