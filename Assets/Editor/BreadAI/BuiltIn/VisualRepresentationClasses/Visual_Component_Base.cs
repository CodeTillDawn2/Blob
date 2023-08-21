using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
[HideReferenceObjectPicker]
public class Visual_Component_Base : Visual_Member
{
    public Component TargetComponent { get; protected set; }
    [NonSerialized]
    public Editor componentEditor;

    public Visual_Component_Base(string name, string returnType, string memberType, List<Visual_Attribute> attributes) 
        : base(name, returnType, memberType, attributes)
    {

    }

    public virtual void DrawComponentInspector()
    {
        if (componentEditor != null)
        {
            componentEditor.OnInspectorGUI();
        }
    }

    //public virtual void Dispose()
    //{
    //    if (componentEditor != null)
    //    {
    //        UnityEngine.Object.DestroyImmediate(TargetComponent.gameObject);
    //        componentEditor = null;
    //    }
    //}
}