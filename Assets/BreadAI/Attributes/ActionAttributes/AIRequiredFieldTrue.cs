using System;

/// <summary>
/// Use this to decorate character system public methods with fields which 
/// are required to evaluate to a boolean true.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AIRequiredFieldTrueAttribute : BreadAIAttributeBase
{
    public Type FieldToEvaluate { get; }

    public AIRequiredFieldTrueAttribute(Type fieldToEvaluate)
    {
        FieldToEvaluate = fieldToEvaluate;
    }
}
