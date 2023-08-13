using System;
using UnityEngine;

public abstract class CharacterSystem : MonoBehaviour
{



    //region template
    #region Unity Methods

    protected virtual void Awake()
    {
        enabled = false;

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
            string test = "";
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
