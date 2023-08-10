using System;
using UnityEngine;

public abstract class CharacterSystem : MonoBehaviour
{


    //region template
    #region Unity Methods

    protected virtual void Awake()
    {
        enabled = false;
        AssignConfiguration(configuration);
        enabled = true;
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
        AssignComponentProperies();
        if (config != null)
        {
            AssignConfigurationProperties(config);
            //AssignConfigurationConditionals(config);
        }

    }


    private void AssignComponentProperies()
    {
        if (AIBaker.instance.BakedMappings.ContainsKey(GetType().FullName))
        {
            foreach (var interfaceMapping in AIBaker.instance.BakedMappings[GetType().FullName])
            {
                foreach (var propertyMapping in interfaceMapping.Value)
                {
                    if (typeof(Component).IsAssignableFrom(propertyMapping.DestinationProperty.PropertyType))
                    {
                        Debug.Log("Successfully logged " + propertyMapping.SourceProperty + " to " + propertyMapping.DestinationProperty);
                        propertyMapping.DestinationProperty.SetValue(this, GetComponent(propertyMapping.DestinationProperty.PropertyType));
                    }
                }
            }
        }
    }

    private void AssignConfigurationProperties(ConfigurationBase config)
    {
        if (AIBaker.instance.BakedMappings.ContainsKey(GetType().FullName))
        {
            foreach (var interfaceMapping in AIBaker.instance.BakedMappings[GetType().FullName])
            {
                foreach (var propertyMapping in interfaceMapping.Value)
                {
                    if (config.GetType().GetInterface(interfaceMapping.Key) != null)
                    {
                        var value = propertyMapping.SourceProperty.GetValue(config);
                        Debug.Log("Successfully logged " + propertyMapping.SourceProperty + " to " + propertyMapping.DestinationProperty);
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
