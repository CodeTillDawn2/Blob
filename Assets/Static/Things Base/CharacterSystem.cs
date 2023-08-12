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
                        var sourceInstance = AIBakerData.instance.ConfigurationInstances.GetConfigurationInstance(propertyMapping.SourceInstanceType.FullName);
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






    private void AssignConfigurationProperties(ConfigurationBase config)
    {
        if (AIBakerData.instance.BakedConfigurationAssignmentLogic.ContainsKey(GetType().FullName))
        {
            foreach (var interfaceMapping in AIBakerData.instance.BakedConfigurationAssignmentLogic[GetType().FullName])
            {
                foreach (var propertyMapping in interfaceMapping.Value)
                {
                    if (config.GetType().GetInterface(interfaceMapping.Key) != null)
                    {
                        var value = propertyMapping.SourceProperty.GetValue(config);
                        Debug.Log("Successfully logged " + propertyMapping.SourceProperty.Name + " to " + propertyMapping.DestinationProperty.Name);
                        propertyMapping.DestinationProperty.SetValue(this, value);
                    }
                }
            }
        }


    }




    ///// <summary>
    ///// Assigns component properies, which are fields which hold components for use by the system.
    ///// Naming convention: IUse...
    ///// </summary>
    //private void AssignComponentProperies()
    //{
    //    {
    //        if ((this is IUseMeshRenderer me))
    //            me.meshRenderer = GetComponent<MeshRenderer>();
    //    }
    //    {
    //        if ((this is IUseRigidbody me))
    //            me.rigidbody = GetComponent<Rigidbody>();
    //    }
    //}
    ///// <summary>
    ///// Assigns properies, which are fields which are assigned by stats
    ///// Naming convention: IHave...
    ///// </summary>
    //private void AssignConfigurationProperties(ConfigurationBase config)
    //{
    //    {
    //        if ((this is IHaveDigestDamageDealt me) && (config is IHaveDigestDamageDealt it))
    //            me.DigestDamageDealt = it.DigestDamageDealt;
    //    }
    //    {
    //        if ((this is IHaveEyes me) && (config is IHaveEyes it))
    //            me.Eyes = it.Eyes;
    //    }
    //    {
    //        if ((this is IHaveGrowthSpeed me) && (config is IHaveGrowthSpeed it))
    //            me.GrowthSpeed = it.GrowthSpeed;
    //    }
    //    {
    //        if ((this is IHaveHitPoints me) && (config is IHaveHitPoints it))
    //            me.HitPoints = it.HitPoints;
    //    }
    //    {
    //        if ((this is IHaveMass me) && (config is IHaveMass it))
    //            me.Mass = it.Mass;
    //    }
    //    {
    //        if ((this is IHaveMassPerCubicFoot me) && (config is IHaveMassPerCubicFoot it))
    //            me.MassPerCubicFoot = it.MassPerCubicFoot;
    //    }
    //    {
    //        if ((this is IHaveMaxTentacles me) && (config is IHaveMaxTentacles it))
    //            me.MaxTentacles = it.MaxTentacles;
    //    }
    //    {
    //        if ((this is IHaveMoveSpeed me) && (config is IHaveMoveSpeed it))
    //            me.MoveSpeed = it.MoveSpeed;
    //    }
    //    {
    //        if ((this is IHaveNutrition me) && (config is IHaveNutrition it))
    //            me.Nutrition = it.Nutrition;
    //    }
    //    {
    //        if ((this is IHaveRotateSpeed me) && (config is IHaveRotateSpeed it))
    //            me.RotateSpeed = it.RotateSpeed;
    //    }
    //    {
    //        if ((this is ICanSee me) && (config is ICanSee it))
    //        {
    //            me.OnlySeeMask = it.OnlySeeMask;
    //            me.ThingsSeen = it.ThingsSeen;
    //            me.SightDistance = it.SightDistance;
    //        }

    //    }
    //    {
    //        if ((this is IHaveSuckSpeed me) && (config is IHaveSuckSpeed it))
    //            me.SuckSpeed = it.SuckSpeed;
    //    }
    //    {
    //        if ((this is IHaveTentacleHitSpeed me) && (config is IHaveTentacleHitSpeed it))
    //            me.TentacleHitSpeed = it.TentacleHitSpeed;
    //    }
    //    {
    //        if ((this is IHaveTentacleReach me) && (config is IHaveTentacleReach it))
    //        {
    //            me.MinTentacleReach = it.MinTentacleReach;
    //            me.MaxTentacleReach = it.MaxTentacleReach;
    //        }
    //    }
    //    {
    //        if ((this is IHaveThingsInMyStomach me) && (config is IHaveThingsInMyStomach it))
    //        {
    //            me.DragInsideStomach = it.DragInsideStomach;
    //            me.AngularDragInsideStomach = it.AngularDragInsideStomach;
    //        }
    //    }
    //}
    /// <summary>
    /// Assigns conditionals, which are fields which booleans
    /// Naming convention: IAm...
    /// </summary>
    //private void AssignConfigurationConditionals(ConfigurationBase config)
    //{
    //    {
    //        if ((this is IAmAlive me) && (config is IAmAlive it))
    //            me.IsAlive.Value = it.IsAlive.Value;
    //    }

    //}

}
