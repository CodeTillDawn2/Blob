using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class Nerves : CharacterSystem
{
    protected virtual Brain brain { get; private set; }
    protected virtual Senses senses { get; private set; }
    protected virtual Locomotion locomotion { get; private set; }
    protected virtual Body body { get; private set; }

    protected virtual NervePlan NervePlan { get; private set; }


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
        brain = (Brain)CreateAndAttachSystem(NervePlan.Brain, NervePlan.BrainConfig, gameObject);
        senses = (Senses)CreateAndAttachSystem(NervePlan.Senses, NervePlan.SensesConfig, gameObject);
        locomotion = (Locomotion)CreateAndAttachSystem(NervePlan.Locomotion, NervePlan.LocomotionConfig, gameObject);
        body = (Body)CreateAndAttachSystem(NervePlan.Body, NervePlan.BodyConfig, transform.Find("Body")?.gameObject ?? gameObject, "Body");
        enabled = true;
        if (senses != null) senses.enabled = true;
        if (locomotion != null) locomotion.enabled = true;
        if (body != null) body.enabled = true;
        if (brain != null) brain.enabled = true;
    }

    private Component AddComponentFromType(GameObject parentObject, string type)
    {
        return GOLibrary.instance.AddComponentByTypeName(parentObject, type);
    }


    private CharacterSystem CreateAndAttachSystem(string componentType, string statsName, GameObject parentObject, string fallbackName = null)
    {
        if (string.IsNullOrEmpty(componentType)) return null;

        GameObject childObject = new GameObject(fallbackName ?? componentType);
        childObject.transform.SetParent(parentObject.transform);

        Component newComponent = AddComponentFromType(childObject, componentType);

        if (newComponent is CharacterSystem characterSystem)
        {
            characterSystem.AssignConfiguration(statsName);
            return characterSystem;
        }
        return null;
    }


}
