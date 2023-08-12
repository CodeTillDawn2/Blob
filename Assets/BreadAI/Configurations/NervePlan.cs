using UnityEngine;

[CreateAssetMenu(fileName = "NervePlan", menuName = "AI/NervePlan")]
public class NervePlan : ScriptableObject
{
    [Header("Brain")]
    [BaseClassRequired("Brain")]
    public string brain;

    [Header("Senses")]
    [BaseClassRequired("Senses")]
    public string senses;

    [Header("Locomotion")]
    [BaseClassRequired("Locomotion")]
    public string locomotion;

    [Header("Body")]
    [BaseClassRequired("Body")]
    public string body;
}
