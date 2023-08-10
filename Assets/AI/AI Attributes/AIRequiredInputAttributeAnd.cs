using System;

/// <summary>
/// Use this to decorate character system public methods with fields which are required
/// as a parameter for this method. If multiple are provided, all are needed.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AIRequiredInputAndAttribute : CustomAIAttributeBase
{
    public Type[] InputTypes { get; }

    public AIRequiredInputAndAttribute(params Type[] inputTypes)
    {
        InputTypes = inputTypes;
    }
}
