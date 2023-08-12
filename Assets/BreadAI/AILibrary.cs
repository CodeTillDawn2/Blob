using UnityEngine;

public class AILibrary : MonoBehaviour
{
    public static AILibrary instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern check
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject); // Ensure this object persists between scenes
    }

    public bool AreAttributeConditionsMet(GameObject go, MethodData methodData)
    {
        foreach (var evaluator in methodData.AttributeEvaluators)
        {
            if (!evaluator(go))
            {
                return false;
            }
        }
        return true;
    }


}
