using System;

/// <summary>
/// Use this to decorate character system public methods with fields which 
/// are required for the action to occur.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AIRequiredFieldExistsAttribute : CustomAIAttributeBase
{
    public Type FieldToEvaluate { get; }

    public AIRequiredFieldExistsAttribute(Type fieldToEvaluate)
    {
        FieldToEvaluate = fieldToEvaluate;
    }
}
