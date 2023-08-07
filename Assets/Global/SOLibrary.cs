using UnityEngine;

public class SOLibrary : MonoBehaviour
{
    [SerializeField] public GameObjectVariable EmptyGameObjectVariable;
    [SerializeField] public ImpulseVariable EmptyImpulseVariable;
    [SerializeField] public BooleanVariable EmptyBooleanVariable;
    [SerializeField] public FloatVariable EmptyFloatVariable;
    [SerializeField] public GameObjectRuntimeSet EmptyGameObjectRuntimeSet;
    [SerializeField] public Dict_GameObjectToLastSeen EmptyGameObjectToLastSeenDict;
    public static SOLibrary instance { get; set; }

    private void Awake()
    {
        instance = this;
        EmptyGameObjectVariable.Value = null;
        EmptyImpulseVariable.Value = null;
        EmptyFloatVariable.Value = 0;
        EmptyBooleanVariable.Value = false;
        EmptyGameObjectRuntimeSet.RemoveAll();
        EmptyGameObjectToLastSeenDict.Value.Clear();
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
