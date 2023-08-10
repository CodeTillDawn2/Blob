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
    }

    protected override void Start()
    {

        base.Start();
        if (!string.IsNullOrEmpty(NervePlan.brain))
        {
            GameObject brainChild = new GameObject(NervePlan.brain);
            brainChild.transform.SetParent(gameObject.transform);
            Brain newBrain = AddComponentFromType(brainChild, NervePlan.brain) as Brain;
            SetConfigurationBasedOnDropdown(NervePlan.brainStats, newBrain);
        }
        if (!string.IsNullOrEmpty(NervePlan.senses))
        {
            GameObject sensesChild = new GameObject(NervePlan.senses);
            sensesChild.transform.SetParent(gameObject.transform);
            Senses newSenses = AddComponentFromType(sensesChild, NervePlan.senses) as Senses;
            SetConfigurationBasedOnDropdown(NervePlan.sensesStats, newSenses);
        }
        if (!string.IsNullOrEmpty(NervePlan.locomotion))
        {
            GameObject locomotionChild = new GameObject(NervePlan.locomotion);
            locomotionChild.transform.SetParent(gameObject.transform);
            Locomotion newLocomotion = AddComponentFromType(locomotionChild, NervePlan.locomotion) as Locomotion;
            SetConfigurationBasedOnDropdown(NervePlan.locomotionStats, newLocomotion);
        }
        if (!string.IsNullOrEmpty(NervePlan.body))
        {
            GameObject bodyChild = transform.Find("Body")?.gameObject ?? new GameObject(NervePlan.body);
            bodyChild.name = NervePlan.body;
            bodyChild.transform.SetParent(gameObject.transform);
            Body newBody = AddComponentFromType(bodyChild, NervePlan.body) as Body;
            SetConfigurationBasedOnDropdown(NervePlan.bodyStats, newBody);
        }
    }

    private Component AddComponentFromType(GameObject parentObject, string type)
    {
        return GOLibrary.instance.AddComponentByTypeName(parentObject, type);
    }

    private void SetConfigurationBasedOnDropdown(string statsName, CharacterSystem characterSystem)
    {
        Type configType = null;
        foreach (var cachedConfig in AIBaker.instance.ConfigurationDataCache.Configurations)
        {
            if (cachedConfig.ConfigurationType.Name == statsName)
            {
                configType = cachedConfig.ConfigurationType;
                break;
            }
        }

        if (configType != null)
        {
            ConfigurationBase newConfiguration = ScriptableObject.CreateInstance(configType) as ConfigurationBase;
            characterSystem.configuration = newConfiguration;
        }
        else
        {
            Debug.LogWarning($"No ConfigurationBase derivative found for type string '{statsName}'");
        }
    }


}
