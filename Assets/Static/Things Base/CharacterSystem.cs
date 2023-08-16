using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSystem : MonoBehaviour
{

    public readonly Dictionary<Type, object> interfaces = new Dictionary<Type, object>();


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
        foreach (var iface in AIBakerData.instance.BreadSystemInterfaces[GetType()])
        {
            interfaces[iface] = this;
        }
    }

    #endregion
    public ConfigurationBase configuration { get; set; }

    public abstract Type[] ExpectedStatsInterfaces { get; }

    public void AssignConfiguration(ConfigurationBase config = null)
    {

        AssignComponentProperties();
        PigSenses senses = PigSenses.instance;
        if (config != null)
        {
            //AssignConfigurationProperties(config);
            //AssignConfigurationConditionals(config);
        }
        enabled = true;
    }


    private void AssignComponentProperties()
    {

        PigSenses senses = PigSenses.instance;
        GameObject go = senses.gameObject;
        string gopath = go.name;
        if (AIBakerData.instance.BakedConfigurationAssignmentLogic.ContainsKey(GetType().FullName))
        {
            var test22 = AIBakerData.instance.BakedConfigurationAssignmentLogic[GetType().FullName];
            foreach (var interfaceMapping in AIBakerData.instance.BakedConfigurationAssignmentLogic[GetType().FullName])
            {
                string test = interfaceMapping.Key + "|" + interfaceMapping.Value;
                foreach (var propertyMapping in interfaceMapping.Value)
                {
                    string test2 = propertyMapping.SourceProperty + "|" + propertyMapping.DestinationProperty;
                    // Debugging: Print the names of the Source and Destination properties.
                    Debug.Log($"Property Mapping - Source: {propertyMapping.SourceProperty.Name}, Destination: {propertyMapping.DestinationProperty.Name}");

                    if (typeof(Component).IsAssignableFrom(propertyMapping.DestinationProperty.PropertyType))
                    {
                        // Add the component of the desired type if it doesn't already exist.
                        Component componentInstance = GetComponent(propertyMapping.DestinationProperty.PropertyType);
                        if (componentInstance == null)
                        {
                            componentInstance = gameObject.AddComponent(propertyMapping.DestinationProperty.PropertyType);
                        }

                        Debug.Log("Successfully added/logged (Component) " + propertyMapping.SourceProperty.Name + " to " + propertyMapping.DestinationProperty.Name);
                        propertyMapping.DestinationProperty.SetValue(this, componentInstance);


                        Component[] componentsOnObject = GetComponents<Component>();
                        string componentNames = "Components on GameObject after assignment: ";
                        foreach (var component in componentsOnObject)
                        {
                            componentNames += component.GetType().Name + ", ";
                        }
                        Debug.Log(componentNames.TrimEnd(',', ' ')); // This will log all the component names attached to the GameObject.
                    }
                    else
                    {
                        // For non-Component types, retrieve the value from the source.
                        var sourceInstance = AIBakerData.instance.AllConfigInstances[propertyMapping.SourceInstanceType];
                        var valueFromSource = propertyMapping.SourceProperty.GetValue(sourceInstance);

                        // Debugging: Print the values of the sourceInstance and valueFromSource.
                        Debug.Log($"SourceInstance Type: {sourceInstance?.GetType().Name ?? "null"}, ValueFromSource: {valueFromSource}");

                        // If value from source is null, create a new instance
                        if (valueFromSource == null)
                        {
                            var newInstance = SOLibrary.Create(propertyMapping.DestinationProperty.PropertyType);
                            if (newInstance != null)
                            {
                                Debug.Log($"Created new instance of type: {newInstance.GetType().Name}");
                                propertyMapping.DestinationProperty.SetValue(this, newInstance);
                                continue; // Skip to the next propertyMapping iteration since this one is handled.
                            }
                        }
                        // Otherwise, set the value on the destination.
                        Debug.Log("Successfully logged " + propertyMapping.SourceProperty.Name + " to " + propertyMapping.DestinationProperty.Name);
                        propertyMapping.DestinationProperty.SetValue(this, valueFromSource);
                    }
                }
            }
        }
    }
}
