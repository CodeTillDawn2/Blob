using UnityEngine;

[CreateAssetMenu(fileName = "NervePlan", menuName = "AI/NervePlan")]
public class NervePlan : ScriptableObject
{
    [Header("Brain")]
    [SerializeField]
    [CompountDependentDropdown("Brain")]
    private string brain;

    public string Brain
    {
        get
        {
            if (brain == "None") return null;
            return brain.Split(":")[0];
        }
    }

    public string BrainConfig
    {
        get
        {
            if (brain == "None") return null;
            return brain.Split(":")[1];
        }
    }

    [Header("Senses")]
    [SerializeField]
    [CompountDependentDropdown("Senses")]
    private string senses;

    public string Senses
    {
        get
        {
            if (senses == "None") return null;
            return senses.Split(":")[0];
        }
    }
    public string SensesConfig
    {
        get
        {
            if (senses == "None") return null;
            return senses.Split(":")[1];
        }
    }

    [Header("Locomotion")]
    [SerializeField]
    [CompountDependentDropdown("Locomotion")]
    private string locomotion;

    public string Locomotion
    {
        get
        {
            if (locomotion == "None") return null;
            return locomotion.Split(":")[0];
        }
    }
    public string LocomotionConfig
    {
        get
        {
            if (locomotion == "None") return null;
            return locomotion.Split(":")[1];
        }
    }

    [Header("Body")]
    [SerializeField]
    [CompountDependentDropdown("Body")]
    private string body;

    public string Body
    {
        get
        {
            if (body == "None") return null;
            return body.Split(":")[0];
        }
    }
    public string BodyConfig
    {
        get
        {
            if (body == "None") return null;
            return body.Split(":")[1];
        }
    }
}