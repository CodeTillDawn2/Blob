using UnityEngine;

public class DamageTypeLibrary : MonoBehaviour
{
    [SerializeField] public DamageTypeEnum acidDamage;
    [SerializeField] public DamageTypeEnum fireDamage;
    [SerializeField] public DamageTypeEnum piercingDamage;

    public static DamageTypeLibrary instance;

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

    public static DamageTypeEnum AcidDamage
    {
        get { return instance.acidDamage; }
    }
    public static DamageTypeEnum FireDamage
    {
        get { return instance.fireDamage; }
    }
    public static DamageTypeEnum PiercingDamage
    {
        get { return instance.piercingDamage; }
    }
}
