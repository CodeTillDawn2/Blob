using UnityEngine;

public class SOLibrary : MonoBehaviour
{
    public static SOLibrary instance { get; private set; }

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

    public T Create<T>() where T : ScriptableObject
    {
        return ScriptableObject.CreateInstance<T>();
    }
}
