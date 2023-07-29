using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SOLibrary : MonoBehaviour
{
    [SerializeField] public GameObjectVariable EmptyGameObjectVariable;
    [SerializeField] public ImpulseVariable EmptyImpulseVariable;
    [SerializeField] public BooleanVariable EmptyBooleanVariable;
    public static SOLibrary instance { get; set; }

    private void Awake()
    {
        instance = this;
        EmptyGameObjectVariable.Value = null;
        EmptyImpulseVariable.Value = null;
        EmptyBooleanVariable.Value = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
