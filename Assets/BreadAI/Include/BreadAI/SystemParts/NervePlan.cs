using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NervePlan", menuName = "AI/NervePlan")]
public class NervePlan : ScriptableObject
{

    //Brain
    [SerializeField]
    [ValueDropdown("BrainTypes")]
    private string brain;
    public string Brain { get { return brain; } }

    public IEnumerable BrainTypes
    {
        get
        {
            return GetDropdownList("Brain");
        }
    }
    [Required]
    [ShowIf(nameof(IsBrainSelected))]
    [ValueDropdown("BrainConfigurations")]
    [SerializeField]
    private string brainConfig;
    public string BrainConfig { get { return brainConfig; } }

    public IEnumerable BrainConfigurations
    {
        get
        {
            return GetConfigurations("Brain", brain);
        }
    }




    // Senses
    [SerializeField]
    [ValueDropdown("SensesTypes")]
    private string senses;
    public string Senses { get { return senses; } }

    public IEnumerable SensesTypes
    {
        get
        {
            return GetDropdownList("Senses");
        }
    }

    [Required]
    [ShowIf(nameof(IsSensesSelected))]
    [ValueDropdown("SensesConfigurations")]
    [SerializeField]
    private string sensesConfig;
    public string SensesConfig { get { return sensesConfig; } }

    public IEnumerable SensesConfigurations
    {
        get
        {
            return GetConfigurations("Senses", senses);
        }
    }

    // Locomotion
    [SerializeField]
    [ValueDropdown("LocomotionTypes")]
    private string locomotion;
    public string Locomotion { get { return locomotion; } }

    public IEnumerable LocomotionTypes
    {
        get
        {
            return GetDropdownList("Locomotion");
        }
    }

    [Required]
    [ShowIf(nameof(IsLocomotionSelected))]
    [ValueDropdown("LocomotionConfigurations")]
    [SerializeField]
    private string locomotionConfig;
    public string LocomotionConfig { get { return locomotionConfig; } }

    public IEnumerable LocomotionConfigurations
    {
        get
        {
            return GetConfigurations("Locomotion", locomotion);
        }
    }

    // Body
    [SerializeField]
    [ValueDropdown("BodyTypes")]
    private string body;
    public string Body { get { return body; } }

    public IEnumerable BodyTypes
    {
        get
        {
            return GetDropdownList("Body");
        }
    }

    [Required]
    [ShowIf(nameof(IsBodySelected))]
    [ValueDropdown("BodyConfigurations")]
    [SerializeField]
    private string bodyConfig;
    public string BodyConfig { get { return bodyConfig; } }

    public IEnumerable BodyConfigurations
    {
        get
        {
            return GetConfigurations("Body", body);
        }
    }

    private IEnumerable GetDropdownList(string key)
    {
        var result = new ValueDropdownList<string>();
        if (AIBakerData.Instance.BreadValidConfigurations.TryGetValue(key, out var configs))
        {
            foreach (var k in configs.Keys)
            {
                result.Add(k, k);
            }
            result.Insert(0, new ValueDropdownItem<string>("None", ""));
        }
        return result;
    }

    private IEnumerable GetConfigurations(string key, string subKey)
    {
        var result = new ValueDropdownList<string>();
        if (AIBakerData.Instance.BreadValidConfigurations.TryGetValue(key, out var configs))
        {
            if (configs.TryGetValue(subKey, out var values))
            {
                foreach (var value in values)
                {
                    result.Add(value, value);
                }
            }
        }
        return result;
    }

    public bool IsBrainSelected() => IsSelected(brain);
    public bool IsSensesSelected() => IsSelected(senses);
    public bool IsLocomotionSelected() => IsSelected(locomotion);
    public bool IsBodySelected() => IsSelected(body);

    public bool IsSelected(string value)
    {
        return !string.IsNullOrEmpty(value) && value != "None";
    }
}
