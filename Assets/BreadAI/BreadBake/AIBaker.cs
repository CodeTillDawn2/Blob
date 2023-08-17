using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AIBaker : MonoBehaviour
{





    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject); // Ensure this object persists between scenes
        EatBread();
    }

    private void FixedUpdate()
    {

    }

    private static void EatBread()
    {
        string json = "";

        json = File.ReadAllText(AIBakerData.instance.BreadConfigurations_Path);
        AIBakerData.instance.BreadConfigurations = SerializationUtility.DeserializeObject<Dictionary<string, ScriptableObject>>(json, false);
        json = File.ReadAllText(AIBakerData.instance.BreadValidConfigurations_Path);
        AIBakerData.instance.BreadValidConfigurations = SerializationUtility.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(json, false);
        json = File.ReadAllText(AIBakerData.instance.BreadDataMembers_Path);
        AIBakerData.instance.BreadDataMembers = SerializationUtility.DeserializeObject<Dictionary<string, List<SimpleMemberInfo>>>(json, false);
        json = File.ReadAllText(AIBakerData.instance.BreadMethods_Path);
        AIBakerData.instance.BreadMethods = SerializationUtility.DeserializeObject<List<ClassData>>(json, false);
        json = File.ReadAllText(AIBakerData.instance.BreadInterfaces_Path);
        AIBakerData.instance.BreadInterfaces = SerializationUtility.DeserializeObject<Dictionary<string, List<SimpleMemberInfo>>>(json, false);
        json = File.ReadAllText(AIBakerData.instance.BreadSystemInterfaces_Path);
        AIBakerData.instance.BreadSystemInterfaces = SerializationUtility.DeserializeObject<Dictionary<Type, List<Type>>>(json, false);
    }


 
}
