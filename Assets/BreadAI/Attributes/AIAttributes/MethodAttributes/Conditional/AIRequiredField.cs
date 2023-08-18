using System;

/// <summary>
/// Use this to decorate character system public methods with fields which 
/// are required for the action to occur.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AIRequiredFieldAttribute : BreadAIAttributeBase
{
    public Type FieldToEvaluate { get; }

    public AIRequiredFieldAttribute(Type fieldToEvaluate)
    {
        FieldToEvaluate = fieldToEvaluate;
    }
}
