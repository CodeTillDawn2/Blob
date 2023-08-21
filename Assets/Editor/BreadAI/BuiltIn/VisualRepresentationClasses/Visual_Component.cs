using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Visual_Component<T> : Visual_Component_Base where T : Component
{
    private GameObject prefabInstance;
    private string prefabPath = "";

    public Visual_Component(string name, string returnType, string memberType, List<Visual_Attribute> attributes) :
        base(name, returnType, memberType, attributes)
    {
        prefabPath = $"Assets/BreadAI/BuiltIn/BreadComponents/BreadPrefab_{name}.prefab";

        Debug.LogWarning("!!!");

        // Check if the prefab already exists
        GameObject existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (existingPrefab != null)
        {
            // Load the existing prefab contents
            prefabInstance = PrefabUtility.LoadPrefabContents(prefabPath);
        }
        else
        {
            try
            {
                // Create a new prefab
                GameObject tempObject = new GameObject();
                tempObject.AddComponent<T>();
                //tempObject.hideFlags = HideFlags.HideAndDontSave; // Makes the object hidden and not saved to the scene
                PrefabUtility.SaveAsPrefabAsset(tempObject, prefabPath);
                Editor.DestroyImmediate(tempObject);
                prefabInstance = PrefabUtility.LoadPrefabContents(prefabPath);
                
            }
            catch (Exception ex)
            {
                Debug.LogWarning("ex1");
                string test = "";
            }
         
        }

        TargetComponent = prefabInstance.GetComponent<T>();
        componentEditor = Editor.CreateEditor(TargetComponent);

    }



    public void SaveComponentToPrefab()
    {
        Debug.LogWarning("!!11!");
        // Update the component of type T on the root
        T existingComponent = prefabInstance.GetComponent<T>();

        if (existingComponent == null)
        {
            existingComponent = prefabInstance.AddComponent<T>();
        }
        try
        {
            EditorUtility.CopySerialized(TargetComponent, existingComponent);

            // Save the changes to the prefab
            PrefabUtility.SaveAsPrefabAsset(prefabInstance, prefabPath);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("ex2");
            string test = "";
        }

       
    }

    public void ReplacePrefabWithSelf()
    {
        // Replace the prefab with itself (the current instance)
        PrefabUtility.SaveAsPrefabAsset(prefabInstance, prefabPath);
    }
}
