using System;

/// <summary>
/// Use this to decorate character system public methods with fields which 
/// are required to evaluate to a boolean false.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AIRequiredFieldFalseAttribute : CustomAIAttributeBase
{
    public Type FieldToEvaluate { get; }

    public AIRequiredFieldFalseAttribute(Type fieldToEvaluate)
    {
        FieldToEvaluate = fieldToEvaluate;
    }
}
