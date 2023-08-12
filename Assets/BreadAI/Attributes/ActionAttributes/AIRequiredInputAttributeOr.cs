using System;

/// <summary>
/// Use this to decorate character system public methods with fields which are required
/// as a parameter for this method. If multiple are provided, only one is needed.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AIRequiredInputOrAttribute : CustomAIAttributeBase
{
    public Type[] InputTypes { get; }

    public AIRequiredInputOrAttribute(params Type[] inputTypes)
    {
        InputTypes = inputTypes;
    }
}
