using UnityEngine;

[CreateAssetMenu(fileName = "UnityLayerVariable", menuName = "ScriptableObjects/UnityLayer")]
public class UnityLayerVariable : ScriptableObject, IConfigDefault
{
    public Shortcuts.UnityLayers Value;
}
