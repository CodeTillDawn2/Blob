using UnityEngine;
[CreateAssetMenu(fileName = "NervePlan", menuName = "AI/NervePlan")]
public class NervePlan : ScriptableObject
{
    [Header("Brain")]
    [BaseClassRequired(typeof(Brain))]
    public string brain;

    [BaseClassRequiredImplementingInterfaces(typeof(ConfigurationBase), "brain")]
    public string brainStats;

    [Header("Senses")]
    [BaseClassRequired(typeof(Senses))]
    public string senses;

    [BaseClassRequiredImplementingInterfaces(typeof(ConfigurationBase), "senses")]
    public string sensesStats;

    [Header("Locomotion")]
    [BaseClassRequired(typeof(Locomotion))]
    public string locomotion;

    [BaseClassRequiredImplementingInterfaces(typeof(ConfigurationBase), "locomotion")]
    public string locomotionStats;

    [Header("Body")]
    [BaseClassRequired(typeof(Body))]
    public string body;

    [BaseClassRequiredImplementingInterfaces(typeof(ConfigurationBase), "body")]
    public string bodyStats;
}
