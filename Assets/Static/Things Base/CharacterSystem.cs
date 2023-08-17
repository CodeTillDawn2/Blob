using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public abstract class CharacterSystem : MonoBehaviour
{

    public readonly Dictionary<Type, object> interfaces = new Dictionary<Type, object>();

    private Type myType;

    public bool Am<U>(out U result) where U : class
    {
        result = null;

        if (this == null)
        {
            return false;
        }

        Type instanceType = this.GetType();
        Type targetType = typeof(U);

        List<Type> interfaceTypes;

        // If the dictionary contains the type of the instance
        if (AIBakerData.instance.BreadSystemInterfaces.TryGetValue(instanceType, out interfaceTypes))
        {
            if (interfaceTypes.Contains(targetType)) // The Contains method works for both HashSet and List
            {
                result = this as U;
                return true;
            }
        }

        // If you reach this point, then there's a casting problem.
        // Log the scenario where the instance cannot be safely cast to the desired interface (consider delaying or skipping this in critical paths)
        Debug.LogError($"Attempt to cast {instanceType.Name} to {targetType.Name} failed. Check pre-baking process.");
        return false;
    }

    /// <summary>
    /// Checks if the current instance implements all specified interfaces.
    /// </summary>
    /// <remarks>
    /// This method prioritizes speed over type safety, as the type information is pre-baked using reflection.
    /// Ensure accuracy of the pre-baking process to avoid type mismatches at runtime.
    /// </remarks>
    /// <param name="interfaceTypes">The array of interface types to check against.</param>
    /// <returns>Returns <c>true</c> if all specified interfaces are implemented; otherwise, <c>false</c>.</returns>
    public bool Am(params Type[] interfaceTypes)
    {
        List<Type> knownInterfaces;
        if (AIBakerData.instance.BreadSystemInterfaces.TryGetValue(this.GetType(), out knownInterfaces))
        {
            foreach (var targetType in interfaceTypes)
            {
                if (!knownInterfaces.Contains(targetType))
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Retrieves the instance of the specified interface type.
    /// </summary>
    /// <typeparam name="T">The interface type to retrieve.</typeparam>
    /// <remarks>
    /// This method prioritizes speed over type safety due to pre-baked reflection data.
    /// Ensure accuracy of the pre-baking process to mitigate potential type mismatches.
    /// </remarks>
    /// <returns>Returns the instance of the specified interface type if found; otherwise, <c>null</c>.</returns>
    public T I<T>() where T : class
    {
        interfaces.TryGetValue(typeof(T), out object foundObject);
        return foundObject as T;
    }




    //region template
    #region Unity Methods

    protected virtual void Awake()
    {
        enabled = false;
        SetupInterfaceDictionary();
    }

    protected virtual void Start()
    {

    }


    #endregion

    #region Interface Fields

    #endregion

    #region Interface Methods

    #endregion

    #region Private methods
    private void SetupInterfaceDictionary()
    {
        Type currentType = this.GetType();
        if (AIBakerData.instance.BreadSystemInterfaces.TryGetValue(currentType, out var interfaceTypes))
        {
            foreach (var iface in interfaceTypes)
            {
                interfaces[iface] = this;
            }
        }
    }
    #endregion

    public void AssignConfiguration(string configName)
    {
        if (string.IsNullOrEmpty(configName))
        {
            Debug.LogError("Configuration name is null or empty.");
            return;
        }

        // 1. Get the ConfigurationBase instance.
        if (!AIBakerData.instance.BreadConfigurations.TryGetValue(configName, out var configObj))
        {
            Debug.LogError($"Configuration {configName} not found.");
            return;
        }

        var configInstance = configObj as ConfigurationBase;
        if (configInstance == null)
        {
            Debug.LogError($"Config {configName} is not of type ConfigurationBase.");
            return;
        }

        // 2. Loop through the interfaces of CharacterSystem.
        Type currentType = this.GetType();
        if (!AIBakerData.instance.BreadSystemInterfaces.TryGetValue(currentType, out var interfaceList))
        {
            Debug.LogError($"No interfaces found for type: {currentType.FullName}");
            return;
        }

        bool AllImplemented = true;

        if (!AIBakerData.instance.BreadDataMembers.TryGetValue(currentType.FullName, out var members))
        {
            Debug.LogError($"No members found for type: {currentType.FullName}");
            return;
        }

        // Loop through the members of the class
        foreach (var member in members)
        {
            var memberInfo = member as SimpleDataMemberInfo;
            if (memberInfo == null)
            {
                Debug.LogWarning($"Failed to cast member to SimpleDataMemberInfo.");
                continue;
            }

            bool memberImplemented = false;

            // For each interface implemented by CharacterSystem
            foreach (var iface in interfaceList)
            {
                if (iface == null)
                {
                    Debug.LogWarning("Encountered a null interface in the interface list.");
                    continue;
                }

                // Check if the ConfigurationBase instance also implements this interface
                List<Type> configInterfaces;
                if (!AIBakerData.instance.BreadSystemInterfaces.TryGetValue(configInstance.GetType(), out configInterfaces)
                    || !configInterfaces.Contains(iface))
                {
                    continue;  // Skip to the next interface if configInstance does not implement iface
                }

                var ConfigMember = memberInfo.FindMatchingMember(configInstance);
                if (ConfigMember != null)
                {
                    memberImplemented = true;
                    TransferInitialConfigurations(configInstance, this, (SimpleDataMemberInfo)ConfigMember, memberInfo);
                    break; // Exit the inner loop as we have found a matching interface
                }
            }

            if (!memberImplemented)
            {
                AllImplemented = false;
            }
        }

        if (!AllImplemented)
        {
            Debug.LogWarning("Not all interfaces matched between " + currentType + " and " + configInstance.GetType());
        }
    }





    private void TransferInitialConfigurations(ConfigurationBase config, CharacterSystem character, SimpleDataMemberInfo configInfo, SimpleDataMemberInfo systemInfo)
    {

        if (configInfo.VariableType.FullName != systemInfo.VariableType.FullName)
        {
            Debug.LogError($"Expected a {systemInfo.VariableType.FullName}. Got: {configInfo.VariableType}");
            return;
        }
        try
        {
            // Get value from config using SimpleDataMemberInfo
            var configValue = configInfo.GetValue(config);

            if (typeof(Component).IsAssignableFrom(configInfo.VariableType))
            {
                systemInfo.SetValue(character, GOLibrary.instance.AddComponentByTypeName(character.gameObject, systemInfo.VariableType.AssemblyQualifiedName));
            }
            else
            {
                systemInfo.SetValue(character, configValue); 
            }

            // Set value to character using a different, appropriate SimpleDataMemberInfo

            
        }
        catch (TargetInvocationException invE)
        {
            if (invE.InnerException != null)
            {
                Debug.LogError($"Inner exception: {invE.InnerException.Message}");
            }
            else
            {
                Debug.LogError(invE.Message);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to transfer value for {configInfo.MemberName}: {e.Message}");
        }
    }



}
