using System;

/// <summary>
/// Use this to decorate character system public methods with fields which 
/// are required, and have a certain boolean that evaluates false.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AIRequiredEvalBoolAttribute : BreadAIAttributeBase
{
    public Type FieldToEvaluate { get; }

    public bool Evaluator { get; }

    public AIRequiredEvalBoolAttribute(Type fieldToEvaluate, bool evaluator)
    {
        FieldToEvaluate = fieldToEvaluate;
        Evaluator = evaluator;
    }
}
