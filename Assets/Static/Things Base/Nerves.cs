using System;
using UnityEngine;

public abstract class Nerves : CharacterSystem
{
    protected abstract Brain brain { get; }
    protected abstract Senses senses { get; }
    protected abstract Locomotion locomotion { get; }
    protected abstract Body body { get; }

    protected abstract NervePlan NervePlan { get; }


    protected override void Awake()
    {
        base.Awake();
        enabled = true; //Nerves need to wake themselves up as the first part of the character to exist
        
    }


    void OnEnable()
    {

    }

    protected override void Start()
    {

        base.Start();
        CreateAndAttachComponent(NervePlan.Brain, NervePlan.BrainConfig, gameObject);
        CreateAndAttachComponent(NervePlan.Senses, NervePlan.SensesConfig, gameObject);
        CreateAndAttachComponent(NervePlan.Locomotion, NervePlan.LocomotionConfig, gameObject);
        CreateAndAttachComponent(NervePlan.Body, NervePlan.BodyConfig, transform.Find("Body")?.gameObject ?? gameObject, "Body");
        enabled = true;
    }

    private Component AddComponentFromType(GameObject parentObject, string type)
    {
        return GOLibrary.instance.AddComponentByTypeName(parentObject, type);
    }

  
    private void CreateAndAttachComponent(string componentType, string statsName, GameObject parentObject, string fallbackName = null)
    {
        if (string.IsNullOrEmpty(componentType)) return;

        GameObject childObject = new GameObject(fallbackName ?? componentType);
        childObject.transform.SetParent(parentObject.transform);

        Component newComponent = AddComponentFromType(childObject, componentType);

        if (newComponent is CharacterSystem characterSystem)
        {
            ConfigurationBase configInstance = AIBakerData.instance.ConfigurationInstances.GetConfigurationInstance(statsName);

            if (configInstance != null)
            {
                characterSystem.AssignConfiguration(configInstance);
            }
            else
            {
                characterSystem.AssignConfiguration(null);
                Debug.LogWarning($"No ConfigurationBase instance found for type string '{statsName}'");
            }
        }
    }

}
