using System;

/// <summary>
/// Use this to decorate character system public methods with fields which 
/// are required to evaluate a float.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AIRequiredEvalIntegerAttribute : BreadAIAttributeBase
{
    public Type FieldToEvaluate { get; }

    public OperatorEnum Evaluator { get; }

    public AIRequiredEvalIntegerAttribute(Type fieldToEvaluate, OperatorEnum evaluator)
    {
        FieldToEvaluate = fieldToEvaluate;
        Evaluator = evaluator;
    }


    public enum OperatorEnum
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan
    }

}
