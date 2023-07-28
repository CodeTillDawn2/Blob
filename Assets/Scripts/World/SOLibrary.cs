using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SOLibrary : MonoBehaviour
{
    [SerializeField] public GameObjectVariable EmptyGameObjectVariable;
    public static SOLibrary instance { get; set; }

    private void Awake()
    {
        EmptyGameObjectVariable.Value = null;
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
