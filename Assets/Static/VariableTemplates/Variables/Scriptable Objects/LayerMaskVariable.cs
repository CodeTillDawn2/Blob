using UnityEngine;

[CreateAssetMenu(fileName = "LayerMaskVariable", menuName = "ScriptableObjects/LayerMask")]
public class LayerMaskVariable : ScriptableObject, IConfigDefault
{
    public Shortcuts.LayerMasks Value;
}
